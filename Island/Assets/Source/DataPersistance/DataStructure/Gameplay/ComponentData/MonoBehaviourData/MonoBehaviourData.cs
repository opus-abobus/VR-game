using System;
using System.Xml.Serialization;
using static DataPersistence.Gameplay.BerrySpawnManagerData;
using static DataPersistence.Gameplay.InventoryData;

namespace DataPersistence.Gameplay
{
    [Serializable]
    [XmlInclude(typeof(HungerSystemData))]
    [XmlInclude(typeof(InventoryData))]
    [XmlInclude(typeof(InventorySlotData))]
    [XmlInclude(typeof(GameTimeData))]
    [XmlInclude(typeof(BerrySpawnZoneData))]
    public abstract class MonoBehaviourData : ComponentData { }
}