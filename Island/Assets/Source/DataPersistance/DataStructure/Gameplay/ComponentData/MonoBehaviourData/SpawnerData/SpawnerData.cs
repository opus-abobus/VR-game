using System;
using System.Xml.Serialization;

namespace DataPersistence.Gameplay
{
    [Serializable]
    [XmlInclude(typeof(BananaTreeManagerData))]
    [XmlInclude(typeof(CoconutSpawnerData))]
    [XmlInclude(typeof(BerrySpawnManagerData))]
    [XmlInclude(typeof(ItemSpawnerManagerData))]
    [XmlInclude(typeof(RockSpawnerData))]
    public abstract class SpawnerData : MonoBehaviourData
    {
        public string key;
    }
}