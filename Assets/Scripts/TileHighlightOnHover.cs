using UnityEngine;

public class TileHighlightOnHover : MonoBehaviour
{
    GameObject indicator;
    private void Start()
    {
        indicator = transform.Find("Indicator").gameObject;
        indicator.SetActive(false);
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
