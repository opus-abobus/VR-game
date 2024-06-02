using System;
using System.Xml.Serialization;
using static DataPersistence.Gameplay.BerrySpawnManagerData;
using static DataPersistence.Gameplay.InventoryData;
using static DataPersistence.Gameplay.SOS_ManagerData;

namespace DataPersistence.Gameplay
{
    [Serializable]
    [XmlInclude(typeof(HungerSystemData))]
    [XmlInclude(typeof(InventoryData))]
    [XmlInclude(typeof(InventorySlotData))]
    [XmlInclude(typeof(GameTimeData))]
    [XmlInclude(typeof(BerrySpawnZoneData))]
    [XmlInclude(typeof(SignalGunData))]
    [XmlInclude(typeof(BonfireData))]
    [XmlInclude(typeof(SOS_ManagerData))]
    [XmlInclude(typeof(PlaceholdingData))]
    public abstract class MonoBehaviourData : ComponentData { }
}