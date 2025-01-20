using UnityEngine;

namespace Core.Scripts.Tools.Parsers
{
    public class JsonParser
    {
        public bool TryParse<T>(string json, out T obj)
        {
            obj = JsonUtility.FromJson<T>(json);
            return obj != null;
        }
    }
}