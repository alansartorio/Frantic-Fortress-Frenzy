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
            occupant.transform.SetParent(transform.parent, false);
        }
    }

    public bool IsOccupied()
    {
        return Occupant;
    }
}