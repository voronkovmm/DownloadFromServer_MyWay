using UnityEngine;
using UnityEngine.UI;

namespace Features.UI.Screens.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Image imgProgress;

        private void Start()
        {
            Progress = 0;
        }

        public float Progress
        {
            get => imgProgress.fillAmount;
            set => imgProgress.fillAmount = value;
        }
    }
}
