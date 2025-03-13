using System;
using System.Collections.Generic;

public class CustomValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public CustomValidationException(Dictionary<string, string[]> errors)
        : base("Validation failed")
    {
        Errors = errors;
    }
}
