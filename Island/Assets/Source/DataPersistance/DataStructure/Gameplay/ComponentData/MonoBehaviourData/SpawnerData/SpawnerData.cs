using System;
using System.Xml.Serialization;

namespace DataPersistence.Gameplay
{
    [Serializable]
    [XmlInclude(typeof(BananaTreeManagerData))]
    [XmlInclude(typeof(CoconutSpawnerData))]
    [XmlInclude(typeof(BerrySpawnManagerData))]
    public abstract class SpawnerData : MonoBehaviourData
    {
        public string key;
    }
}