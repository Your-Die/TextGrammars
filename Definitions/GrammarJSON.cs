namespace Chinchillada.Grammar
{
    using System.Collections.Generic;
    using System.Linq;

    public static class GrammarJSON
    {
        public static JSONObject GrammarToJSON(IGrammarDefinition grammar)
        {
            var json = GetOriginJSON(grammar);
            var rulesJSON = grammar.Rules.Select(GenerateRuleJSON);

            foreach (var ruleJSON in rulesJSON)
                json.Merge(ruleJSON);

            return json;
        }

        private static JSONObject GetOriginJSON(IGrammarDefinition grammar)
        {
            var rule = new GrammarRuleDefinition(GrammarConstants.OriginSymbol, grammar.Origin);
            return GenerateRuleJSON(rule);
        }
        
        public static JSONObject GenerateRuleJSON(GrammarRuleDefinition rule)
        {
            var replacementsJSON = ConvertReplacementsToJSON(rule.Replacements);

            var json = new JSONObject();
            json.AddField(rule.Symbol, replacementsJSON);

            return json;
        }
        
        private static JSONObject ConvertReplacementsToJSON(IEnumerable<string> replacements)
        {
            var json = new JSONObject(JSONObject.Type.ARRAY);

            foreach (var replacement in replacements) 
                json.Add(replacement);

            return json;
        }
    }
}