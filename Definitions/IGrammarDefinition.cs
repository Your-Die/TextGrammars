using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Chinchillada;
using Sirenix.OdinInspector.Editor.TypeSearch;

namespace Chinchillada.Grammar
{
    public interface IGrammarDefinition
    {
        string Name { get; }
        string Origin { get; set; }
        List<GrammarRuleDefinition> Rules { get; set; }
    }

    
    public static class GrammarDefinitionExtensions
    {
        public static IEnumerable<string> GetSymbols(this IGrammarDefinition grammarDefinition)
        {
            yield return GrammarConstants.OriginSymbol;

            foreach (var rule in grammarDefinition.Rules)
                yield return rule.Symbol;
        }
        
        public static IList<string> FindRule(this IGrammarDefinition definition, string symbol)
        {
            var rules = definition.GetAllRules();
            var rule = rules.First(item => item.Symbol == symbol);

            return rule.Replacements;
        }

        public static GrammarRuleDefinition BuildOriginRule(this IGrammarDefinition grammarDefinition)
        {
            return new GrammarRuleDefinition(GrammarConstants.OriginSymbol, grammarDefinition.Origin);
        }

        public static IEnumerable<GrammarRuleDefinition> GetAllRules(this IGrammarDefinition definition)
        {
            return definition.Rules.Prepend(definition.BuildOriginRule());
        }
    }
}