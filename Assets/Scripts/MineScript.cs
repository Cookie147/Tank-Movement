using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{

    public int colour;
    public int TRIGGERRANGE;
    public float colliderRadius;
    public float timer;
    public MeshRenderer mr;
    public Material yellowMat, redMat;
    public List<GameObject> objectsInRange;

    private const int YELLOW = 0;
    private const int RED = 1;


    // Start is called before the first frame update
    void Start()
    {
        timer = 8f;
        colliderRadius = GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Explode();
        else if (timer <= 5f)
            Flash();
        if (CheckObjects())
        {
            //Trigger();
        }
    }

    /*
     * makes the mine explode and takes all units in the explosion radius with it
     */
    private void Explode()
    {
        foreach(GameObject g in objectsInRange)
        {
            Destroy(g);
        }
        Destroy(mr.gameObject);
    }

    /*
     * checks for all objects  if they are close enough to trigger the mine or far enough so that they won't be affected anymore if the mine explodes
     * 
     * @var objectsInRange won't contain objects that are too far away anymore
     * 
     * @return true if an object (specifically a tank) is near enough to trigger the mine, false otherwise
     */
    private bool CheckObjects()
    {
        foreach(GameObject g in objectsInRange)
        {
            if((g.CompareTag("Player Tank") || g.CompareTag("Enemy Tank")) && Vector3.Distance(transform.position, g.transform.position) < TRIGGERRANGE)
            {
                //Trigger();
                return true;
            }
            else if(Vector3.Distance(transform.position, g.transform.position) > colliderRadius)
            {
                objectsInRange.Remove(g);
            }
        }
        return false;
    }

    /*
     * whenever a new object enters the explosion radius (represented by the sphere collider), we save this object in the list "objectsInRange" so that we know which objects to
     * destroy when the mine explodes and so that we can check if any object gets near enough to trigger the mine.
     */
    public void OnTriggerEnter(Collider other)
    {
        for(int i=0; i<objectsInRange.Count; ++i)
        {
            if (!objectsInRange[i])
            {
                objectsInRange[i] = other.gameObject;
                return;
            }
        }
    }

    /*
     * when the timer is low the mine will begin to swap between red and yellow indicating that the explosion is near
     * the lower the timer the faster the materials will be switched
     */
    private void Flash()
    {
        if (timer > 5f) return;

        float t = 1;
        if      (timer > 2.5f) t = 1f;
        else if (timer > 1f)   t = 0.5f;
        else if (timer > 0f)   t = 0.25f;
        if (timer % t <= (t/2))
        {
            mr.material = redMat;
            colour = RED;
        }
        else
        {
            mr.material = yellowMat;
            colour = YELLOW;
        }
    }
}
