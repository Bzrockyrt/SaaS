namespace SaaS.DataAccess.Exceptions.SuperCompany.Functionnality
{
    public class FunctionnalityCodeAlreadyExistsException : Exception
    {
        public FunctionnalityCodeAlreadyExistsException()
        {

        }

        public FunctionnalityCodeAlreadyExistsException(string message) : base(message)
        {

        }

        public FunctionnalityCodeAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
