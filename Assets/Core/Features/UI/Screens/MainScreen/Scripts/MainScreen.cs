using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Features.UI.Screens.MainScreen
{
    public class MainScreen : MonoBehaviour
    {
        public event Action<bool> OnVisibleChanged;
        public event Action OnClickIncreaseCounter;
        public event Action OnClickUpdateContent;
        
        [SerializeField] private Button increaseCounterButton;
        [SerializeField] private TMP_Text counterLabel;
        [SerializeField] private TMP_Text welcomeLabel;

        public string CounterLabel
        {
            set => counterLabel.text = value;
        }

        public string WelcomeLabel
        {
            set => welcomeLabel.text = value;
        }

        public Sprite IncreaseCounterSprite
        {
            set => increaseCounterButton.image.sprite = value;
        }

        public void SetVisible(bool value)
        {
            gameObject.SetActive(value);
            OnVisibleChanged?.Invoke(value);
        }

        public void ClickIncreaseCounter()
        {
            OnClickIncreaseCounter?.Invoke();
        }

        public void ClickUpdateContent()
        {
            OnClickUpdateContent?.Invoke();
        }
    }
}