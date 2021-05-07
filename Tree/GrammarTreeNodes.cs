using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada;

namespace Chinchillada.Grammar
{
    public interface IGrammarNode
    {
        string Stringify();
    }

    public class TerminalNode : IGrammarNode
    {
        public Terminal Token { get; }

        public TerminalNode(Terminal token) => this.Token = token;

        public TerminalNode(string text) => this.Token = new Terminal(text);

        public override string ToString() => this.Token.ToString();
        public string Stringify() => this.ToString();
    }

    public abstract class RuleNode : IGrammarNode
    {
        public GrammarRule Rule { get; }
        public GrammarTree Tree { get; }
        public int Index { get; }
        public INodeParent Parent { get; }

        public List<Replacement> Replacements => this.Rule.Replacements;

        protected RuleNode(GrammarRule rule, GrammarTree tree, int index, INodeParent parent)
        {
            this.Rule = rule;
            this.Tree = tree;
            this.Index = index;
            this.Parent = parent;
        }

        public virtual void SetReplacement(int replacementIndex)
        {
            var closedNode = new ClosedNode(this, replacementIndex);
            this.Replace(closedNode);

            this.Tree.OnNodeClosed(closedNode);
        }

        public void Replace(IGrammarNode otherNode)
        {
            this.Parent.ChildLayer[this.Index] = otherNode;
        }

        public abstract string Stringify();
    }

    public class OpenNode : RuleNode
    {
        public NonTerminal Token => this.Rule.Symbol;

        public OpenNode(GrammarTree tree, GrammarRule rule, int index, INodeParent parent)
            : base(rule, tree, index, parent)
        {
        }

        public override string ToString() => this.Rule.Symbol.ToString();
        public override string Stringify() => this.Token.Key;
    }

    public class ClosedNode : RuleNode, INodeParent
    {
        public int ReplacementIndex { get; }
        public GrammarTreeLayer ChildLayer { get; }
        public Replacement Replacement { get; }

        public ClosedNode(RuleNode node, int replacementIndex)
            : base(node.Rule, node.Tree, node.Index, node.Parent)
        {
            this.ReplacementIndex = replacementIndex;
            this.Replacement = this.Rule.Replacements[replacementIndex];

            this.ChildLayer = new GrammarTreeLayer(this.Replacement, this, this.Tree);
        }

        public ClosedNode(ClosedNode node) : base(node.Rule, node.Tree, node.Index, node.Parent)
        {
            this.ReplacementIndex = node.ReplacementIndex;
            this.Replacement = node.Replacement;
            this.ChildLayer = node.ChildLayer;
        }

        public void Open()
        {
            var openNode = new OpenNode(this.Tree, this.Rule, this.Index, this.Parent);
            this.Replace(openNode);

            this.Tree.OnNodeOpening(this);
        }

        public override void SetReplacement(int replacementIndex)
        {
            base.SetReplacement(replacementIndex);
            this.Tree.OnNodeOpening(this);
        }


        public override string ToString()
        {
            return $"#{this.Rule.Symbol}# => {this.Replacement}";
        }

        public override string Stringify() => this.Stringify(node => node.Stringify());

        public virtual string Stringify(Func<IGrammarNode, string> stringifyFunction)
        {
            var nodeStrings = this.ChildLayer.Nodes.Select(stringifyFunction);
            return string.Join(string.Empty, nodeStrings);
        }

       
    }

}