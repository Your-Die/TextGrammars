namespace Chinchillada.Grammar
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A replacement rule, made of the <see cref="Symbol"/> and the <see cref="Replacements"/>.
    /// </summary>
    public class GrammarRule
    {
        /// <summary>
        /// The symbol that this rule defines replacements for.
        /// </summary>
        public NonTerminal Symbol { get; }

        /// <summary>
        /// The replacements.
        /// </summary>
        public List<Replacement> Replacements => this.Symbol.Replacements;
        
        /// <summary>
        /// Construct a new <see cref="GrammarRule"/>.
        /// </summary>
        /// <param name="symbolToken">The <see cref="Symbol"/>.</param>
        public GrammarRule(NonTerminal symbolToken)
        {
            this.Symbol = symbolToken;
        }

        public override string ToString()
        {
            var replacementStrings = this.Replacements.Select(replacement => replacement.ToString());
            var replacements = string.Join(" | ", replacementStrings);

            return $"{this.Symbol} => {replacements}";

        }
    }
}