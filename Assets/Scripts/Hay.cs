using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hay : MonoBehaviour
{

    public GameObject hay;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnFire()
    {
        Destroy(hay);
    }
}
