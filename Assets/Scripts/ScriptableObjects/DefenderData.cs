using UnityEngine;

[CreateAssetMenu(fileName = "Defender", menuName = "DefenderData", order = 1)]
public class DefenderData : ScriptableObject
{
    public float cost;
    public GameObject model;
    public GameObject gameObject;
    public Sprite icon;
    public bool onPath;
}