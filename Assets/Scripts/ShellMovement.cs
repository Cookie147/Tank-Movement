using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellMovement : MonoBehaviour
{

    public int speed;
    public int bounceCount;
    public int maxBounce;
    public float angle;
    public Rigidbody shell;
    public GameObject tankExplosionPrefab;
    public GameObject shellExplosionPrefab;
    public GameObject shotFiring;

    private Vector3 v;                                  //this is really important, see explanation at "Bounce()" function
    private GameObject myTank;
    private GameObject tankExplosion;                   //conatins an instance of the prefab, so that it can actually be played
    private GameObject shellExplosion;                  //conatins an instance of the prefab, so that it can actually be played
    private AudioSource tankExplosionAudio;
    private ParticleSystem tankParticles;
    private ParticleSystem shellParticles;

    // Start is called before the first frame update
    void Start()
    {
        shell = GetComponent<Rigidbody>();
        float x = Mathf.Sin(shell.rotation.eulerAngles.y * Mathf.PI / 180);
        float z = Mathf.Cos(shell.rotation.eulerAngles.y * Mathf.PI / 180);
        shell.velocity = new Vector3(x, 0, z).normalized * speed;
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
            Physics.IgnoreCollision(GetComponent<Collider>(), myTank.GetComponentInChildren<Collider>(), false);
        }
        //check if the angle of the shell corresponds to the angle of its movement, if not then adjust it
        //this is a problem when the shell hits a wall, without this correction, the shell will rotate wildly
        if(Mathf.Abs(angle - transform.eulerAngles.y) > 0.1f)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        //to visualize the problem stated above, use the next line. 
        //Debug.DrawRay(transform.position, transform.forward * 20, Color.green, 0f);
    }

    /*
     * When hitting another object, react accordingly
     * 
     * Wall: bounce off or get destroyed if it already bounced
     * Any Tank: destroy tank and shell, play explosion animation
     * Hay: destroy shot (as it goes into the hay)
     * Shot: destroy both shots, play sound
     * Mine: do nothing as this is done in the mine script
     */
    private void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.GetContact(0).otherCollider;
        if (other.CompareTag("Wall"))
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

    /*
     * bounces off a wall in a mathematically ideal way
     * 
     * Here, the Vector3 v becomes important. If in the first line, v is replaced with shell.velocity, the shells will have a 
     * way smaller output angle than input angle. Due to the physical collision the velocity in direction of the normal of the plane hit is set to 0.
     * So, to bounce mathematically ideal, we have to use the velocity of the last frame, which is stored in v
     */
    private void Bounce(Vector3 normal)
    {
        Vector3 direction = (Vector3.Reflect(v.normalized, normal)).normalized;
        shell.velocity = direction * speed;
        transform.rotation = Quaternion.LookRotation(shell.velocity);
        angle = transform.eulerAngles.y;
        v = shell.velocity;
    }

    /*
     * When a tank is currently colliding with a wall and then shoots at the wall, the shot never leaves the tank's collider. This is why we have to 
     * test tank hits not only in Enter, but also in Stay
     */
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
        Destroy(tankExplosion, 1.05f);
        Destroy(other.transform.parent.gameObject);
    }

    public void DestroyShot()
    {
        shellParticles.transform.position = transform.position;
        shellParticles.gameObject.SetActive(true);
        shellParticles.Play();
        //destroy the explosion with a small delay so it can play properly
        Destroy(shellExplosion, 1f);
        if (tankExplosion) Destroy(tankExplosion, 1.05f);
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
        Physics.IgnoreCollision(GetComponent<Collider>(), myTank.GetComponentInChildren<Collider>(), true);
    }
}
