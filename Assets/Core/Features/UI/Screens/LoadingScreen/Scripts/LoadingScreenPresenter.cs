using System;
using Core.Scripts.MainScreen;
using Core.Scripts.Managers;
using VContainer;
using VContainer.Unity;

namespace Features.UI.Screens.LoadingScreen
{
    public class LoadingScreenPresenter : IDisposable, IInitializable
    {
        private LoadingScreen view;
        private LoadingScreenInitializer loadingScreenInitializer;
        private MainScreenCanvasManager canvasManager;

        [Inject]
        public LoadingScreenPresenter(LoadingScreen view, LoadingScreenInitializer loadingScreenInitializer, MainScreenCanvasManager canvasManager)
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