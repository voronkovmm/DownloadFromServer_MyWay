using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools.DataLoaders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Scripts.AssetBundles
{
    public interface IAssetBundleService
    {
        UniTask LoadAsync<T>(string key, CancellationToken token) where T : Object;
        void Put<T>(T asset, string key) where T : Object;
        bool TryGet<T>(string key, out T asset) where T : Object;
        void Unload(string key);
    }
    
    public class AssetBundleService : IAssetBundleService
    {
        private readonly IAssetBundleLoader assetBundleLoader;
        private readonly AssetBundleManger manager = new();

        public AssetBundleService(IAssetBundleLoader assetBundleLoader)
        {
            this.assetBundleLoader = assetBundleLoader;
        }

        public async UniTask LoadAsync<T>(string key, CancellationToken token) where T : Object
        {
            var asset = await assetBundleLoader.LoadBundleAsync<T>(key, token);
            manager.PutAsset(asset, key);
        }

        public void Put<T>(T asset, string key) where T : Object
        {
            manager.PutAsset(asset, key);
        }

        public bool TryGet<T>(string key, out T asset) where T : Object
        {
            return manager.TryGetAsset(key, out asset);
        }

        public void Unload(string key)
        {
            manager.RemoveAsset(key);
            assetBundleLoader.UnloadBundle(key, true);
        }
    }
}