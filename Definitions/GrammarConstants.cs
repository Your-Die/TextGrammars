namespace Chinchillada.Grammar
{
    using Chinchillada;
    using UnityEngine;

    [DefaultExecutionOrder(-1)]
    public class GrammarConstants : SingletonBehaviour<GrammarConstants>
    {
        [SerializeField] private char symbolGuard = '#';

        [SerializeField] private  string originSymbol = "origin";

        [SerializeField] private string symbolRegex;
        
        public static char SymbolGuard => Instance.symbolGuard;
        
        public static string OriginSymbol => Instance.originSymbol;

        public static string SymbolRegex => Instance.symbolRegex;
    }
}