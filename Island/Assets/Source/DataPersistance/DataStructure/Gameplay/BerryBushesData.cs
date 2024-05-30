using System;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class BerryBushesData
    {
        public BerryBushData[] data;

        public BerryBushesData() { }
        public BerryBushesData(BerryBushData[] data)
        {
            this.data = data;
        }     
    }

    [Serializable]
    public class BerryBushData
    {
        public string key;

        public bool wasStartSpawn;

        public BerrySpawnZoneData[] zonesData;

        public BerryBushData() { }

        public BerryBushData(string key, bool wasStartSpawn, BerrySpawnZoneData[] zonesData)
        {
            this.key = key;
            this.wasStartSpawn = wasStartSpawn;
            this.zonesData = zonesData;
        }
    }
}
