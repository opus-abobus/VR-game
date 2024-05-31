using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class BerrySpawnManagerData : SpawnerData
    {
        public bool wasStartSpawn;
        public BerrySpawnZoneData[] berrySpawnZonesData;

        public BerrySpawnManagerData() { }
        public BerrySpawnManagerData(string key, bool wasStartSpawn, BerrySpawnZoneData[] data)
        {
            this.key = key;
            this.wasStartSpawn = wasStartSpawn;
            this.berrySpawnZonesData = data;
        }


        [Serializable]
        public class BerrySpawnZoneData : MonoBehaviourData
        {
            public int index;
            public bool hasBerry;
            public float cooldownTimeLeft;

            public BerrySpawnZoneData() { }
            public BerrySpawnZoneData(int index, bool hasBerry, float cooldownTimeLeft)
            {
                this.index = index;
                this.hasBerry = hasBerry;
                this.cooldownTimeLeft = cooldownTimeLeft;
            }
        }
    }
}