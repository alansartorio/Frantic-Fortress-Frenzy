using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update() {
        transform.rotation = mainCamera.transform.rotation;
    }
}
