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

    [field: SerializeField] public GameObject Berry { get; private set; } = null;

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