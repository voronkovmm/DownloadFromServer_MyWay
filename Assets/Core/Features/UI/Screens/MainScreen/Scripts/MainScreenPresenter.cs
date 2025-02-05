using System;
using System.Threading;
using Core.Scripts.MainScreen.Models;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Features.UI.Screens.MainScreen
{
    public interface IMainScreenPresenter
    {
        string CounterLabel { get; }
        string WelcomeLabel { get; }
        Sprite IncreaseCounterButtonSprite { get; }
        void SetView(IMainScreen view);
        void IncreaseCounter();
        void UpdateContent();
        void VisibleChanged(bool isVisible);
    }
    
    public class MainScreenPresenter : IMainScreenPresenter, IDisposable
    {
        private IMainScreen view;
        private MainScreenModel model;
        private CancellationTokenSource tokenSource = new();

        public MainScreenPresenter(MainScreenModel modelModel)
        {
            this.model = modelModel;
        }

        public string CounterLabel => model.Counter.ToString();
        public string WelcomeLabel => model.WelcomeLabel;
        public Sprite IncreaseCounterButtonSprite => model.BundleAsset.IncreaseCounterButton;

        public void Dispose()
        {
            if(tokenSource is { IsCancellationRequested: false })
                tokenSource.Cancel();
            
            Unsubscribe();
        }
        
        private void Subscribe()
        {
            model.OnCounterChanged += Model_CounterChanged;
            model.OnContentChanged += Model_ContentChanged;
        }

        private void Unsubscribe()
        {
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
            
            view.CounterLabel = CounterLabel;
            view.WelcomeLabel = WelcomeLabel;
            view.IncreaseCounterButtonSprite = IncreaseCounterButtonSprite;
        }

        #region ViewCallbacks

        public void SetView(IMainScreen view)
        {
            this.view = view;
        }

        public void VisibleChanged(bool isVisible)
        {
            if (isVisible)
            {
                Subscribe();
                RefreshView();
            }
            else
            {
                Unsubscribe();
            }
        }

        public void IncreaseCounter()
        {
            model.IncreaseCounter();
        }
        
        public void UpdateContent()
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
            view.IncreaseCounterButtonSprite = model.BundleAsset.IncreaseCounterButton;
        }

        #endregion
    }
}