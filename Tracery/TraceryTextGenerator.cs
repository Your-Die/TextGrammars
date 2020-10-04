using System;
using Chinchillada.Generation;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityTracery;

namespace Chinchillada.Grammar
{
    [Serializable]
    public class TraceryTextGenerator : GeneratorBase<string>
    {
        [SerializeField, InlineEditor(InlineEditorObjectFieldModes.Foldout)]
        private IGrammarDefinition grammarDefinition;

        private TraceryGrammar grammar;

        public bool IsDefinitionDirty { get; set; } = true;
        
        public IGrammarDefinition GrammarDefinition
        {
            get => this.grammarDefinition;
            set
            {
                this.grammarDefinition = value;
                this.IsDefinitionDirty = true;
            }
        }

        public string Generate(IGrammarDefinition definition)
        {
            this.GrammarDefinition = definition;
            return this.Generate();
        }

        public string Generate(string origin)
        {
            string GenerationFunction() => this.grammar.Parse(origin);
            return this.Generate(GenerationFunction);
        }

        protected override string GenerateInternal() => this.grammar.Generate();

        protected override void OnBeforeGenerate()
        {
            if (this.IsDefinitionDirty) 
                this.BuildGrammar();
        }

        private void BuildGrammar()
        {
            this.grammar = TraceryFactory.Construct(this.grammarDefinition);
            this.IsDefinitionDirty = false;
        }
    }
}