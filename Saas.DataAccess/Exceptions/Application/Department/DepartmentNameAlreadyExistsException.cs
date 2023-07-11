namespace SaaS.DataAccess.Exceptions.Application.Department
{
    public class DepartmentNameAlreadyExistsException : Exception
    {
        public DepartmentNameAlreadyExistsException()
        {

        }

        public DepartmentNameAlreadyExistsException(string message) : base(message)
        {

        }

        public DepartmentNameAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
