using Core.Features.UI.Screens.MainScreen;
using Core.Scripts.Managers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.Scripts.MainScreen
{
    public class EntryPoint : IInitializable
    {
        [Inject] private LoadingScreenInitializer loadingScreenInitializer;
        [Inject] private IMainScreen mainScreen;
        [Inject] private CanvasManager canvasManager;

        public void Initialize()
        {
            LoadConfigs().Forget();
        }

        private async UniTaskVoid LoadConfigs()
        {
            canvasManager.SetActiveMainScreenCanvas(false);
            await loadingScreenInitializer.LoadContentAsync();
            ShowMainScreen();
        }

        private void ShowMainScreen()
        {
            mainScreen.SetVisible(true);
            canvasManager.SetActiveMainScreenCanvas(true);
        }
    }
}
