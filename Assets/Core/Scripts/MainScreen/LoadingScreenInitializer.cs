using System;
using System.Threading;
using Core.Scripts.AssetBundles;
using Core.Scripts.Configs;
using Core.Scripts.MainScreen.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Core.Scripts.MainScreen
{
    public class LoadingScreenInitializer
    {
        public event Action<float> OnProgressChanged;
        public event Action OnLoadingComplete;

        private float progress;
        private CancellationTokenSource tokenSource = new();
        private MainScreenModel mainScreenModel;

        [Inject]
        public LoadingScreenInitializer(MainScreenModel mainScreenModel)
        {
            this.mainScreenModel = mainScreenModel;
        }
        
        public float Progress
        {
            get => progress;
            private set
            {
                progress = value;
                OnProgressChanged?.Invoke(progress);
            }
        }
        
        public async UniTask RunLoadAsync()
        {
            Progress = 0;
            
            // симуляция долгой загрузки
            while (Progress < 0.33)
            {
                await UniTask.Delay(100);
                Progress += 0.05f;
            }

            await mainScreenModel.LoadContentAsync(tokenSource.Token);
            
            // симуляция долгой загрузки
            await UniTask.Delay(500); 
            
            // симуляция долгой загрузки
            while (Progress < 0.66f)
            {
                await UniTask.Delay(50);
                Progress += 0.025f;
            }
            
            // симуляция долгой загрузки
            await UniTask.Delay(400); 
            
            // симуляция долгой загрузки
            while (Progress < 1f)
            {
                await UniTask.Delay(50);
                Progress = Mathf.Min(1, Progress + 0.015f);
            }
            
            // симуляция долгой загрузки
            await UniTask.Delay(300); 

            OnLoadingComplete?.Invoke();
        }
    }
}