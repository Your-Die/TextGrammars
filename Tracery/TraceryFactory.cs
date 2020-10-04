namespace Chinchillada.Grammar
{
    using System.Collections.Generic;
    using UnityTracery;

    public static class TraceryFactory
    {
        public static TraceryGrammar Construct(IGrammarDefinition definition, IEnumerable<TraceryAction> actions = null)
        {
            var origin = definition.Origin;

            if (actions != null)
                definition.Origin = actions.ToJSON() + origin;

            var json = GrammarJSON.GrammarToJSON(definition);
            var grammar = new TraceryGrammar(json);

            definition.Origin = origin;
            return grammar;
        }

        public static TraceryGrammar Construct(IGrammarDefinition definition, GrammarActionSet actionSet)
        {
            return Construct(definition, actionSet.Actions);
        }
    }
}