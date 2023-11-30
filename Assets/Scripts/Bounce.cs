using UnityEngine;

public class Bounce : MonoBehaviour
{
    private float t = 0;
    private float period = 1f;
    
    void Update()
    {
        t += Time.deltaTime;
        if (t > period)
        {
            t -= period;
        }

        var p = t / period;
        var transform = this.transform;
        transform.localPosition = new Vector3(0, Mathf.Abs(Mathf.Sin(p * Mathf.PI * 2)), 0);
        transform.localRotation = Quaternion.Euler(Mathf.Sin((p + .25f) * Mathf.PI * 2) * 5f, -90, Mathf.Sin(p * Mathf.PI * 2) * 5f);
    }
}
