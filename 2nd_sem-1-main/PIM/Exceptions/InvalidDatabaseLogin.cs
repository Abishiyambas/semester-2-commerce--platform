using System;

namespace PIM.Exceptions
{
    public class InvalidDatabaseLogin : Exception
    {
        public InvalidDatabaseLogin ( string message ) : base ( message )
        {

        }

        public InvalidDatabaseLogin ( string message, Exception inner )
        {

        }
    }
}