namespace SaaS.DataAccess.Exceptions.Application.Connection
{
    public class UserIsNotEnabledException : Exception
    {
        public UserIsNotEnabledException()
        {

        }

        public UserIsNotEnabledException(string message) : base(message)
        {

        }

        public UserIsNotEnabledException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
