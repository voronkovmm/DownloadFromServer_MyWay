using System;
using Core.Scripts.Managers;

namespace Core.Scripts.SaveLoad
{
    public interface ISaveLoad
    {
        void Save();
        bool TryGetSave<TExpected>(ISaveble saveble, out TExpected save);
        void LoadSaves();
    }

    public interface ISaveble
    {
        string SaveKey { get; }
    }
    
    public class SaveLoadService : IDisposable
    {
        private readonly ISaveLoad saveLoad;
        private readonly IGameEvents gameEvents;

        public SaveLoadService(ISaveLoad saveLoad, IGameEvents gameEvents)
        {
            this.gameEvents = gameEvents;
            this.saveLoad = saveLoad;

            gameEvents.OnApplicationQuitEvent += GameManager_ApplicationQuit;
        }

        public void Dispose()
        {
            gameEvents.OnApplicationQuitEvent -= GameManager_ApplicationQuit;
        }

        public void Save()
        {
            saveLoad.Save();
        }

        public bool TryGetSave<TExpected>(ISaveble saveble, out TExpected save)
        {
            return saveLoad.TryGetSave<TExpected>(saveble, out save);
        }

        private void GameManager_ApplicationQuit()
        {
            Save();
        }
    }
}