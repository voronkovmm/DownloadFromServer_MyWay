using System;
using UnityEngine;

namespace Core.Scripts.Managers
{
    public interface IGameEvents
    {
        event Action OnApplicationQuitEvent;
    }
    
    public class GameEvents : MonoBehaviour, IGameEvents
    {
        public event Action OnApplicationQuitEvent;

        private void OnApplicationQuit()
        {
            OnApplicationQuitEvent?.Invoke();
        }
    }
}