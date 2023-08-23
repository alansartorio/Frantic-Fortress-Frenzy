using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuySlot : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public DefenderData defender;
    public GameObject draggableDefender;
    
    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var draggingDefender = Instantiate(draggableDefender);
        draggingDefender.name = "DraggingDefender";
        draggingDefender.GetComponent<DraggableDefender>().toInstantiate = defender.gameObject;

        var defenderGameObject = Instantiate(defender.model, draggingDefender.transform);

        eventData.pointerDrag = draggingDefender;
    }
}
