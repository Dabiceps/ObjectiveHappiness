using System.Resources;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public LayerMask buildableLayer;
    public float maxPlacementDistance = 100f;

    private BuildingData currentData;
    private GameObject currentGhost;
    private bool isPlacing = false;

    void Awake()
    {
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
            currentGhost.transform.position = hit.point;

            bool valid = IsPlacementValid(hit.point);

            SetGhostColor(valid ? Color.green : Color.red);
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

        if (!IsPlacementValid(pos))
            return false;

        SpendResources(currentData);

        Instantiate(currentData.prefab, pos, Quaternion.identity);

        return true;
    }

    void EndPlacement()
    {
        Destroy(currentGhost);
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
