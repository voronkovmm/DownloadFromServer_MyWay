using UnityEngine;

namespace Core.Scripts.Managers
{
    public class CanvasManager : MonoBehaviour
    {
        [field: SerializeField] public Canvas LoadinScreenCanvas { get; private set; }
        [field: SerializeField] public Canvas MainScreenCanvas { get; private set; }

        public void SetActiveLoadingScreenCanvas(bool value)
        {
            LoadinScreenCanvas.gameObject.SetActive(value);
        }
        
        public void SetActiveMainScreenCanvas(bool value)
        {
            MainScreenCanvas.gameObject.SetActive(value);
        }
    }
}