using System.Linq;
using UnityEngine;

namespace Rules
{
    [CreateAssetMenu(menuName = "Street/Rule")]
    public class Rule : ScriptableObject
    {
        public string letter;
        
        [SerializeField]
        private string[] results = null;

        public string GetResult()
        {
            return results[0];
        }
    }
}