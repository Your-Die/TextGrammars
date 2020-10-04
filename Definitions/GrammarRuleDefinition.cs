using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Chinchillada
{
    [Serializable]
    public class GrammarRuleDefinition : IGrammarRuleDefinition
    {
        [SerializeField] private string symbol;

        [OdinSerialize, Multiline] private IList<string> replacements = new List<string>();

        public string Symbol
        {
            get => this.symbol;
            set => this.symbol = value;
        }

        public IList<string> Replacements
        {
            get => this.replacements;
            set => this.replacements = value;
        }

        public GrammarRuleDefinition()
        {
        }

        public GrammarRuleDefinition(string symbol, params string[] replacements)
            : this(symbol, replacements.AsEnumerable())
        {
        }

        public GrammarRuleDefinition(string symbol, IEnumerable<string> replacements)
        {
            this.symbol = symbol;
            this.replacements = replacements.ToList();
        }

        public virtual bool Match(string text, int startIndex)
        {
            for (var i = 0; i < this.symbol.Length; i++)
            {
                var index = startIndex + i;
                
                if (text[index] != this.symbol[i])
                    return false;
            }

            return true;
        }

        [Button, FoldoutGroup("Actions")]
        private void SplitReplacementsOnLine()
        {
            var newLine = Environment.NewLine.ToCharArray();
            this.replacements = this.replacements.SelectMany(Split).ToList();

            IEnumerable<string> Split(string replacement) => replacement.Split(newLine);
        }

        [Button, FoldoutGroup("Actions")]
        private void AddRange(float start, float end, float stepSize, string format)
        {
            for (var value = start; value <= end; value += stepSize)
            {
                var stringValue = value.ToString(format);
                this.replacements.Add(stringValue);
            }
        }

        public override string ToString() => this.symbol;

        public GrammarRuleDefinition DeepCopy()
        {
            var replacementsCopy = new List<string>(this.replacements);
            return new GrammarRuleDefinition(this.symbol, replacementsCopy);
        }
    }
}