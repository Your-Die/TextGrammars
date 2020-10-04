namespace Chinchillada.Grammar
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A replacement, which makes up the right-hand-side of a <see cref="GrammarRule"/>.
    /// Saved as a chain of <see cref="IToken"/>, with the <see cref="GrammarStatement"/> tokens extracted.
    /// </summary>
    public class Replacement : IEnumerable<IToken>
    {
        /// <summary>
        /// The tokens that make up this replacement.
        /// </summary>
        public List<IToken> Tokens { get; }

        /// <summary>
        /// Construct a new <see cref="Replacement"/>.
        /// </summary>
        /// <param name="tokens">The tokens that make up the <see cref="Replacement"/>.</param>
        public Replacement(IEnumerable<IToken> tokens)
        {
            this.Tokens = tokens.ToList();
        }

        public string GetTokenText()
        {
            var tokenStrings = this.Tokens.Select(token => token.Key);
            return string.Join(string.Empty, tokenStrings);
        }

        public override string ToString()
        {
            var tokenStrings = this.Tokens.Select(Stringify);
            return string.Join(string.Empty, tokenStrings);

            string Stringify(object value) => value.ToString();
        }

        #region IEnumerable

        IEnumerator<IToken> IEnumerable<IToken>.GetEnumerator() => this.Tokens.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) this.Tokens).GetEnumerator();

        #endregion
    }
}