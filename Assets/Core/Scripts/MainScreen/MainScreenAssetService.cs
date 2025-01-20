using System.Threading;
using Core.Scripts.API;
using Core.Scripts.AssetBundles;
using Core.Scripts.MainScreen.JsonSchemes;
using Core.Scripts.Tools.DataLoaders;
using Core.Scripts.Tools.Parsers;
using Cysharp.Threading.Tasks;

namespace Core.Scripts.Configs
{
    public interface IMainScreenAssetService
    {
        bool ContentLoaded { get; }
        MainScreenSettingsJsonScheme SettingsAsset { get; }
        MainScreenWelcomeLabelJsonScheme WelcomeLabelAsset { get; }
        MainScreenBundleAsset GetBundleAsset();
        UniTask Load(CancellationToken token);
    }
    
    public class MainScreenAssetService : IMainScreenAssetService
    {
        private bool contentLoaded;
        
        public MainScreenSettingsJsonScheme SettingsAsset { get; private set; }
        public MainScreenWelcomeLabelJsonScheme WelcomeLabelAsset { get; private set; }
        
        private readonly IDataLoader dataLoader;
        private readonly JsonParser jsonParser;
        private readonly IAssetBundleService bundleService;
        
        public MainScreenAssetService(IDataLoader dataLoader, JsonParser jsonParser, IAssetBundleService bundleService)
        {
            this.bundleService = bundleService;
            this.dataLoader = dataLoader;
            this.jsonParser = jsonParser;
        }

        public bool ContentLoaded => contentLoaded;

        public MainScreenBundleAsset GetBundleAsset()
        {
            return bundleService.TryGet<MainScreenBundleAsset>(AssetBundleKeysAPI.main_screen, out var asset) ? asset : null;
        }

        public async UniTask Load(CancellationToken token)
        {
            contentLoaded = false;

            await UniTask.WhenAll(
                LoadSettings(token),
                LoadWelcomeLabel(token),
                LoadAsset(token)
            );
            
            contentLoaded = true;
        }

        private async UniTask LoadSettings(CancellationToken token)
        {
            var settingsFile = await dataLoader.LoadDataAsync(FileLocationAPI.Settings, token);
            if (jsonParser.TryParse<MainScreenSettingsJsonScheme>(settingsFile, out var settingsData))
            {
                SettingsAsset = settingsData;
            }
        }
        
        private async UniTask LoadWelcomeLabel(CancellationToken token)
        {
            var welcomeFile = await dataLoader.LoadDataAsync(FileLocationAPI.Welcome, token);
            if (jsonParser.TryParse<MainScreenWelcomeLabelJsonScheme>(welcomeFile, out var welcomeLabelData))
            {
                WelcomeLabelAsset = welcomeLabelData;
            }
        }

        private async UniTask LoadAsset(CancellationToken token)
        {
            bundleService.Unload(AssetBundleKeysAPI.main_screen);
            await bundleService.LoadAsync<MainScreenBundleAsset>(AssetBundleKeysAPI.main_screen, token);
        }
    }
}