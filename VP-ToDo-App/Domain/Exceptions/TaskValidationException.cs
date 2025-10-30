namespace VP_ToDo_App.Domain.Exceptions;

public class TaskValidationException : Exception
{
    public TaskValidationException(string message) 
        : base(message)
    {
    }
}
