using UnityEngine;

public abstract class ObjectIDMaker : MonoBehaviour
{
    [SerializeField] protected string _headName = string.Empty;

    public string GetHeadName()
    {
        return _headName;
    }
}
