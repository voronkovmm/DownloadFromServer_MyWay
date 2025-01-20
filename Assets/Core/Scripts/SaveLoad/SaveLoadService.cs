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
        private readonly GameManager gameManager;

        public SaveLoadService(ISaveLoad saveLoad, GameManager gameManager)
        {
            this.gameManager = gameManager;
            this.saveLoad = saveLoad;

            gameManager.OnApplicationQuitEvent += GameManager_ApplicationQuit;
        }

        public void Dispose()
        {
            gameManager.OnApplicationQuitEvent -= GameManager_ApplicationQuit;
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