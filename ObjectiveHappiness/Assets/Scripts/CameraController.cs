using UnityEngine;

public class CameraControllerTopDown : MonoBehaviour
{
    [Header("Déplacement latéral")]
    public float moveSpeed = 20f;
    public float edgeSize = 20f;

    [Header("Zoom vertical + rotation")]
    public float zoomSpeed = 10f;

    public float minHeight = 5f;
    public float maxHeight = 25f;

    public float minAngle = 35f;  // angle quand on est proche du sol
    public float maxAngle = 65f;  // angle quand on est haut

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
        HandleZoomHeightRotation();
    }

    void HandleMovement()
    {
        Vector3 direction = Vector3.zero;
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= edgeSize) direction += Vector3.left;
        if (mousePos.x >= Screen.width - edgeSize) direction += Vector3.right;
        //if (mousePos.y <= edgeSize) direction += Vector3.back;
        //if (mousePos.y >= Screen.height - edgeSize) direction += Vector3.forward;

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void HandleZoomHeightRotation()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f) return;

        // 1) Modifier la hauteur
        float newY = cam.position.y - scroll * zoomSpeed;
        newY = Mathf.Clamp(newY, minHeight, maxHeight);

        Vector3 pos = cam.position;
        pos.y = newY;
        cam.position = pos;

        // 2) Adapter l'angle suivant la hauteur
        float t = Mathf.InverseLerp(minHeight, maxHeight, newY);
        float angle = Mathf.Lerp(minAngle, maxAngle, t);

        Vector3 rot = cam.localEulerAngles;
        rot.x = angle;
        cam.localEulerAngles = rot;
    }
}
