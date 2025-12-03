using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuildingButton : MonoBehaviour, IPointerClickHandler
{
    public BuildingData buildingData;

    public void OnPointerClick(PointerEventData eventData)
    {
        BuildingManager.Instance.StartPlacing(buildingData);
    }
}
