using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Core.Scripts.SaveLoad
{
    public class JsonSaveLoad : ISaveLoad, IInitializable
    {
        [Inject] private IObjectResolver resolver;
        
        private Dictionary<string, ISaveble> saves = new(20);

        private JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
        };

        private string savePath => Path.Combine(Application.persistentDataPath, "save.json");
        
        public void Initialize()
        {
            LoadSaves();
        }

        public bool TryGetSave<TExpected>(ISaveble saveble, out TExpected save)
        {
            if (saves.TryGetValue(saveble.SaveKey, out var saveObj) && saveObj is TExpected expectedSave)
            {
                save = expectedSave;
            }
            else
            {
                save = default;
            }

            return save != null;
        }

        public void Save()
        {
            var dataToSave = GetAllSavebles().ToDictionary(
                kvp => kvp.SaveKey,
                kvp => kvp
            );

            var serializedJson = JsonConvert.SerializeObject(dataToSave, settings);
            File.WriteAllText(savePath, serializedJson);
        }

        private IEnumerable<ISaveble> GetAllSavebles()
        {
            return resolver.Resolve<IEnumerable<ISaveble>>();
        }

        public void LoadSaves()
        {
            if (!File.Exists(savePath))
            {
                Debug.LogError("Файл сохранений не найден");
                return;
            }

            var saveFile = File.ReadAllText(savePath);
            if (string.IsNullOrWhiteSpace(saveFile))
            {
                Debug.LogWarning("Файл сохранений пустой");
                return;
            }

            var deserializedDict = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(saveFile);
            saves.Clear();

            foreach (var kvp in deserializedDict)
            {
                var type = Type.GetType(kvp.Value["$type"]?.ToString());
                if (type == null)
                {
                    continue;
                }

                try
                {
                    var saveble = (ISaveble)kvp.Value.ToObject(type, JsonSerializer.Create(settings));
                    saves.Add(kvp.Key, saveble);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Ошибка при десериализации ключа {kvp.Key}: {ex.Message}");
                }
            }
        }
    }
}