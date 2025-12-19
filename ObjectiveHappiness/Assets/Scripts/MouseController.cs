using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MouseController : MonoBehaviour
{
    public Camera cam;
    private GameObject currentObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                switch (hit.collider.tag)
                {
                    case "Villager":
                        currentObject = hit.collider.gameObject;
                        IJobInterface jobInterface = currentObject.GetComponent<IJobInterface>();
                        IdentityManager.Instance.OpenMenu(jobInterface);
                        Debug.Log("Clicked on: " + hit.collider.name);
                        break;
                    case "Ground":
                        IdentityManager.Instance.CloseMenu();
                        break;
                    case "Building":
                        IdentityManager.Instance.CloseMenu();
                        Debug.Log("Building clicked: " + hit.collider.name);
                        break;
                    default:
                        IdentityManager.Instance.CloseMenu();
                        Debug.Log("Clicked on: " + hit.collider.name);
                        break;
                }
            }
        }
    }
}
