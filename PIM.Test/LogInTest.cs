using Avalonia.Controls;
using PIM_Logic;
using PIM_Presentation;

namespace PIM.Test
{
    class LogInTest
    {
        private Pim pim;
        private EmployeeManager employeeManager;

        [SetUp]
        public void Setup ()
        {
            AppEnvironment.IsTestEnvironment = true;
            PIM_Data.DatabaseUtility databaseUtility = new PIM_Data.DatabaseUtility();
            pim = new Pim ( databaseUtility );
            employeeManager = new EmployeeManager ( databaseUtility );
        }

        

        [Test]
        public async Task LogIn ()
        {
            Employee? notLogIn = EmployeeManager.ActiveEmployee;

            LogInPage logInPage = new LogInPage(null, pim, employeeManager);

            if ( await employeeManager.DoesEmployeeExist ( "1", "password" ) )
            {
                if ( await logInPage.LogIn ( "1", "password" ) )
                {
                    Assert.That ( EmployeeManager.ActiveEmployee, Is.Not.EqualTo ( notLogIn ) );
                }
            }
        }

        [Test]
        public async Task NonAdminDeleteEmployee ()
        {
            LogInPage logInPage = new LogInPage(null, pim, employeeManager);

            await logInPage.LogIn ( "2", "password" ); // login as NonAdmin

            // Create and insert mock employee
            Employee mockEmployee = new Employee("Ove", "password", false);
            mockEmployee = await pim.InsertEmployee ( mockEmployee );

            if ( await employeeManager.DoesEmployeeExist ( mockEmployee.Employee_id.ToString (), mockEmployee.Password ) )
            {
                await pim.DeleteEmployee ( mockEmployee );
                Assert.That ( await employeeManager.DoesEmployeeExist ( mockEmployee.Employee_id.ToString (), mockEmployee.Password ), Is.True );
                await logInPage.LogIn ( "1", "password" ); // login as Admin
                await pim.DeleteEmployee ( mockEmployee );
            }
            else
            {
                Assert.Fail ( "mock employee does not exists in database" );
            }
        }

        [Test]
        public async Task AdminDeleteEmployee ()
        {
            LogInPage logInPage = new LogInPage(null, pim, employeeManager);

            await logInPage.LogIn ( "1", "password" ); // login as Admin

            // Create and insert mock employee
            Employee mockEmployee = new Employee("Ove", "password", false);
            mockEmployee = await pim.InsertEmployee ( mockEmployee );

            if ( await employeeManager.DoesEmployeeExist ( mockEmployee.Employee_id.ToString (), mockEmployee.Password ) )
            {
                await pim.DeleteEmployee ( mockEmployee );
                Assert.That ( await employeeManager.DoesEmployeeExist ( mockEmployee.Employee_id.ToString (), mockEmployee.Password ), Is.False );
            }
            else
            {
                Assert.Fail ( "mock employee does not exists in database" );
            }
        }

        [Test]
        public async Task EmployeeManagementAccessNonAuth ()
        {
            LogInPage logInPage = new LogInPage(null, pim, employeeManager);

            await logInPage.LogIn ( "2", "password" ); // Not Admin

            UserControl employeesPage = PIM_Presentation.MainTabControl.GetEmployeePage ( pim, employeeManager );

            Assert.That ( employeesPage.GetType (), Is.EqualTo ( typeof ( UnauthorizedPage ) ) );
        }

        [Test]
        public async Task EmployeeManagementAccessAuth ()
        {
            LogInPage logInPage = new LogInPage(null, pim, employeeManager);

            await logInPage.LogIn ( "1", "password" ); // Admin

            UserControl employeesPage = PIM_Presentation.MainTabControl.GetEmployeePage ( pim, employeeManager );

            Assert.That ( employeesPage.GetType (), Is.EqualTo ( typeof ( EmployeesPage ) ) );
        }
    }
}