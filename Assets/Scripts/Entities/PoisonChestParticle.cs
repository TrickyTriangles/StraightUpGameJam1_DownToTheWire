using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonChestParticle : MonoBehaviour
{
    [SerializeField] List<ParticleSystem> particles;
    [SerializeField] private int count;

    private void Awake()
    {
        foreach (var particle in particles)
        {
            DetectParticleEnd dpe = particle.GetComponent<DetectParticleEnd>();

            if (dpe != null)
            {
                dpe.Subscribe_Callback(DetectParticleEnd_Callback);
                count++;
            }
        }
    }

    private void DetectParticleEnd_Callback()
    {
        count--;
    }

    public IEnumerator ProcessParticleSystem()
    {
        foreach (var particle in particles)
        {
            particle.Play();
        }

        while (count > 0)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
