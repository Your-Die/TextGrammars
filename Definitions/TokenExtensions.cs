namespace Chinchillada.Grammar
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TokenExtensions
    {
        public static string Stringify(this IEnumerable<IToken> tokens)
        {
            var tokenStrings = tokens.Select(token => token.ToString());
            return string.Join(string.Empty, tokenStrings);
        }
    }
}