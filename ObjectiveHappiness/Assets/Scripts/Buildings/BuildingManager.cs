using System.Resources;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    [Header("Ghost Settings")]
    public float ghostHeightOffset = 0.5f;
    public NavMeshSurface navMeshSurface;

    [Header("Construction site")]
    public GameObject constructionSitePrefab;

    [Header("Placement Rules")]
    public LayerMask buildableLayer;
    public LayerMask nonBuildableLayer;
    public LayerMask buildingLayer;
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

    // ----------------------------
    // START PLACING
    // ----------------------------
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

    // ----------------------------
    // GHOST MOVEMENT
    // ----------------------------
    void HandleGhostMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        if (Physics.Raycast(ray, out RaycastHit hit, maxPlacementDistance, buildableLayer))
        {
            Vector3 ghostPos = hit.point;
            
            currentGhost.transform.position = ghostPos;
            bool valid = IsPlacementValid(hit.point); 
            SetGhostColor(valid ? Color.green : Color.red);
        }
 
        if (Physics.Raycast(ray, out RaycastHit hit2, maxPlacementDistance, nonBuildableLayer))
        {
            Vector3 ghostPos = hit2.point;
            
            currentGhost.transform.position = ghostPos;
            bool valid = IsPlacementValid(hit2.point); 
            SetGhostColor(valid ? Color.green : Color.red);
        }

            if (Input.GetMouseButton(2))
        {
            currentGhost.transform.Rotate(Vector3.up, 1f);
        }
    }

    // ----------------------------
    // INPUT VALIDATION
    // ----------------------------
    void HandlePlacementInput()
    {
        // Empêche la pose si la souris est sur un élément UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                CancelPlacement();
                return;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (TryPlaceBuilding())
            {
                EndPlacement();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelPlacement();
        }
    }

    // ----------------------------
    // PLACEMENT
    // ----------------------------
    bool TryPlaceBuilding()
    {
        Vector3 pos = currentGhost.transform.position;
        Quaternion rot = currentGhost.transform.rotation;

        if (!IsPlacementValid(pos))
            return false;

        SpendResources(currentData);

        var site = Instantiate(constructionSitePrefab, pos, rot);
        site.transform.SetParent(parent.transform, true);

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

    // ----------------------------
    // VALIDATION + OVERLAP CHECK
    // ----------------------------
    bool IsPlacementValid(Vector3 pos)
    {
        float radius = currentData.placementRadius;

        // Zone non-constructible
        if (Physics.CheckSphere(pos, 2, nonBuildableLayer))
            return false;
        if (Physics.CheckSphere(pos, radius, buildingLayer))
            return false;


            // Collision avec bâtiments existants
            Collider[] overlap = Physics.OverlapSphere(pos, radius, buildingLayer);
        if (overlap.Length > 0)
            return false;

        return true;
    }

    // ----------------------------
    // GHOST COLOR
    // ----------------------------
    void SetGhostColor(Color c)
    {
        foreach (Renderer r in currentGhost.GetComponentsInChildren<Renderer>())
        {
            foreach (var mat in r.materials)
            {
                mat.color = c;
            }
        }
    }

    // ----------------------------
    // RESSOURCES
    // ----------------------------
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
