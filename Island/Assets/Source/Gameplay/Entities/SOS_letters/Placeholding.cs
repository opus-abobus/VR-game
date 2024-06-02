using System;
using UnityEngine;
using Valve.VR.InteractionSystem;
using static DataPersistence.Gameplay.SOS_ManagerData;

public class Placeholding : MonoBehaviour
{
    public event Action<Placeholding> ItemPlaced;

    public int index;
    public bool itemPlaced;

    public void SetData(PlaceholdingData data)
    {
        if (data != null)
        {
            itemPlaced = data.isOccupied;
            index = data.index;
        }
    }

    public PlaceholdingData GetData()
    {
        return new PlaceholdingData(index, itemPlaced);
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        var interactable = other.GetComponent<Interactable>();
        if (((tag == "rock" || tag == "coconutUnbroken") &&
            interactable != null && interactable.attachedToHand != null &&
            interactable.attachedToHand.currentAttachedObject != null &&
            tag == other.GetComponent<Interactable>().attachedToHand.currentAttachedObject.tag) || (itemPlaced &&
            tag == "rock" || tag == "coconutUnbroken"))
        {

            if (!itemPlaced)
                other.GetComponent<Interactable>().attachedToHand.DetachObject(
                    other.GetComponent<Interactable>().attachedToHand.currentAttachedObject);

            Destroy(other.GetComponent<VelocityEstimator>());
            Destroy(other.GetComponent<Throwable>());
            Destroy(other.GetComponent<Interactable>());

            other.transform.parent = this.transform;
            other.transform.localPosition = Vector3.zero;
            other.transform.localRotation = Quaternion.identity;

            if (!itemPlaced)
            {
                itemPlaced = true;
                ItemPlaced?.Invoke(this);
            }

            Destroy(other.attachedRigidbody);
            Destroy(GetComponent<MeshRenderer>());
            Destroy(GetComponent<MeshFilter>());
            Destroy(GetComponent<Collider>());
            Destroy(this);
        }
    }
}
