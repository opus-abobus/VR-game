using System;
using System.Linq;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class GameplayData
    {
        public string displayName;

        public int difficultyID;

        public PlayerData playerData = new();

        public GameTimeData gameTimeData = new();

        public ObjectData[] gameObjectsData;

        public SpawnerData[] spawnersData;

        public T GetSpawnerData<T>(string key) where T : SpawnerData
        {
            return spawnersData.FirstOrDefault(x => x.key == key) as T;
        }

        public GameplayData() {
            displayName = "New game started by " + Environment.UserName + " at " + DateTime.Now;
        }

        public GameplayData(int difficultyID)
        {
            this.difficultyID = difficultyID;
            displayName = "New game started by " + Environment.UserName + " at " + DateTime.Now;
        }
    }
}
