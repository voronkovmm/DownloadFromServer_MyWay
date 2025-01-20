using System;
using System.Threading;
using Core.Scripts.AssetBundles;
using Core.Scripts.MainScreen.Models;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Core.Features.UI.Screens.MainScreen
{
    public class MainScreenPresenter : IDisposable, IInitializable
    {
        private MainScreen view;
        private MainScreenModel model;
        private CancellationTokenSource tokenSource = new();

        public MainScreenPresenter(
            MainScreen view, 
            MainScreenModel modelModel)
        {
            this.view = view;
            this.model = modelModel;
        }

        public void Initialize()
        {
            view.OnVisibleChanged += View_VisibleChanged;
        }

        public void Dispose()
        {
            tokenSource.Cancel();
            view.OnVisibleChanged -= View_VisibleChanged;
            Unsubscribe();
        }
        
        private void Subscribe()
        {
            view.OnClickUpdateContent += View_UpdateContent;
            view.OnClickIncreaseCounter += View_IncreaseCounter;
            model.OnCounterChanged += Model_CounterChanged;
            model.OnContentChanged += Model_ContentChanged;
        }

        private void Unsubscribe()
        {
            view.OnClickUpdateContent -= View_UpdateContent;
            view.OnClickIncreaseCounter -= View_IncreaseCounter;
            model.OnCounterChanged -= Model_CounterChanged;
            model.OnContentChanged -= Model_ContentChanged;
        }

        private void RefreshView()
        {
            if (model.NeedUpdateContent)
            {
                model.UpdateContentAsync(tokenSource.Token).Forget();
                return;
            }
            
            view.CounterLabel = model.Counter.ToString();
            view.WelcomeLabel = model.WelcomeLabel;
            view.IncreaseCounterSprite = model.BundleAsset.IncreaseCounterButton;
        }

        #region ViewCallbacks

        private void View_VisibleChanged(bool visibleValue)
        {
            if (visibleValue)
            {
                Subscribe();
                RefreshView();
            }
            else
            {
                Unsubscribe();
            }
        }

        private void View_IncreaseCounter()
        {
            model.IncreaseCounter();
        }
        
        private void View_UpdateContent()
        {
            if (model.IsLoadContentProcessing)
                return;
            
            model.UpdateContentAsync(tokenSource.Token).Forget();
        }

        #endregion

        #region ModelCallbacks

        private void Model_CounterChanged(int counter)
        {
            view.CounterLabel = counter.ToString();
        }
        
        private void Model_ContentChanged()
        {
            view.WelcomeLabel = model.WelcomeLabel;
            view.IncreaseCounterSprite = model.BundleAsset.IncreaseCounterButton;
        }

        #endregion
    }
}