using System.Collections.Generic;
using UnityEngine;

namespace Core.Scripts.AssetBundles
{
    public class AssetBundleManger
    {
        class AssetHolder<T>
        {
            public T Asset { get; set; }

            public AssetHolder(T asset)
            {
                Asset = asset;
            }
        }
        
        private readonly Dictionary<string, object> activeAssets = new();

        public void PutAsset<T>(T asset, string key)
        {
            activeAssets[key] = new AssetHolder<T>(asset);
        }

        public bool TryGetAsset<T>(string key, out T asset) where T : Object
        {
            if (activeAssets.TryGetValue(key, out var holder) && holder is AssetHolder<T> typedHolder)
            {
                asset = typedHolder.Asset;
                return true;
            }

            asset = null;
            return false;
        }

        public void RemoveAsset(string key)
        {
            activeAssets.Remove(key);
        }
    }
}