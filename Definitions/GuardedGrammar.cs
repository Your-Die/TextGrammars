using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Chinchillada;
using Chinchillada.Grammar;

namespace Chinchillada.Generation.Grammar
{
    public static class GuardedGrammar
    {
        public static IEnumerable<string> ParseSymbols(string text)
        {
            var openSymbol = false;
            var symbolBuilder = new StringBuilder();

            foreach (var character in text)
            {
                if (openSymbol)
                {
                    if (character == GrammarConstants.SymbolGuard)
                    {
                        yield return symbolBuilder.ToString();

                        symbolBuilder.Clear();
                        openSymbol = false;
                    }
                    else
                    {
                        symbolBuilder.Append(character);
                    }
                }
                else
                {
                    if (character == GrammarConstants.SymbolGuard)
                        openSymbol = true;
                }
            }
        }
             
        public static void ReplaceSymbol(IGrammarDefinition grammar, string oldSymbol, string newSymbol)
        {
            if (grammar.Rules.TryFind(matchingRule => string.Equals(oldSymbol, matchingRule.Symbol), out var rule))
                ReplaceSymbol(grammar, rule, newSymbol);
        }

        public static void ReplaceSymbol(IGrammarDefinition grammar, GrammarRuleDefinition rule, string symbol)
        {
            var oldSymbol = rule.Symbol;
            rule.Symbol = symbol;

            var regex = new Regex(GrammarConstants.SymbolRegex);
            var evaluator = new MatchEvaluator(ReplaceOldSymbol);

            foreach (var ruleDefinition in grammar.Rules)
            {
                for (var index = 0; index < ruleDefinition.Replacements.Count; index++)
                {
                    var replacement = ruleDefinition.Replacements[index];
                    var newReplacement = regex.Replace(replacement, evaluator);

                    if (string.Equals(replacement, newReplacement))
                        continue;

                    rule.Replacements[index] = newReplacement;
                }
            }

            var newOrigin = regex.Replace(grammar.Origin, evaluator);
            if (!string.Equals(grammar.Origin, newOrigin))
                grammar.Origin = newOrigin;

            string ReplaceOldSymbol(Match match)
            {
                var group = match.Groups[1];
                var capturedSymbol = group.Value;

                var newSymbol = string.Equals(capturedSymbol, oldSymbol) ? symbol : capturedSymbol;
                return $"{GrammarConstants.SymbolGuard}{newSymbol}{GrammarConstants.SymbolGuard}";
            }
        }
    }
}