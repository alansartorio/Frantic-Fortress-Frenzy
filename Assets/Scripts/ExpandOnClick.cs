using UnityEngine;

public class ExpandOnClick : MonoBehaviour
{
    public Vector2Int position;

    private void OnMouseDown()
    {
        FindObjectOfType<MapGenerator>().ExpandMap(position);
    }
}
