using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class DraggableDefender : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public DefenderData defender;

    Tile GetHoveringTile()
    {
        var ray = Camera.main!.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hitInfo)) return null;
        var hitTransform = hitInfo.transform;
        if (!hitTransform.parent || !hitTransform.parent.CompareTag("Tile")) return null;
        var tile = hitTransform.GetComponent<Tile>();
        if (tile.IsOccupied()) return null;

        return tile;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var tile = GetHoveringTile();
        if (!tile) return;

        if (tile is PathTile == defender.onPath)
        {
            transform.position = tile.transform.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(gameObject);
        var tile = GetHoveringTile();
        if (!tile) return;
        if (tile is PathTile != defender.onPath)
            return;

        FindObjectOfType<GameDirector>().Spend((int)defender.cost);
        tile.Occupant = Instantiate(defender.gameObject);
    }
}