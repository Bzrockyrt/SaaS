namespace SaaS.DataAccess.Exceptions.Application.Subsidiary
{
    public class SubsidiaryCodeAlreadyExistsException : Exception
    {
        public SubsidiaryCodeAlreadyExistsException()
        {

        }

        public SubsidiaryCodeAlreadyExistsException(string message) : base(message)
        {

        }

        public SubsidiaryCodeAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
