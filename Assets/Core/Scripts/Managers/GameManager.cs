using System;
using UnityEngine;

namespace Core.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public event Action OnApplicationQuitEvent;

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
        }
    }
}