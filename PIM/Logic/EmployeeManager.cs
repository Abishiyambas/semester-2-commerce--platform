using System.Threading.Tasks;
namespace PIM_Logic;

public class EmployeeManager
{
    private static Employee? activeEmployee;

    public static Employee? ActiveEmployee
    {
        get { return activeEmployee; }
        set { activeEmployee = value; }
    }

    private readonly PIM_Data.IOperations dbOperations;

    public EmployeeManager ( PIM_Data.IOperations dbOperations )
    {
        this.dbOperations = dbOperations;
        activeEmployee = null;
    }

    async public Task<Employee?> GetAccount ( string id, string password )
    {
        Employee? account = null;
        int idParsed;
        if ( int.TryParse ( id, out idParsed ) )
            return await dbOperations.SelectSingle ( "employees", "employee_id", idParsed, "password", password, reader => new Employee ( reader ) );
        else
            return account;
    }

    public async Task<bool> DoesEmployeeExist ( string id, string password )
    {
        int idParsed;
        if ( int.TryParse ( id, out idParsed ) )
            return await dbOperations.Count ( "employees", "employee_id", idParsed, "password", password ) > 0;
        else
            return false;
    }
}