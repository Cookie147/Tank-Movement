using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hay : MonoBehaviour
{
    public GameObject firePrefab;
    private ParticleSystem fireParticles;


    void Start()
    {

        //get all animation components
        fireParticles = Instantiate(firePrefab).GetComponent<ParticleSystem>();

        fireParticles.gameObject.SetActive(false);
    }

    public void SetOnFire()
    {
        fireParticles.transform.position = transform.position;
        fireParticles.gameObject.SetActive(true);

        fireParticles.Play();

        //tankExplosionAudio.Play();

        Destroy(fireParticles);
        Destroy(gameObject);
    }
}
