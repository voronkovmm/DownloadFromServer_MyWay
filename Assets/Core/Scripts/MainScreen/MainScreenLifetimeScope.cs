using Core.Features.UI.Screens.MainScreen;
using Core.Scripts.AssetBundles;
using Core.Scripts.Configs;
using Core.Scripts.MainScreen.Models;
using Core.Scripts.Managers;
using Core.Scripts.SaveLoad;
using Core.Scripts.Tools.DataLoaders;
using Core.Scripts.Tools.Parsers;
using Features.UI.Screens.LoadingScreen;
using Tools.DataLoaders;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Scripts.MainScreen
{
    public class MainScreenLifetimeScope : LifetimeScope
    {
        [SerializeField] private LoadingScreen loadingScreen;
        
        protected override void Configure(IContainerBuilder builder)
        {
            EntryPoint(builder);
            Tools(builder);
            AssetBundles(builder);
            Managers(builder);
            MainScreen(builder);
            LoadingScreen(builder);
            SaveLoad(builder);
        }

        private void EntryPoint(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<EntryPoint>();
        }

        private void Tools(IContainerBuilder builder)
        {
            builder.Register<JsonParser>(Lifetime.Singleton);
            builder.Register<ExternalLoader>(Lifetime.Singleton)
                .As<IDataLoader>();
        }

        private void AssetBundles(IContainerBuilder builder)
        {
            builder.Register<AssetBundleService>(Lifetime.Singleton)
                .AsImplementedInterfaces();
            builder.Register<AssetBundleLoader>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }

        private void Managers(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<CanvasManager>();

            var gameManagerGameObject = new GameObject("GameEvents");
            var gameManager = gameManagerGameObject.AddComponent<GameEvents>();
            DontDestroyOnLoad(gameManagerGameObject);
            builder.RegisterInstance(gameManager)
                .As<IGameEvents>();
        }

        private void MainScreen(IContainerBuilder builder)
        {
            builder.Register<MainScreenAssetService>(Lifetime.Scoped)
                .AsImplementedInterfaces();

            builder.Register<MainScreenModel>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();
            builder.RegisterComponentInHierarchy<Features.UI.Screens.MainScreen.MainScreen>()
                .As<IMainScreen>();
            builder.Register<MainScreenPresenter>(Lifetime.Scoped)
                .AsImplementedInterfaces();
        }

        private void LoadingScreen(IContainerBuilder builder)
        {
            builder.Register<LoadingScreenInitializer>(Lifetime.Scoped);
            builder.Register<LoadingScreenAdapter>(Lifetime.Scoped)
                .WithParameter(loadingScreen)
                .AsImplementedInterfaces();
        }

        private void SaveLoad(IContainerBuilder builder)
        {
            builder.Register<SaveLoadService>(Lifetime.Singleton)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.Register<JsonSaveLoad>(Lifetime.Singleton)
                .AsImplementedInterfaces();
        }
    }
}