namespace SaaS.DataAccess.Exceptions.Application.WorkHour
{
    public class WorkDayHasAlreadyBeenRegisteredException : Exception
    {
        public WorkDayHasAlreadyBeenRegisteredException()
        {

        }

        public WorkDayHasAlreadyBeenRegisteredException(string message) : base(message)
        {

        }

        public WorkDayHasAlreadyBeenRegisteredException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
