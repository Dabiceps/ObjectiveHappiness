using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseController : MonoBehaviour
{
    public Camera cam;
    private GameObject currentObject;
    private NavMeshAgent currentAgent;
    public IdentityManager idManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                switch (hit.collider.tag)
                {
                    case "Villager":
                        currentObject = hit.collider.gameObject;
                        IJobInterface jobInterface = currentObject.GetComponent<IJobInterface>();
                        idManager.OpenMenu(jobInterface);
                        Debug.Log("Clicked on: " + hit.collider.name);
                        break;
                    case "Ground":
                        idManager.CloseMenu();
                        break;
                    case "Building":
                        idManager.CloseMenu();
                        Debug.Log("Building clicked: " + hit.collider.name);
                        break;
                    default:
                        Debug.Log("Clicked on: " + hit.collider.name);
                        break;
                }
            }
        }
    }
}
