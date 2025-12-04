using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public AffichageUI UI;

    public int wood = 0;
    public int stone = 0;
    public int food = 0;
    public int residents = 5;
    public float prosperity = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un RessourceManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void Update()
    {
        prosperity += 0.01f;
        UI.SetResource(wood.ToString(), stone.ToString(), food.ToString(), residents.ToString());
    }
}
