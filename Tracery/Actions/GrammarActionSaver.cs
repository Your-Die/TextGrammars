using System.Linq;
using Chinchillada.Foundation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.Grammar

{
    [RequireComponent(typeof(TraceryActionGenerator))]
    public class GrammarActionSaver : ChinchilladaBehaviour
    {
        [SerializeField, FindComponent] private TraceryActionGenerator generator;

        [SerializeField, InlineEditor] private GrammarActionSet actionSet;

        [Button]
        public void Save()
        {
            var actions = this.generator.Generate();
            this.actionSet.Actions = actions.ToList();
        }
    }
}