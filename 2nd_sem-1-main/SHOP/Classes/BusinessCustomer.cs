using System;
namespace SHOP.Classes;

public class BusinessCustomer : User
{
    public string Company { get; private set; }
    public string CVR { get; private set; }
    public string ContactPerson { get; set; }
    
    //When a business logs in they see their name, and then the Company
    public override string DisplayName => $"{ContactPerson} ({Company})";

    public BusinessCustomer(string cvr, string company, string contactPerson, string email)
    {
        CVR = cvr;
        Company = company;
        ContactPerson = contactPerson;
        Email = email;
    }
}

