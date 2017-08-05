using System;

namespace MatchMaker.UI.Exceptions
{
    public class NoConnectionException : Exception
    {
        public NoConnectionException(string message)
            : base(message)
        {
            
        }

        public NoConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}