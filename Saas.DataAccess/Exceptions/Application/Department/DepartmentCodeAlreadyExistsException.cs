namespace SaaS.DataAccess.Exceptions.Application.Department
{
    public class DepartmentCodeAlreadyExistsException : Exception
    {
        public DepartmentCodeAlreadyExistsException()
        {

        }

        public DepartmentCodeAlreadyExistsException(string message) : base(message)
        {

        }

        public DepartmentCodeAlreadyExistsException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
