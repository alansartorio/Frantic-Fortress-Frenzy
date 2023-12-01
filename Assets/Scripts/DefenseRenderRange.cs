using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DefenseRenderRange : MonoBehaviour
{

    [Range(3, 256)]
    public int numSegments = 128;

    void Start()
    {
        DoRenderer();
        GetComponent<LineRenderer>().enabled = true;
    }


    public void DoRenderer()
    {
        float radius = GetComponent<SphereCollider>().radius;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0.1f, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}