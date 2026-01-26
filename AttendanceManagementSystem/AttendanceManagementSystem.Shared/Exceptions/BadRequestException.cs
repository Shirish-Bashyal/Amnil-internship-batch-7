namespace AttendanceManagementSystem.Shared.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(
        string message = "The request was not valid. Please verify and try again."
    )
        : base(message) { }
}
