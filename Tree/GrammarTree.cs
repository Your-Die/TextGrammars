using System.Collections.Generic;
using System.Linq;
using Chinchillada.Foundation;

namespace Chinchillada.Grammar
{
    using System;

    public partial class GrammarTree : INodeParent
    {
        public Grammar Grammar { get; }
        public GrammarTreeLayer RootLayer { get; }

        GrammarTreeLayer INodeParent.ChildLayer => this.RootLayer;

        public event Action<ClosedNode> NodeClosed;
        public event Action<ClosedNode> NodeOpening;

        public GrammarTree(Grammar grammar)
        {
            this.Grammar = grammar;
            this.RootLayer = this.BuildLayer(grammar.OriginRule, 0, this);
        }

        public void SetReplacement(int nodeIndex, int replacementIndex)
        {
            var node = this.RootLayer[nodeIndex];

            if (!(node is RuleNode ruleNode))
                throw new ArgumentException();

            ruleNode.SetReplacement(replacementIndex);
        }

        public string Stringify(Func<IGrammarNode, string> stringifyFunction)
        {
            var nodeStrings = this.RootLayer.Nodes.Select(stringifyFunction);
            return string.Join(string.Empty, nodeStrings);
        }

        public void OnNodeClosed(ClosedNode node) => this.NodeClosed?.Invoke(node);

        public void OnNodeOpening(ClosedNode node) => this.NodeOpening?.Invoke(node);

        private GrammarTreeLayer BuildLayer(GrammarRule rule, int replacementIndex, INodeParent parent)
        {
            var replacement = rule.Replacements[replacementIndex];
            return new GrammarTreeLayer(replacement, parent, this);
        }
    }

    public interface INodeParent
    {
        GrammarTreeLayer ChildLayer { get; }
    }

    public static class NodeParentExtensions
    {
        public static IEnumerable<IGrammarNode> GetChildren(this INodeParent nodeParent)
        {
            return nodeParent.ChildLayer.SelectMany(Enumerate);

            IEnumerable<IGrammarNode> Enumerate(IGrammarNode node)
            {
                return node is INodeParent parent 
                    ? parent.GetChildren() 
                    : Enumerables.Single(node);
            }
        }
    }

}