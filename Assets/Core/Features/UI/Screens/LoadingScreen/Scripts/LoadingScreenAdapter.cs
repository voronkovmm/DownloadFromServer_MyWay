using System;
using Core.Scripts.MainScreen;
using Core.Scripts.Managers;
using VContainer;
using VContainer.Unity;

namespace Features.UI.Screens.LoadingScreen
{
    public class LoadingScreenAdapter : IDisposable, IInitializable
    {
        private LoadingScreen view;
        private LoadingScreenInitializer loadingScreenInitializer;
        private CanvasManager canvasManager;

        [Inject]
        public LoadingScreenAdapter(LoadingScreen view, LoadingScreenInitializer loadingScreenInitializer, CanvasManager canvasManager)
        {
            this.canvasManager = canvasManager;
            this.loadingScreenInitializer = loadingScreenInitializer;
            this.view = view;
        }

        public void Initialize()
        {
            canvasManager.SetActiveLoadingScreenCanvas(true);
            loadingScreenInitializer.OnProgressChanged += LoadingScreenInitializerProgressChanged;
            loadingScreenInitializer.OnLoadingComplete += LoadingScreenInitializerLoadingComplete;
        }

        public void Dispose()
        {
            loadingScreenInitializer.OnProgressChanged -= LoadingScreenInitializerProgressChanged;
            loadingScreenInitializer.OnLoadingComplete -= LoadingScreenInitializerLoadingComplete;
        }

        private void LoadingScreenInitializerProgressChanged(float progress)
        {
            view.Progress = progress;
        }

        private void LoadingScreenInitializerLoadingComplete()
        {
            canvasManager.SetActiveLoadingScreenCanvas(false);
            Dispose();
        }
    }
}