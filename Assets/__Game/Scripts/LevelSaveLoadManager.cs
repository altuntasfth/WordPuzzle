using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace __Game.Scripts
{
    public class LevelSaveLoadManager : MonoBehaviour
    {
        public static LevelSaveLoadManager Instance;
        public const string fileName = "/levelsData.dat";
        
        private string path;
        
        private void Awake()
        {
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this; 
            }

            path = DataPath();
            Debug.Log(path);
        }
        
        public AllLevelsData Load()
        {
            AllLevelsData allLevelsData = null;
            
            if (File.Exists(path))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(path, FileMode.Open);

                allLevelsData = (AllLevelsData)bf.Deserialize(file);
                file.Close();
            }

            return allLevelsData;
        }
        
        public void SaveInitializedData(AllLevelsData data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(path);
            
            bf.Serialize(file, data);
            file.Close();
            
            Debug.Log("Initialize Data");
        }
        
        private string DataPath()
        {
            if (Directory.Exists(Application.persistentDataPath))
            {
                return Path.Combine(Application.persistentDataPath + fileName);
            }
            
            return Path.Combine(Application.streamingAssetsPath + fileName);
        }
    }
}