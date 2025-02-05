using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Core.Features.UI.Screens.MainScreen
{
    public interface IMainScreen
    {
        string CounterLabel { set; }
        string WelcomeLabel { set; }
        Sprite IncreaseCounterButtonSprite { set; }
        void SetVisible(bool value);
    }
    
    public class MainScreen : MonoBehaviour, IMainScreen
    {
        private IMainScreenPresenter mainScreenPresenter;
        
        [SerializeField] private Button increaseCounterButton;
        [SerializeField] private TMP_Text counterLabel;
        [SerializeField] private TMP_Text welcomeLabel;

        [Inject]
        public void Construct(IMainScreenPresenter mainScreenPresenter)
        {
            this.mainScreenPresenter = mainScreenPresenter;
            mainScreenPresenter.SetView(this);
        }

        public string CounterLabel
        {
            set => counterLabel.text = value;
        }
        
        public string WelcomeLabel
        {
            set => welcomeLabel.text = value;
        }
        
        public Sprite IncreaseCounterButtonSprite
        {
            set => increaseCounterButton.image.sprite = value;
        }

        private void OnEnable()
        {
            UpdateCounterLabel();
            UpdateWelcomeLabel();
            UpdateIncreaseCounterButtonSprite();
        }

        public void SetVisible(bool value)
        {
            gameObject.SetActive(value);
            mainScreenPresenter.VisibleChanged(value);
        }
        
        private void UpdateCounterLabel()
        {
            CounterLabel = mainScreenPresenter.CounterLabel;
        }
        
        private void UpdateWelcomeLabel()
        {
            WelcomeLabel = mainScreenPresenter.WelcomeLabel;
        }
        
        private void UpdateIncreaseCounterButtonSprite()
        {
            IncreaseCounterButtonSprite = mainScreenPresenter.IncreaseCounterButtonSprite;
        }
        
        #region Events

        public void ClickIncreaseCounter()
        {
            mainScreenPresenter.IncreaseCounter();
        }

        public void ClickUpdateContent()
        {
            mainScreenPresenter.UpdateContent();
        }

        #endregion
    }
}