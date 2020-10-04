namespace Chinchillada.Grammar
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Chinchillada.Foundation;

    public class GrammarTreeLayer : IReadOnlyList<IGrammarNode>
    {
        public int Count => this.Nodes.Count;

        public IList<IGrammarNode> Nodes { get; }
        
        public IGrammarNode this[int index]
        {
            get => this.Nodes[index];
            set => this.Nodes[index] = value;
        }

        public GrammarTreeLayer(Replacement replacement, INodeParent parent, GrammarTree tree)
        {
            this.Nodes = replacement.Tokens.SelectWithIndex((token, index) => BuildNode(token, index, parent, tree)).ToList();
        }
        
        private static IGrammarNode BuildNode(IToken token, int index, INodeParent parent, GrammarTree tree)
        {
            switch (token)
            {
                case Terminal terminal:
                    return new TerminalNode(terminal);

                case NonTerminal nonTerminal:
                    var expansionRule = tree.Grammar.GetRule(nonTerminal);
                    return new OpenNode(tree, expansionRule, index, parent);
                
                default:
                    throw new NotImplementedException();
            }
        }
        public IEnumerator<IGrammarNode> GetEnumerator() => this.Nodes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.Nodes.GetEnumerator();
    }
}