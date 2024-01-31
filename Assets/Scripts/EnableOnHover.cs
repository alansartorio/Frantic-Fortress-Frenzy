using UnityEngine;

public class EnableOnHover : MonoBehaviour
{
    public GameObject indicator;

    private void Awake()
    {
        indicator.SetActive(false);
    }

    void OnMouseEnter()
    {
        indicator.SetActive(true);
    }

    void OnMouseExit()
    {
        indicator.SetActive(false);
    }
}
