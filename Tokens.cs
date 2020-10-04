namespace Chinchillada.Grammar
{
    using System.Collections.Generic;

    public interface IToken
    {
        string Key { get; }
    }


    public class NonTerminal : IToken
    {
        public string Key { get; }

        public List<Replacement> Replacements { get; } = new List<Replacement>();

        public NonTerminal(string text)
        {
            this.Key = text;
        }

        public NonTerminal AddReplacement(params IToken[] replacementTokens)
        {
            var replacement = new Replacement(replacementTokens);
            this.Replacements.Add(replacement);
            
            return this;
        }

        public NonTerminal AddReplacement(string replacementString)
        {
            var token = new Terminal(replacementString);
            return this.AddReplacement(token);
        }

        public override string ToString()
        {
            return $"{GrammarConstants.SymbolGuard}{this.Key}{GrammarConstants.SymbolGuard}";
        }
    }

    public class Terminal : IToken
    {
        public string Key { get; set; }

        public Terminal(string text)
        {
            this.Key = text;
        }

        public override string ToString() => this.Key;
    }
}