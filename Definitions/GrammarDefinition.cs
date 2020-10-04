using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using Chinchillada.Generation.Grammar;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;

namespace Chinchillada.Grammar
{
    [CreateAssetMenu(menuName = "Chinchillada/Grammar/Definition")]
    public class GrammarDefinition : SerializedScriptableObject, IGrammarDefinition
    {
        [SerializeField, MultiLineProperty(10), FoldoutGroup("Origin")]
        private string origin;

        [OdinSerialize] private List<GrammarRuleDefinition> rules = new List<GrammarRuleDefinition>();

        public string Name => this.name;

        public string Origin
        {
            get => this.origin;
            set => this.origin = value;
        }

        public List<GrammarRuleDefinition> Rules
        {
            get => this.rules;
            set => this.rules = value;
        }

        [Button]
        private void AddMissingSymbols()
        {
            var allRules = this.GetAllRules().ToList();
            var symbolSet = allRules.Select(rule => rule.Symbol).ToHashSet();

            var symbols = allRules.SelectMany(rule => rule.Replacements.SelectMany(GuardedGrammar.ParseSymbols));

            foreach (var symbol in symbols)
            {
                if (symbolSet.Contains(symbol))
                    continue;

                this.Rules.Add(new GrammarRuleDefinition(symbol));
                symbolSet.Add(symbol);
            }
        }

        [Button, FoldoutGroup("Actions")]
        private void CreateDebugOrigin()
        {
            var symbols = this.rules.Select(rule => WrapGuards(rule.Symbol));
            this.origin = string.Join(Environment.NewLine, symbols);

            string WrapGuards(string text) => $"{GrammarConstants.SymbolGuard}{text}{GrammarConstants.SymbolGuard}";
        }

        [Button, FoldoutGroup("Actions")]
        public void ReplaceSymbolInRules(string oldSymbol, string newSymbol)
        {
            GuardedGrammar.ReplaceSymbol(this, oldSymbol, newSymbol);
        }
    }
}