using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class CoconutSpawnersData
    {
        public CoconutSpawnerData[] data;

        public CoconutSpawnersData() { }
        public CoconutSpawnersData(CoconutSpawnerData[] data)
        {
            this.data = data;
        }
    }

    [Serializable]
    public class CoconutSpawnerData
    {
        public string key;

        public bool wasStartSpawn;

        public CoconutSpawnerData() { }

        public CoconutSpawnerData(string key, bool wasStartSpawn)
        {
            this.key = key;
            this.wasStartSpawn = wasStartSpawn;
        }
    }
}
