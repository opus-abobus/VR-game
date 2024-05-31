using System;
using System.Xml.Serialization;

namespace DataPersistence.Gameplay
{
    [Serializable]
    [XmlInclude(typeof(ObjectData))]
    [XmlInclude(typeof(PlayerData))]
    public abstract class Data {}
}