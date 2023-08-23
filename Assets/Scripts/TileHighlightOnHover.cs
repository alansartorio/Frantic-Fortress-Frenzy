using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHighlightOnHover : MonoBehaviour
{
    GameObject indicator;
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;
    }

    void OnMouseEnter()
    {
        indicator.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        indicator.gameObject.SetActive(false);
    }
}
