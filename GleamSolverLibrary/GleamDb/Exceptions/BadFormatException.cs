using System;

namespace GleamSolverLibrary.GleamDb.Exceptions
{
    public class BadFormatException : Exception
    {
        public BadFormatException(string msg) : base(msg)
        {
        }
    }
}