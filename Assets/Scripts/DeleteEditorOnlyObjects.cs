using UnityEngine;

public class DeleteEditorOnlyObjects : MonoBehaviour
{
    void Awake()
    {
        foreach (var obj in GameObject.FindGameObjectsWithTag("EditorNotPlayingOnly"))
        {
            Destroy(obj);
        }
    }
}