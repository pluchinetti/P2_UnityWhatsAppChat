using System;

[Serializable]

internal class ExceptionPhone : Exception
{
    public ExceptionPhone()
    {
        
    }
    public ExceptionPhone(string message) : base (message)
    {

    }
}