using UnityEngine;

public class InventorySlotSprite : MonoBehaviour
{
    [SerializeField]
    private Sprite _spriteInInvenory;
    public Sprite SriteInInventory { get { return _spriteInInvenory; } }
}
