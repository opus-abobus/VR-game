using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class CoconutSpawnerData : SpawnerData
    {
        public bool wasStartSpawn;

        public CoconutSpawnerData() { }
        public CoconutSpawnerData(string key, bool wasStartSpawn)
        {
            this.key = key;
            this.wasStartSpawn = wasStartSpawn;
        }
    }
}