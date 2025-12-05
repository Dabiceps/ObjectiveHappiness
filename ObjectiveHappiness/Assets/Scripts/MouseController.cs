using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseController : MonoBehaviour
{
    public Camera cam;
    private GameObject currentObject;
    private NavMeshAgent currentAgent;

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
                        OpenVillagerMenu();
                        break;
                    case "Ground":
                        break;
                    case "Building":
                        Debug.Log("Building clicked: " + hit.collider.name);
                        break;
                    default:
                        Debug.Log("Clicked on: " + hit.collider.name);
                        break;
                }
            }
       }

       void OpenVillagerMenu()
       {
            // Implementation for opening villager menu
       }

    }
}
