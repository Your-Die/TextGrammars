namespace Chinchillada.Grammar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A grammar, defined as a collection of <see cref="GrammarRule"/>.
    /// </summary>
    public class Grammar
    {
        /// <summary>
        /// The origin rule. The rule the grammar starts with.
        /// </summary>
        public GrammarRule OriginRule { get; }

        /// <summary>
        /// The rules.
        /// </summary>
        public IEnumerable<GrammarRule> Rules { get; }

        /// <summary>
        /// Lookup for the <see cref="GrammarRule"/> by <see cref="GrammarRule.Symbol"/>.
        /// </summary>
        private readonly Dictionary<NonTerminal, GrammarRule> symbolsToRules;

        /// <summary>
        /// Construct a new <see cref="Grammar"/>.
        /// </summary>
        /// <param name="rules">The rules that make up the grammar.</param>
        public Grammar(params GrammarRule[] rules)
            : this(rules.AsEnumerable())
        {
        }

        /// <summary>
        /// Construct a new <see cref="Grammar"/>.
        /// </summary>
        /// <param name="rules">The rules that make up the grammar.</param>
        public Grammar(IEnumerable<GrammarRule> rules)
        {
            this.Rules = rules.ToArray();

            // Extract the origin rule.
            this.OriginRule = this.FindRule(GrammarConstants.OriginSymbol);

            // Build dictionary.
            this.symbolsToRules = this.Rules.ToDictionary(
                rule => rule.Symbol,
                rule => rule);
        }


        public IEnumerable<Replacement> GetAllReplacements() => this.Rules.SelectMany(rule => rule.Replacements);

        /// <inheritdoc />
        public override string ToString()
        {
            var ruleStrings = this.Rules.Select(rule => rule.ToString());
            return string.Join(Environment.NewLine, ruleStrings);
        }

        /// <summary>
        /// Get the <see cref="GrammarRule"/> associated with the <paramref name="symbol"/>.
        /// </summary>
        public GrammarRule GetRule(NonTerminal symbol) => this.symbolsToRules[symbol];

        public void SetTerminalRule(string symbolKey, IEnumerable<string> replacements)
        {
            // Get the relevant symbol.
            var rule = this.FindRule(symbolKey);
            var symbol = rule.Symbol;

            // Clear out previous replacements.
            symbol.Replacements.Clear();
            
            // add the new replacements.
            foreach (var replacement in replacements)
                symbol.AddReplacement(replacement);
        }

        private GrammarRule FindRule(string symbolKey) => this.Rules.First(rule => rule.Symbol.Key == symbolKey);
    }
}