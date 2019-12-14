using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed;
    public int bounceCount;
    public int maxBounce;
    public float angle;
    public const float maxRayDistance = 55f;
    public Vector3 v;
    public Rigidbody shell;
    public GameObject shot;
    public string type = "Normal";//to be automatized later
    public GameObject tankExplosionPrefab;
    public GameObject shellExplosionPrefab;
    public GameObject tankExplosion;
    public GameObject shellExplosion;
    public GameObject shotFiring;

    private GameObject myTank;
    private AudioSource tankExplosionAudio;
    private ParticleSystem tankParticles;
    private ParticleSystem shellParticles;

    // Start is called before the first frame update
    void Start()
    {
        //SetBounceAngle();
        shell = GetComponent<Rigidbody>();
        shot = gameObject;
        speed = 8;
        float x = Mathf.Sin(shell.rotation.eulerAngles.y * Mathf.PI / 180);
        float z = Mathf.Cos(shell.rotation.eulerAngles.y * Mathf.PI / 180);
        shell.velocity = (new Vector3(x, 1, z)).normalized * speed;
        v = shell.velocity;
        angle = transform.eulerAngles.y;

        //get all animation components
        tankExplosion = Instantiate(tankExplosionPrefab);
        shellExplosion = Instantiate(shellExplosionPrefab);
        tankParticles = tankExplosion.GetComponent<ParticleSystem>();
        shellParticles = shellExplosion.GetComponent<ParticleSystem>();
        tankExplosionAudio = tankExplosion.GetComponent<AudioSource>();
        tankParticles.gameObject.SetActive(false);
        shellParticles.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //as soon as the shot bounced off a wall once, it should be able to hit the tank it was shot from again
        if(bounceCount > 0 && myTank)
        {
            Physics.IgnoreCollision(shell.GetComponent<Collider>(), myTank.GetComponentInChildren<Collider>(), false);
        }
        //check if the angle of the shell corresponds to the angle of its movement, if not then adjust it
        //this is a problem when the shell hits a wall, without this correction, the shell will rotate wildly
        if(Mathf.Abs(angle - transform.eulerAngles.y) > 0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.GetContact(0).otherCollider;
        if (other.CompareTag("Wall") || (other.CompareTag("Hay") && type == "Normal"))
        {
            ++bounceCount;
            if (bounceCount > maxBounce)
            {
                DestroyShot();
            }
            else
            {
                Bounce(collision.GetContact(0).normal);
            }

        }
        else if (other.CompareTag("Enemy Tank"))
        {
            if (other.gameObject == myTank && bounceCount == 0)
            {
                return;
            }
            DestroyTank(other.gameObject);
            DestroyShot();
        }
        else if (other.CompareTag("Shot"))
        {
            other.SendMessage("DestroyShot");
            GameObject ruedi = Instantiate(shotFiring);
            Destroy(ruedi, 0.5f);
            DestroyShot();
        }
        else if (other.CompareTag("Player Tank"))
        {
            if (other.gameObject == myTank && bounceCount == 0)
            {
                return;
            }
            //other.GetComponent<TankShooting>().DestroyTank(other.gameObject);
            DestroyTank(other.gameObject);
            DestroyShot();
        }
        else if (other.CompareTag("Hay"))
        {
            Destroy(tankExplosion);
            Destroy(shellExplosion);
            Destroy(gameObject);
        }
    }

    /**
     * bounces off a wall in a mathematically ideal way
     */
    private void Bounce(Vector3 normal)
    {
        /*
        float angle;
        Vector3 dir = other.transform.position - shell.transform.position;
        float currentAngle = shell.transform.eulerAngles.y;
        if (Mathf.Abs(dir.x) < Mathf.Abs(dir.z))
        {
            angle = 180 - currentAngle;
        }
        else if (Mathf.Abs(dir.z) < Mathf.Abs(dir.x))
        {
            angle = 360 - currentAngle;
        }
        else
        {
            angle = currentAngle + 180;
        }*/

        Vector3 direction = Vector3.Reflect(v.normalized, normal);
        shell.velocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(direction);
        angle = transform.eulerAngles.y;


        /*
        shot.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        shell.velocity = new Vector3(Mathf.Sin(angle * Mathf.PI / 180) * speed, 0, Mathf.Cos(angle * Mathf.PI / 180) * speed);
        */
    }

    private void OnTriggerStay(Collider other)
    {
        if (bounceCount == 0 && other.gameObject == myTank)
        {
            return;
        }
        if (other.CompareTag("Enemy Tank"))
        {
            DestroyTank(other.gameObject);
            DestroyShot();
        }
        else if (other.CompareTag("Player Tank"))
        {
            DestroyTank(other.gameObject);
            DestroyShot();
        }
    }

    public void DestroyTank(GameObject other)
    {
        tankParticles.transform.position = other.transform.position;
        tankParticles.gameObject.SetActive(true);
        tankParticles.Play();
        tankExplosionAudio.Play();
        //destroy the explosion with a small delay so it can play properly
        Destroy(tankExplosion, 1f);
        Destroy(other.transform.parent.gameObject);
    }

    public void DestroyShot()
    {
        shellParticles.transform.position = transform.position;
        shellParticles.gameObject.SetActive(true);
        shellParticles.Play();
        //destroy the explosion with a small delay so it can play properly
        Destroy(shellExplosion, 1f);
        if (tankExplosion) Destroy(tankExplosion, 1f);
        Destroy(gameObject);
    }

    /*
     * sets "myTank" to the given Gameobject
     * the shot needs to know which tank it was shot from so that it only hits this tank after bouncing once, 
     * all other tanks can be hit immediately
     * this function should always be called when a tank fires a shot
     * 
     * @param tank the tank that has shot the parent of this script instance
     */
    public void SetMyTank(GameObject tank)
    {
        myTank = tank;
        Physics.IgnoreCollision(shell.GetComponent<Collider>(), myTank.GetComponentInChildren<Collider>(), true);
    }
}
