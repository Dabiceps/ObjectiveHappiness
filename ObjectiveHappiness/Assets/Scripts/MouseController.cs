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
                        break;
                    case "Ground":
                        if(currentObject != null)
                        {
                            if(currentObject.TryGetComponent<NavMeshAgent>(out currentAgent))
                            {
                                currentAgent.SetDestination(hit.point);
                                currentObject = null;
                            }
                            else
                            {
                                currentObject = null;
                            }
                        }
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
    }
}
