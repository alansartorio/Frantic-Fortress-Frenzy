using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ExpandOnClick : MonoBehaviour
{
    public Vector2Int position;
    public int cost = 100;
    private bool canBuy = false;
    private GameDirector _gameDirector;

    public void PartialScoreChanged(int score)
    {
        canBuy = score >= cost;

        transform.GetChild(0).GetComponent<MeshRenderer>().material.color =
            canBuy ? new Color(1f, 1f, 1f, 0.025f) : new Color(1f, 0, 0, 0.2f);
    }


    private void Awake()
    {
        _gameDirector = FindObjectOfType<GameDirector>();
    }

    private void OnEnable()
    {
        _gameDirector.OnPartialScoreChange.AddListener(PartialScoreChanged);
    }

    private void OnDisable()
    {
        _gameDirector.OnPartialScoreChange.RemoveListener(PartialScoreChanged);
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!canBuy) return;
        FindObjectOfType<MapGenerator>().ExpandMap(position);
        _gameDirector.Spend(cost);
    }
}