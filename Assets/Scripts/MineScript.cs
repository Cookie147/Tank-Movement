using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{

    public int colour;
    public int TRIGGERRANGE;
    public float fenceTime;
    public float quickCharge;
    public float colliderRadius;
    public float timer;
    public bool exploded;
    public MeshRenderer mr;
    public Material yellowMat, redMat;
    public List<GameObject> objectsInRange;

    private float mineSize;
    private const int YELLOW = 0;
    private const int RED = 1;

    public GameObject tankExplosionPrefab;
    public GameObject shellExplosionPrefab;

    private AudioSource tankExplosionAudio;
    private ParticleSystem tankParticles;
    private ParticleSystem shellParticles;


    // Start is called before the first frame update
    void Start()
    {
        timer = 20f;
        colliderRadius = GetComponent<SphereCollider>().radius;
        mineSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        fenceTime -= Time.deltaTime;
        if (timer <= 0)
            Explode();
        else if (timer <= 5f)
            Flash();
        if (CheckObjects() && timer > 5f)
        {
            //activates a fast trigger, meaning the mine detected movement and will detonate early
            timer = quickCharge;
        }
    }

    /*
     * makes the mine explode and takes all units in the explosion radius with it
     */
    public void Explode()
    {
        if (exploded) return;
        exploded = true;
        foreach(GameObject g in objectsInRange)
        {
            if(g.CompareTag("Player Tank") || g.CompareTag("Enemy Tank"))
            {

            }
            else if (g.CompareTag("Mine"))
            {
                g.SendMessage("Explode");
                Destroy(g);
            }
            else if (g.CompareTag("Hay"))
            {
                g.SendMessage("SetOnFire");
                Destroy(g, 10);
            }
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
            if (!g)
            {
                print("removed " + g + " because it does not exist anymore");
                objectsInRange.Remove(g);
            }
            else if((g.CompareTag("Player Tank") || g.CompareTag("Enemy Tank")) && 
                     Vector3.Distance(transform.position, g.transform.position) < TRIGGERRANGE)
            {
                if (fenceTime < 0) return true;
            }
            else if (g.CompareTag("Shot") && 
                     Vector3.Distance(transform.position, new Vector3(g.transform.position.x, 0, g.transform.position.z)) < mineSize)
            {
                Explode();
            }
        }
        return false;
    }
    
    /*
     * whenever a new object enters the explosion radius (represented by the sphere collider), we save this object in the list 
     * "objectsInRange" so that we know which objects to
     * destroy when the mine explodes and so that we can check if any object gets near enough to trigger the mine.
     */
    public void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Wall") || other.CompareTag("Ground")))
        {
            objectsInRange.Add(other.gameObject);
        }
    }

    /*
     * removes any object that leaves the collider
     */
    public void OnTriggerExit(Collider other)
    {
        objectsInRange.Remove(other.gameObject);
    }

    public void DestroyTank(GameObject other)
    {
        tankParticles.transform.position = other.transform.position;
        tankParticles.gameObject.SetActive(true);

        tankParticles.Play();

        tankExplosionAudio.Play();
        Destroy(tankParticles);
        Destroy(other);
    }

    /*
     * when the timer is low the mine will begin to swap between red and yellow indicating that the explosion is near
     * the lower the timer the faster the materials will be switched
     */
    private void Flash()
    {
        if (timer > 5f) return;

        float t = 1;
        if      (timer > 1f)   t = 0.5f;
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
