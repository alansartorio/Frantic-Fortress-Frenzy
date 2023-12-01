using UnityEngine;

public class EnemyIdle : MonoBehaviour
{
    private float time = 0;
    public float jumpCooldown = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > jumpCooldown)
        {
            time = 0;
            Jump();
        }
    }

    private void Jump() {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 2f, ForceMode.VelocityChange);
    }
}
