using Chinchillada.Foundation;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

namespace Chinchillada.Grammar
{
    public class TraceryPreview : ChinchilladaBehaviour
    {
        [SerializeField] private TMP_Text textElement;
        
        [OdinSerialize] private TraceryTextGenerator generator;

        [SerializeField] private IEvent<IGrammarDefinition> generateEvent;
        
        public void GeneratePreview(IGrammarDefinition definition)
        {
            this.generator.GrammarDefinition = definition;
            this.GeneratePreview();
        }

        public void GeneratePreview() => this.textElement.text = this.generator.Generate();

        private void OnEnable() => this.generateEvent.Subscribe(this.GeneratePreview);

        private void OnDisable() => this.generateEvent.Unsubscribe(this.GeneratePreview);
    }
}