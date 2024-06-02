using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace DataPersistence
{
    public class XMLSaveSystem : ISaveSystem
    {
        TData ISaveSystem.Load<TData>(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return default(TData);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TData));

            TData savedData = default(TData);

            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                savedData = (TData) serializer.Deserialize(fs);
            }

            return savedData;
        }

        void ISaveSystem.Save<TData>(TData gameData, string fullSavePath)
        {
            if (!File.Exists(fullSavePath))
            {
                //return;
                File.Create(fullSavePath).Close();
            }

            XmlSerializer serializer = new XmlSerializer(typeof(TData));

            using (StreamWriter fs = new StreamWriter(fullSavePath))
            {
                serializer.Serialize(fs, gameData);
            }
        }
    }
}
