namespace Meetup.Core.Application.Data;

public static class ValidationConstants
{
    public static readonly string NoSemicolonRegex = "^[^;]+$";
    public static readonly string NoSemicolonMsg = "Symbol ; is not allowed.";

    public static readonly DateTime MinDateTime = new(1900, 1, 1);
    public static readonly DateTime MaxDateTime = new(2300, 1, 1);
}