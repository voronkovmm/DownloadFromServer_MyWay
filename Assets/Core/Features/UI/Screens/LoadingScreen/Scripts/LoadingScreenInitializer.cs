using System;
using System.Threading;
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
        
        public async UniTask LoadContentAsync(CancellationToken token)
        {
            Progress = 0;

            try
            {
                await mainScreenModel.LoadContentAsync(token);
             
                // симуляция долгой загрузки
                while (Progress < 1f)
                {
                    token.ThrowIfCancellationRequested();

                    await UniTask.Delay(50, cancellationToken: token);
                    Progress = Mathf.Min(1, Progress + 0.03f);
                }
                
                OnLoadingComplete?.Invoke();
            }
            catch (OperationCanceledException)
            {
                OnLoadingComplete?.Invoke();
            }
        }
    }
}