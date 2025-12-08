using System.Resources;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [Header("Ghost Settings")]
    public float ghostHeightOffset = 0.5f; // distance au-dessus du sol
    public NavMeshSurface navMeshSurface;

    [Header("Construction site")]
    public GameObject constructionSitePrefab;

    public LayerMask buildableLayer;
    public float maxPlacementDistance = 100f;

    private BuildingData currentData;
    private GameObject currentGhost;
    private bool isPlacing = false;

    public GameObject parent;

    private void Start()
    {
        parent = GameObject.Find("Buildings");
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plus d’un BuildingManager détecté dans la scène ! Un a été supprimé.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        if (!isPlacing) return;

        HandleGhostMovement();
        HandlePlacementInput();
    }

    public void StartPlacing(BuildingData data)
    {
        if (!HasResources(data))
        {
            Debug.Log("Pas assez de ressources !");
            return;
        }

        currentData = data;

        currentGhost = Instantiate(data.ghostPrefab);
        isPlacing = true;
    }

    void HandleGhostMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxPlacementDistance, buildableLayer))
        {
            Vector3 ghostPos = hit.point;
            ghostPos.y += ghostHeightOffset;  // Décalage pour ne pas clipper
            currentGhost.transform.position = ghostPos;


            bool valid = IsPlacementValid(hit.point);

            SetGhostColor(valid ? Color.green : Color.red);
        }

        if (Input.GetMouseButton(1))
        {
            CancelPlacement();
        }

        if (Input.GetMouseButton(2))
        {
            currentGhost.transform.Rotate(Vector3.up, 1f);
        }

    }

    void HandlePlacementInput()
    {
        // Valider
        if (Input.GetMouseButtonDown(0))
        {
            if (TryPlaceBuilding())
            {
                EndPlacement();
            }
        }

        // Annuler
        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }

    bool TryPlaceBuilding()
    {
        Vector3 pos = currentGhost.transform.position;
        Quaternion rot = currentGhost.transform.rotation;

        if (!IsPlacementValid(pos))
            return false;

        SpendResources(currentData);

        // On instancie un chantier
        var site = Instantiate(constructionSitePrefab, pos, rot);
        site.transform.SetParent(parent.transform, worldPositionStays: true);

        // On renseigne quel bâtiment sera construit
        ConstructionSite cs = site.GetComponent<ConstructionSite>();
        cs.buildingData = currentData;

        return true;
    }

    void EndPlacement()
    {
        Destroy(currentGhost);
        navMeshSurface.BuildNavMesh();
        isPlacing = false;
        currentGhost = null;
        currentData = null;
    }

    void CancelPlacement()
    {
        Destroy(currentGhost);
        isPlacing = false;
        currentGhost = null;
        currentData = null;
    }

    bool IsPlacementValid(Vector3 pos)
    {
        
        // Ici tu peux ajouter :
        // - overlap check
        // - grid alignment
        // - zone constraints

        return true; // Pour l’instant toujours valide
    }

    void SetGhostColor(Color c)
    {
        foreach (Renderer r in currentGhost.GetComponentsInChildren<Renderer>())
        {
            r.material.color = c;
        }
    }

    // ---------------- RESSOURCES ----------------

    bool HasResources(BuildingData data)
    {
        return ResourceManager.Instance.wood >= data.costWood &&
               ResourceManager.Instance.stone >= data.costStone;
    }

    void SpendResources(BuildingData data)
    {
        ResourceManager.Instance.wood -= data.costWood;
        ResourceManager.Instance.stone -= data.costStone;
    }
}
