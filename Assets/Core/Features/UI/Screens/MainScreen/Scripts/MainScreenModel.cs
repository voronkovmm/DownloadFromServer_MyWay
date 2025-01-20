using System;
using System.Threading;
using Core.Scripts.API;
using Core.Scripts.AssetBundles;
using Core.Scripts.Configs;
using Core.Scripts.SaveLoad;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using VContainer;
using VContainer.Unity;

namespace Core.Scripts.MainScreen.Models
{
    public class MainScreenModel : ISaveble, IPostInitializable
    {
        [Inject] private SaveLoadService saveLoadService;
        [Inject] private IMainScreenAssetService mainScreenAssetService;
         
        public event Action<int> OnCounterChanged;
        public event Action OnContentChanged;

        public bool IsLoadContentProcessing { get; private set; }
        public bool NeedUpdateContent { get; private set; }
        
        [JsonProperty]
        public string WelcomeLabel { get; private set; }
        [JsonProperty]
        private int counter;
        private MainScreenBundleAsset bundleAsset;
        
        public int Counter
        {
            get => counter;
            private set
            {
                counter = value;
                OnCounterChanged?.Invoke(counter);
            }
        }

        public MainScreenModel() { }

        [JsonIgnore]
        public MainScreenBundleAsset BundleAsset => mainScreenAssetService.GetBundleAsset();
        
        [JsonIgnore]
        public string SaveKey { get; } = SaveAPI.MainScreenCounter;

        public void PostInitialize()
        {
            if (saveLoadService.TryGetSave(this, out MainScreenModel save))
            {
                counter = save.counter;
                WelcomeLabel = save.WelcomeLabel;
                NeedUpdateContent = false;
            }
            else
            {
                NeedUpdateContent = true;
            }   
        }

        public async UniTask LoadContentAsync(CancellationToken token)
        {
            IsLoadContentProcessing = true;
            
            await mainScreenAssetService.Load(token);

            if (mainScreenAssetService.ContentLoaded && NeedUpdateContent)
            {
                Counter = mainScreenAssetService.SettingsAsset.StartingNumber;
                WelcomeLabel = mainScreenAssetService.WelcomeLabelAsset.WelcomeLabel;
                NeedUpdateContent = false;
                OnContentChanged?.Invoke();
            }
            
            IsLoadContentProcessing = false;
        }

        public async UniTask UpdateContentAsync(CancellationToken token)
        {
            NeedUpdateContent = true;
            await LoadContentAsync(token);
        }

        public void IncreaseCounter()
        {
            Counter += 1;
        }
    }
}