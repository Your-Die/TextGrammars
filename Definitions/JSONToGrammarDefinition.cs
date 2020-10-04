using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Chinchillada.Foundation;

namespace Chinchillada.Grammar
{
    public class JSONToGrammarDefinition : ChinchilladaBehaviour
    {
        [SerializeField] private GrammarDefinition output;

        public void Parse(string text)
        {
            var json = new JSONObject(text);

            this.ParseFromJSON(json);
        }

        public void ParseFromJSON(JSONObject json)
        {
            this.output.Origin = json[GrammarConstants.OriginSymbol].list.First().str;
            json.RemoveField(GrammarConstants.OriginSymbol);

            this.output.Rules = ConvertJSONToRules(json).ToList();
        }

        private static IEnumerable<GrammarRuleDefinition> ConvertJSONToRules(JSONObject json)
        {
            foreach (var symbol in json.keys)
            {
                var ruleJSON = json[symbol];
                yield return ConvertJSONToRule(symbol, ruleJSON);
            }
        }

        private static GrammarRuleDefinition ConvertJSONToRule(string symbol, JSONObject json)
        {
            var replacements = ConvertJSONToReplacements(json);
            return new GrammarRuleDefinition(symbol, replacements);
        }

        private static IEnumerable<string> ConvertJSONToReplacements(JSONObject json)
        {
            if (!json.IsArray)
                throw new ArgumentException(json.ToString());

            return json.list.Select(replacement => replacement.str);
        }
    }
}