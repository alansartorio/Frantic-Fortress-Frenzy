using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuySlot : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public DefenderData defender;
    public GameObject draggableDefender;
    [SerializeField] private Image image;

    private void Start()
    {
        image.sprite = defender.icon;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        var director = FindObjectOfType<GameDirector>();
        if (director.HasEnoughScore((int)defender.cost))
        {
            var draggingDefender = Instantiate(draggableDefender);
            draggingDefender.name = "DraggingDefender";
            var draggableScript = draggingDefender.GetComponent<DraggableDefender>();
            draggableScript.defender = defender;

            var defenderGameObject = Instantiate(defender.model, draggingDefender.transform);

            eventData.pointerDrag = draggingDefender;
        }
    }
}