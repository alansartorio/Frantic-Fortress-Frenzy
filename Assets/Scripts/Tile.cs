using UnityEngine;

public class Tile : MonoBehaviour
{
    private GameObject occupant;
    public GameObject Occupant
    {
        get => occupant;
        set
        {
            occupant = value;
            occupant.transform.SetParent(transform.parent);
            occupant.transform.localPosition = Vector3.zero;
            occupant.transform.localRotation = Quaternion.identity;
            occupant.transform.localScale = Vector3.one;
        }
    }

    public bool IsOccupied()
    {
        return Occupant;
    }
}