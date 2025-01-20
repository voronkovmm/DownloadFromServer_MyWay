using Core.Scripts.Managers;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.Scripts.MainScreen
{
    public class MainScreenEntryPoint : IInitializable
    {
        [Inject] private LoadingScreenInitializer loadingScreenInitializer;
        [Inject] private Features.UI.Screens.MainScreen.MainScreen mainScreen;
        [Inject] private MainScreenCanvasManager canvasManager;

        public void Initialize()
        {
            LoadConfigs().Forget();
        }

        private async UniTaskVoid LoadConfigs()
        {
            canvasManager.SetActiveMainScreenCanvas(false);
            await loadingScreenInitializer.RunLoadAsync();

            ShowMainScreen();
        }

        private void ShowMainScreen()
        {
            mainScreen.SetVisible(true);
            canvasManager.SetActiveMainScreenCanvas(true);
        }
    }
}
