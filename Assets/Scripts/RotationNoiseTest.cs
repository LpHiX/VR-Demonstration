using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationNoiseTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Collider WaterTrigger;
    private Rigidbody rb = null;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.Equals(WaterTrigger))
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.PerlinNoise(Time.realtimeSinceStartup, 0) - 0.5f, 0, Mathf.PerlinNoise(0, Time.realtimeSinceStartup) - 0.5f));
            rb.AddForce(new Vector3(Mathf.PerlinNoise(Time.realtimeSinceStartup, 0) - 0.5f, 0, Mathf.PerlinNoise(0, Time.realtimeSinceStartup) - 0.5f) * 40, ForceMode.Force);
            rb.AddForce(new Vector3(0, 30 / (1 + Mathf.Abs(transform.position.y - WaterTrigger.transform.position.y)), 0));
        }
    }
}
