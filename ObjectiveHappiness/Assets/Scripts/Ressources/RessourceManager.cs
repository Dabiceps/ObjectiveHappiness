using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public int wood = 100;
    public int stone = 100;

    void Awake()
    {
        Instance = this;
    }
}
