using System.Collections.Generic;

namespace Chinchillada
{
    public interface IGrammarRuleDefinition
    {
        string Symbol { get; }
        IList<string> Replacements { get; }
    }
}