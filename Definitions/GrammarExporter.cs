using Chinchillada.Foundation;

namespace Chinchillada.Thesis.PCG.Grammars.Definitions
{
    using Grammar;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class GrammarExporter : ChinchilladaBehaviour
    {
        [SerializeField] private GrammarDefinition grammarDefinition;

        [Button]
        public JSONObject Export()
        {
            var json = GrammarJSON.GrammarToJSON(this.grammarDefinition);
            Debug.Log(json);
            return json;
        }
    }
}