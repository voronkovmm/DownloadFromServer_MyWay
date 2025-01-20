using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Scripts.Tools.DataLoaders
{
    public class ExternalLoader : IDataLoader
    {
        public async UniTask<string> LoadDataAsync(string path, CancellationToken token)
        {
            path = Path.Combine(Application.streamingAssetsPath, path); 

            if (!File.Exists(path))
            {
                Debug.LogError($"File not found at path: {path}");
                throw new FileNotFoundException($"File not found at path: {path}");
            }

            string content;
            try
            {
                content = await File.ReadAllTextAsync(path, token);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to read file: {ex.Message}");
                throw;
            }

            return content;
        }
    }
}