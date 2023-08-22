using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameObject indicator;
    
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;
    }

    void OnMouseEnter()
    {
        indicator.gameObject.SetActive(true);
        // Debug.Log(String.Format("{0}", transform.position));
    }

    void OnMouseExit()
    {
        indicator.gameObject.SetActive(false);
    }
}
