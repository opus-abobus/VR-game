using UnityEngine;
using UnityEngine.AddressableAssets;

public class ScenesDatabase : MonoBehaviour
{
    [SerializeField] public AssetReference 
        Intro,
        MainMenu,
        LevelLoading,
        MainLevel;

    public static ScenesDatabase Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
