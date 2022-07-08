using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameTestScript : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem GlowSystem;
    [SerializeField]
    private ParticleSystem AlphaSystem;
    [SerializeField]
    private Collider FlameCollider;

    private void OnTriggerEnter(Collider other)
    {
        {
            GlowSystem.Play(true);
            AlphaSystem.Play(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        GlowSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        AlphaSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }

    // Update is called once per frame
    void Update()
    {
        AlphaSystem.transform.rotation = Quaternion.LookRotation(Vector3.up);
        GlowSystem.transform.rotation = Quaternion.LookRotation(Vector3.up);
    }
}
