namespace SaaS.DataAccess.Exceptions.Application.WorkSite
{
    public class WorkSiteNameAlreadyExistsException : Exception
    {
        public WorkSiteNameAlreadyExistsException()
        {

        }

        public WorkSiteNameAlreadyExistsException(string message) : base(message)
        {

        }

        public WorkSiteNameAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
