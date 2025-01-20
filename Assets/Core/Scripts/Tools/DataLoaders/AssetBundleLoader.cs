using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Core.Scripts.AssetBundles;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools.DataLoaders
{
    public interface IAssetBundleLoader
    {
        UniTask<T> LoadBundleAsync<T>(string bundleKey, CancellationToken token) where T : Object;
        void UnloadBundle(string bundleName, bool unloadAllLoadedObjects = false);
        void UnloadAllBundles(bool unloadAllLoadedObjects = false);
    }
    
    public class AssetBundleLoader : IDisposable, IAssetBundleLoader
    {
        private const string bundleFilePath = "Bundles"; 
        private const string sourcePath = "Assets/Core/Bundles"; 
        
        private readonly Dictionary<string, AssetBundle> loadedBundles = new();
        
        public void Dispose()
        {
            UnloadAllBundles(true);
        }
        
        public async UniTask<T> LoadBundleAsync<T>(string bundleKey, CancellationToken token) where T : Object
        {
            var path = Path.Combine(Application.streamingAssetsPath, bundleFilePath, bundleKey);
            
            var assetBundle = await AssetBundle.LoadFromFileAsync(path).WithCancellation(token);
            if (assetBundle == null)
            {
                Debug.Log($"Failed to load AssetBundle {bundleFilePath}!");
                return default;
            }

            string assetPath = Path.Combine(sourcePath, $"{bundleKey}.asset");
            var asset = assetBundle.LoadAsset<T>(assetPath);
            if (asset == null)
            {
                assetBundle.Unload(true);
                Debug.LogError($"Failed to load asset {bundleKey}={typeof(T).Name} from AssetBundle={bundleFilePath}!");
                return default;
            }

            loadedBundles[bundleKey] = assetBundle;
            return asset;
        }
        
        public void UnloadBundle(string bundleName, bool unloadAllLoadedObjects = false)
        {
            if (loadedBundles.TryGetValue(bundleName, out var bundle))
            {
                loadedBundles.Remove(bundleName);
                bundle.Unload(unloadAllLoadedObjects);
            }
        }
        
        public void UnloadAllBundles(bool unloadAllLoadedObjects = false)
        {
            foreach (var bundle in loadedBundles.Values)
            {
                bundle.Unload(unloadAllLoadedObjects);
            }

            loadedBundles.Clear();
        }
    }
}