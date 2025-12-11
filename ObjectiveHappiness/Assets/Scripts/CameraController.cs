using UnityEngine;

public class CameraControllerTopDown : MonoBehaviour
{
    [Header("Déplacement")]
    public float moveSpeed = 20f;

    [Header("Zoom vertical + rotation")]
    public float zoomSpeed = 10f;

    public float minHeight = 5f;
    public float maxHeight = 25f;

    public float minAngle = 35f;  // angle proche du sol
    public float maxAngle = 65f;  // angle haut

    private Transform cam;

    // --- AJOUT pour la map ---
    [Header("Map Bounds")]
    public int mapSizeInTiles = 4;   // Doit correspondre au générateur
    public float tileSize = 10f;     // Tes tiles font 10x10

    private float minX, maxX, minZ, maxZ;
    // ---------------------------

    void Start()
    {
        cam = Camera.main.transform;

        CenterCamera();
        CalculateBounds();
        SetMinHeightAndAngle();   // *** NOUVEAU ***
    }

    void Update()
    {
        HandleMovement();
        HandleZoomHeightRotation();
        ClampCamera();
    }

    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        transform.position += direction.normalized * moveSpeed * Time.deltaTime;
    }

    void HandleZoomHeightRotation()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;

        float newY = cam.position.y - scroll * zoomSpeed;
        newY = Mathf.Clamp(newY, minHeight, maxHeight);

        Vector3 pos = cam.position;
        pos.y = newY;
        cam.position = pos;

        float t = Mathf.InverseLerp(minHeight, maxHeight, newY);
        float angle = Mathf.Lerp(minAngle, maxAngle, t);

        Vector3 rot = cam.localEulerAngles;
        rot.x = angle;
        cam.localEulerAngles = rot;
    }

    // -----------------------------
    //        AJOUT DES FONCTIONS
    // -----------------------------

    void CenterCamera()
    {
        float mapSize = mapSizeInTiles * tileSize;
        float half = mapSize / 2f;

        Vector3 pos = transform.position;
        pos.x = half;
        pos.z = half;
        transform.position = pos;
    }

    void CalculateBounds()
    {
        float mapSize = mapSizeInTiles * tileSize;

        minX = 0f;
        maxX = mapSize;
        minZ = 0f;
        maxZ = mapSize;
    }

    void ClampCamera()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }

    // ------------ NOUVEAU -------------
    // Place la caméra à la hauteur minimale + angle minimal
    void SetMinHeightAndAngle()
    {
        // Hauteur minimale
        Vector3 pos = cam.position;
        pos.y = minHeight;
        cam.position = pos;

        // Angle minimal (basse inclinaison)
        Vector3 rot = cam.localEulerAngles;
        rot.x = minAngle;
        cam.localEulerAngles = rot;
    }
}
