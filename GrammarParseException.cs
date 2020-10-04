using System;

namespace Chinchillada.Grammar
{
    public class GrammarParseException : Exception
    {
        public GrammarParseException(string message) : base(message)
        {
        }
    }
}