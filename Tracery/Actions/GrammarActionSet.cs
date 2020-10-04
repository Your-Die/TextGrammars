using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Chinchillada.Grammar

{
    [CreateAssetMenu(menuName = "Chinchillada/Grammar/Tracery Action Set")]
    public class GrammarActionSet : SerializedScriptableObject
    {
        [SerializeField] private List<TraceryAction> actions;

        public List<TraceryAction> Actions
        {
            get => this.actions;
            set => this.actions = value;
        }

        public override string ToString() => this.actions.ToJSON();
    }
}