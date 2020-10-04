using System.Collections.Generic;
using System.Linq;
using Chinchillada.Generation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.Grammar
{
    using UnityTracery;

    public class TraceryActionGenerator : GeneratorComponentBase<IEnumerable<TraceryAction>>
    {
        [SerializeField, InlineEditor] private GrammarDefinition grammarDefinition;

        [SerializeField] private List<string> keys;

        protected override IEnumerable<TraceryAction> GenerateInternal()
        {
            var json = GrammarJSON.GrammarToJSON(this.grammarDefinition);
            var grammar = new TraceryGrammar(json);

            foreach (var key in this.keys)
            {
                var ruleKey = Wrap(key, GrammarConstants.SymbolGuard);
                var result = grammar.Parse(ruleKey);
                
                var action = new TraceryAction(key, result);
                yield return action;
            }
        }

        [Button]
        private void ReadKeysFromGrammar()
        {
            this.keys = this.grammarDefinition.Rules.Select(rule => rule.Symbol).ToList();
        }
        
        
        public static string Wrap(string input, char wrap) => $"{wrap}{input}{wrap}";
    }
}