using UnityEngine;

public class BananaTreeIDMaker : MonoBehaviour
{
    [SerializeField] private string _headName = "Banana tree";

    public string GetHeadName()
    {
        return _headName;
    }
}
