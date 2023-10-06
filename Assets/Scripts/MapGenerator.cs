using System;
using System.Collections.Generic;
using System.Linq;
using AlanSartorio.GridPathGenerator;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private float tileSize;
    [SerializeField] private int tilesPerCell;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject spawnerPrefab;
    [SerializeField] private GameObject baseObject;
    [SerializeField] private GameObject pathObject;
    private MapGeneratorFacade _mapGenerator;
    private Dictionary<Vector2Int, GameObject> _spawners = new();

    private void Awake()
    {
        _mapGenerator = new MapGeneratorFacade();
    }

    private void Start()
    {
        var node = _mapGenerator.Initialize();
        AddCell(node);
    }

    public void ExpandMap(Vector2Int position)
    {
        var node = _mapGenerator.ExpandMap(position);
        AddCell(node);
    }

    void AddCell(MapDelta delta)
    {
        var node = delta.AddedNode;
        var newNode = Instantiate(nodePrefab, transform);
        newNode.transform.localPosition = NodePositionToWorldPosition(node.Position);

        foreach (var child in node.Children)
        {
            for (int i = 0; i < tilesPerCell; i++)
            {
                var path = Instantiate(pathObject);
                path.transform.localPosition = NodePositionToWorldPosition(delta.AddedNode.Position, child.Direction.GetDirection() * (i + 1));
                // var rotation = (int)child.Direction * 90;
                // path.transform.localRotation = Quaternion.AngleAxis(rotation, Vector3.up);
            }
        }

        var toSkip = 0;
        if (_spawners.TryGetValue(node.Position, out var spawner) && node.Children.Length > 0)
        {
            var child = node.Children[0];
            var newPos = node.Position + child.Direction.GetDirection();
            _spawners.Remove(node.Position);
            SetSpawner(spawner, newPos, child.Path);
            _spawners[newPos] = spawner;
            toSkip++;
        }

        foreach (var child in node.Children.Skip(toSkip))
        {
            AddSpawner(node.Position + child.Direction.GetDirection(), child.Path);
        }
    }

    void SetSpawner(GameObject spawner, Vector2Int pos, Path<Vector2Int> path)
    {
        spawner.transform.localPosition = NodePositionToWorldPosition(pos);
        spawner.GetComponent<EnemySpawner>().path = new EnemyPath(
            path.Nodes.Take(path.Nodes.Count - 1).Select(NodePositionToWorldPosition).ToArray(), baseObject);
        spawner.GetComponentInChildren<ExpandOnClick>().position = pos;
    }

    void AddSpawner(Vector2Int pos, Path<Vector2Int> path)
    {
        var obj = Instantiate(spawnerPrefab);
        SetSpawner(obj, pos, path);
        _spawners[pos] = obj;
    }

    private Vector3 NodePositionToWorldPosition(Vector2Int pos)
    {
        return NodePositionToWorldPosition(pos, Vector2Int.zero);
    }

    private Vector3 NodePositionToWorldPosition(Vector2Int pos, Vector2Int tile)
    {
        var xz = pos * tilesPerCell + tile;
        return new Vector3(xz.x, 0, xz.y) * tileSize;
    }
}