namespace SaaS.DataAccess.Exceptions.Application.Job
{
    public class JobNameAlreadyExistsException : Exception
    {
        public JobNameAlreadyExistsException()
        {

        }

        public JobNameAlreadyExistsException(string message) : base(message)
        {

        }

        public JobNameAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
