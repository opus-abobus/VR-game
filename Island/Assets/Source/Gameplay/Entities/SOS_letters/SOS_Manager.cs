using UnityEngine;
using static SOS_Manager;

public class SOS_Manager : MonoBehaviour, ISosLetters {
    [HideInInspector] 
    public static float dayTimeChance = 0.1f;
    [HideInInspector] 
    public static float nightTimeChance = 0.01f;

    [HideInInspector] 
    public bool isSOSLayedOut = false;

    private int _placeholderCount = 0;

    public interface ISosLetters {
        void UpdatePlaceholderCount();
    }

    void ISosLetters.UpdatePlaceholderCount() {
        _placeholderCount++;

        if (transform.childCount == _placeholderCount) {
            isSOSLayedOut = true;

            EvacuationSystem.Instance.AddEvacItem(EvacuationSystem.EvacItem.TypesOfItems.sosRocks);
        }
    }
}
