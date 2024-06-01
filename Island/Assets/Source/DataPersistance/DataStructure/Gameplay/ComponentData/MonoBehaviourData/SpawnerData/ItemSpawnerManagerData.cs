using System;
using UnityEngine;

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class ItemSpawnerManagerData : SpawnerData
    {
        public bool wasSpawn;

        public ItemSpawnerManagerData() { }

        public ItemSpawnerManagerData(string key, bool wasSpawn)
        {
            this.key = key;
            this.wasSpawn = wasSpawn;
        }
    }
}