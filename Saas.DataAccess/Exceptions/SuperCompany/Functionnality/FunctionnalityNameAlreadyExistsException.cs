namespace SaaS.DataAccess.Exceptions.SuperCompany.Functionnality
{
    public class FunctionnalityNameAlreadyExistsException : Exception
    {
        public FunctionnalityNameAlreadyExistsException()
        {
            
        }

        public FunctionnalityNameAlreadyExistsException(string message) : base(message)
        {
            
        }

        public FunctionnalityNameAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {
            
        }
    }
}
