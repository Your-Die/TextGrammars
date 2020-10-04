namespace Chinchillada.Grammar
{
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [CreateAssetMenu(menuName = "Chinchillada/Grammar/Definition Reference")]
    public class GrammarReference : SerializedScriptableObject, IGrammarDefinition
    {
        [SerializeField] private IGrammarDefinition grammarDefinition;

        public string Name => this.grammarDefinition.Name;

        public IGrammarDefinition Definition
        {
            get => this.grammarDefinition;
            set => this.grammarDefinition = value;
        }

        string IGrammarDefinition.Origin
        {
            get => this.Definition.Origin;
            set => this.Definition.Origin = value;
        }

        List<GrammarRuleDefinition> IGrammarDefinition.Rules
        {
            get => this.Definition.Rules;
            set => this.Definition.Rules = value;
        }
    }
}