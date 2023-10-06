using System;
using System.Collections.Generic;
using System.Linq;
using AlanSartorio.GridPathGenerator;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public enum NodeDirection
{
    Up,
    Right,
    Down,
    Left,
}

static class NodeDirectionExtensions
{
    public static Vector2Int GetDirection(this NodeDirection dir)
    {
        return dir switch
        {
            NodeDirection.Left => Vector2Int.left,
            NodeDirection.Up => Vector2Int.up,
            NodeDirection.Right => Vector2Int.right,
            NodeDirection.Down => Vector2Int.down,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }

    public static NodeDirection ToDirection(this Vector2Int dir)
    {
        return (dir.x, dir.y) switch
        {
            (-1, 0) => NodeDirection.Left,
            (0, 1) => NodeDirection.Up,
            (1, 0) => NodeDirection.Right,
            (0, -1) => NodeDirection.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
        };
    }
}

public class ChildWithPath
{
    public NodeDirection Direction { get; private set; }
    public Path<Vector2Int> Path { get; private set; }

    public ChildWithPath(NodeDirection direction, Path<Vector2Int> path)
    {
        Direction = direction;
        Path = path;
    }
}

public class Node
{
    public Vector2Int Position { get; private set; }
    public NodeDirection? Parent { get; private set; }
    public ChildWithPath[] Children { get; private set; }

    public Node(Vector2Int position, NodeDirection? parent, ChildWithPath[] children)
    {
        Position = position;
        Parent = parent;
        Children = children;
    }
}

public class MapDelta
{
    public Node AddedNode { get; private set; }

    public MapDelta(Node addedNode)
    {
        AddedNode = addedNode;
    }
}

public class MapGeneratorFacade
{
    private GridPathGenerator<Vector2Int> _generator = new(
        5,
        5,
        new Vector2IntNeighborGetter(),
        Vector2Int.zero
    );

    private readonly Dictionary<Vector2Int, Vector2Int?> _nodeParent = new();
    public UnityEvent onFinishExpanding = new();
    public UnityEvent<Vector2Int, Vector2Int> onAddNode = new();
    public UnityEvent onRemoved = new();
    public UnityEvent onSpawnAdded = new();

    public MapDelta Initialize()
    {
        return ApplyDelta(_generator.Initialize());
    }

    public MapDelta ExpandMap(Vector2Int position)
    {
        var delta = _generator.EnableNode(position);
        return ApplyDelta(delta);
    }

    private MapDelta ApplyDelta(NodesDelta<Vector2Int> delta)
    {
        foreach (var node in delta.removedNodes) _nodeParent.Remove(node.Position);

        foreach (var added in delta.addedNodes) _nodeParent[added.node.Position] = added.parent?.Position;

        Assert.AreEqual(1, delta.enabledNodes.Count);
        var enabled = delta.enabledNodes.First();

        // // Replace borders with entrances where determined
        // foreach (var enableable in _generator.GetEnableablePositions())
        // {
        //     var parent = _nodeParent[enableable];
        //     if (parent != null && _nodeBorderObjects.TryGetValue(parent.Value, out var parentBorders))
        //     {
        //         var angleIndex = DeltaToAngleIndex(enableable - parent.Value);
        //         var border = parentBorders[angleIndex];
        //         if (border.TryGetComponent<EntranceBehaviour>(out var entranceComponent))
        //             continue;
        //         var entrance = InstantiateWithAnimation(entranceObject, _objects[parent.Value].transform);
        //         entrance.transform.localPosition = border.transform.localPosition;
        //         entrance.transform.localRotation = border.transform.localRotation;
        //         entrance.GetComponent<EntranceBehaviour>().NodePosition = enableable;
        //         Destroy(border);
        //         parentBorders[angleIndex] = entrance;
        //     }
        // }
        //

        var paths = _generator.GetPathsFromLeaves().ToDictionary(p => p.Nodes[0]);

        return new MapDelta(
            new Node(enabled.Position,
                enabled.Parent == null ? null : (enabled.Parent.Position - enabled.Position).ToDirection(),
                enabled.Children.Select(c =>
                    new ChildWithPath((c.Position - enabled.Position).ToDirection(), paths[c.Position])).ToArray())
        );
    }

    public void Paths()
    {
        _generator.GetPathsFromLeaves();
    }
}