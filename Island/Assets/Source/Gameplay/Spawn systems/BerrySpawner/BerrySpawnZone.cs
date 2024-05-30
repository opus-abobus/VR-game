using System;
using UnityEngine;

public class BerrySpawnZone : MonoBehaviour
{
    public event Action<int, GameObject> BerryFell;

    public float CooldownTimeLeft = 0f;

    public int Index { get; private set; }
    public void SetIndex(int index)
    {
        Index = index;
    }

    public GameObject Berry { get; private set; } = null;

    public void SetBerry(GameObject berryObject)
    {
        Berry = berryObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (Berry != null && other == Berry.GetComponent<Collider>())
        {
            Berry.GetComponent<Rigidbody>().useGravity = true;

            Berry = null;

            BerryFell?.Invoke(Index, other.gameObject);
        }
    }
}

namespace DataPersistence.Gameplay
{
    [Serializable]
    public class BerrySpawnZoneData
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
