namespace SaaS.DataAccess.Exceptions.Application.Subsidiary
{
    public class SubsidiaryNameAlreadyExistsException : Exception
    {
        public SubsidiaryNameAlreadyExistsException()
        {

        }

        public SubsidiaryNameAlreadyExistsException(string message) : base(message)
        {

        }

        public SubsidiaryNameAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
