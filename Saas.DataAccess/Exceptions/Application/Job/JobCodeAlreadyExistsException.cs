namespace SaaS.DataAccess.Exceptions.Application.Job
{
    public class JobCodeAlreadyExistsException : Exception
    {
        public JobCodeAlreadyExistsException()
        {

        }

        public JobCodeAlreadyExistsException(string message) : base(message)
        {

        }

        public JobCodeAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
