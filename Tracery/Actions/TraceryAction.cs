using System;
using UnityEngine;

namespace Chinchillada.Grammar
{
    [Serializable]
    public class TraceryAction
    {
        [SerializeField] private string key;

        [SerializeField] private string action;

        public TraceryAction()
        {
        }

        public TraceryAction(string key, string action)
        {
            this.key = key;
            this.action = action;
        }

        public override string ToString() => $"[{this.key}:{this.action}]";
    }
}