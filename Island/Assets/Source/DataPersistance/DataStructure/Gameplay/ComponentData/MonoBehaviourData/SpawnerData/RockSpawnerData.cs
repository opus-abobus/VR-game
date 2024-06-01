using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class RockSpawnerData : SpawnerData
    {
        public bool wasSpawn;

        public RockSpawnerData() { }
        public RockSpawnerData(string key, bool wasSpawn)
        {
            this.key = key;
            this.wasSpawn = wasSpawn;
        }
    }
}
