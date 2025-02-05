using System;
using System.Threading;
using Core.Features.UI.Screens.MainScreen;
using Core.Scripts.Managers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.Scripts.MainScreen
{
    public class EntryPoint : IInitializable, IDisposable
    {
        [Inject] private LoadingScreenInitializer loadingScreenInitializer;
        [Inject] private IMainScreen mainScreen;
        [Inject] private CanvasManager canvasManager;

        private CancellationTokenSource tokenSource;

        public void Initialize()
        {
            tokenSource = new();
            LoadConfigs(tokenSource.Token).Forget();
        }

        private async UniTaskVoid LoadConfigs(CancellationToken token)
        {
            canvasManager.SetActiveMainScreenCanvas(false);
            await loadingScreenInitializer.LoadContentAsync(token);
            ShowMainScreen();
        }

        private void ShowMainScreen()
        {
            mainScreen.SetVisible(true);
            canvasManager.SetActiveMainScreenCanvas(true);
        }

        public void Dispose()
        {
            if(tokenSource is { IsCancellationRequested: false })
                tokenSource.Cancel();
        }
    }
}
