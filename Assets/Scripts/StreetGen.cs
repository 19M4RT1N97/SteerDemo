using System;
using System.Linq;
using System.Text;
using Rules;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class StreetGen : MonoBehaviour
    {
        public Rule[] rules;
        public string rootSentence;
        [Range(0, 10)] public int iterationLimit = 1;

        public string GenerateSentence(string word = null)
        {
            if (string.IsNullOrEmpty(word))
            {
                word = rootSentence;
            }

            return GrowRecursive(word);
        }

        private string GrowRecursive(string word, int itIndex = 0)
        {
            if (iterationLimit <= itIndex)
            {
                return word;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var c in word)
            {
                sb.Append(c);
                foreach (var rule in rules)
                {
                    if (rule.letter == c.ToString())
                        sb.Append(GrowRecursive(rule.GetResult(), itIndex + 1));
                }
            }

            return sb.ToString();
        }
    }
}