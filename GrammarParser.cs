namespace Chinchillada.Grammar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using UnityEngine;

    public static class GrammarParser
    {
        public static Grammar Construct(IGrammarDefinition grammarDefinition)
        {
            var tokens = BuildTokenDictionary(grammarDefinition);

            var ruleDefinitions = grammarDefinition.GetAllRules();
            var rules = ruleDefinitions.Select(rule => ConstructRule(rule, tokens));

            return new Grammar(rules);
        }

        public static GrammarRule ConstructRule(GrammarRuleDefinition definition,
            Dictionary<string, NonTerminal> tokens)
        {
            var symbolToken = tokens[definition.Symbol];

            foreach (var replacement in definition.Replacements)
            {
                try
                {
                    var sentence = ParseReplacement(replacement, tokens).ToArray();
                    symbolToken.AddReplacement(sentence);
                }
                catch (GrammarParseException exception)
                {
                    Debug.LogError($"Broken replacement {replacement} for {symbolToken}: " +
                                   $"{Environment.NewLine}{exception.Message}");
                }
            }

            return new GrammarRule(symbolToken);
        }

        private static Dictionary<string, NonTerminal> BuildTokenDictionary(IGrammarDefinition definition)
        {
            var dictionary = new Dictionary<string, NonTerminal>();

            foreach (var symbol in definition.GetSymbols())
            {
                var token = new NonTerminal(symbol);
                dictionary[symbol] = token;
            }

            return dictionary;
        }

        private static IEnumerable<IToken> ParseReplacement(string replacement, Dictionary<string, NonTerminal> tokens)
        {
            Terminal terminal;
            var textBuilder = new StringBuilder();
            var parseType = ParseType.Normal;

            replacement = replacement.Trim();

            foreach (var character in replacement)
            {
                switch (parseType)
                {
                    case ParseType.Normal when character == GrammarConstants.SymbolGuard:

                        if (TryParseTerminal(textBuilder, out terminal))
                            yield return terminal;

                        parseType = ParseType.Symbol;
                        break;

                    case ParseType.Normal:
                    case ParseType.Symbol when character != GrammarConstants.SymbolGuard:
                        textBuilder.Append(character);
                        break;

                    case ParseType.Symbol:
                    {
                        var tokenString = textBuilder.ToString();

                        yield return ParseSymbol(tokens, tokenString);

                        textBuilder.Clear();
                        parseType = ParseType.Normal;
                        break;
                    }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (parseType == ParseType.Symbol)
                Debug.LogError($"No closing guard ({GrammarConstants.SymbolGuard} found: {textBuilder})");

            if (TryParseTerminal(textBuilder, out terminal))
                yield return terminal;
        }

        private static IToken ParseSymbol(Dictionary<string, NonTerminal> tokens, string tokenString)
        {
            if (tokens.TryGetValue(tokenString, out var token))
                return token;

            Debug.LogError($"No replacement found for symbol: {tokenString}");
            return new Terminal(tokenString);
        }

        private static bool TryParseTerminal(StringBuilder textBuilder, out Terminal terminal)
        {
            var text = textBuilder.Extract();

            if (string.IsNullOrEmpty(text))
            {
                terminal = null;
                return false;
            }

            terminal = new Terminal(text);
            return true;
        }

        private static string Extract(this StringBuilder stringBuilder)
        {
            var text = stringBuilder.ToString();
            stringBuilder.Clear();

            return text;
        }

        private enum ParseType
        {
            Normal,
            Symbol,
        }
    }
}