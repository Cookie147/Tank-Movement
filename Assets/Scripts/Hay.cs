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
        //set position
        fireParticles.transform.position = transform.position + new Vector3(0,2,-0.5f);
        fireParticles.gameObject.SetActive(true);

        fireParticles.Play();

        Destroy(fireParticles, 7);
        Destroy(gameObject, 7);
    }
}
