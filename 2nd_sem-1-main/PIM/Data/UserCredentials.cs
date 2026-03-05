using PIM.Exceptions;
using System;
using System.IO;

namespace PIM_Data;

public class UserCredentials
{
    private string name, password;
    static private UserCredentials user = new UserCredentials();

    static public string Name
    {
        get { return user.name; }
    }

    static public string Password
    {
        get { return user.password; }
    }

    private UserCredentials ()
    {
        ReadCredentials ();
    }

    private void ReadCredentials ()
    {
        using ( StreamReader streamReader = new StreamReader ( CredentialsFilePath () ) )
        {
            string line;
            while ( ( line = streamReader.ReadLine () ) != null )
            {
                string[] values = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                name = values[0];
                password = values[1];
            }
        }
    }

    private static string CredentialsFilePath()
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string projectRoot = Directory.GetParent(baseDirectory).Parent.Parent.Parent.FullName;
        string credentialsFilePath = Path.Combine ( projectRoot, "Credentials.txt" );

        if ( !Path.Exists ( credentialsFilePath ) )
            throw ( new InvalidDatabaseLogin ( $"could not find {credentialsFilePath}" ) );

        return credentialsFilePath;
    }
}