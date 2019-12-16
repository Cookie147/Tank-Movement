using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurretTurn : MonoBehaviour
{

    public int rotationSpeed;
    public Rigidbody turret;
    public GameObject playground;
    public Camera cam;
    public Slider aimArrow;
    public GameObject canvasSystem;
    public Transform arrow;

    private int floorMask;
    private const float rayLength = 100f;


    // Start is called before the first frame update
    void Start()
    {
        if(!playground) playground = GameObject.Find("Play Area");
        if(!cam) cam = Camera.main;
        if(floorMask == 0) floorMask = LayerMask.GetMask("Floor");

    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
    }

    /**
     * Sets the rotation of the turret according to the postion of the mouse
     * the turret will always be turned in the direction where the mouse is
     */
    private void SetRotation()
    {
        if (!Input.mousePresent)
        {
            print("no mouse found!");
            return;
        }

        Vector3 mousePos = Input.mousePosition;
        Ray r = cam.ScreenPointToRay(mousePos);
        float d = 0;

        //Raycast from camera wiev point towards the mouse indicator
        if (Physics.Raycast(r, out RaycastHit hit, rayLength, floorMask))
        {
            Vector3 point = hit.point;
            point.y = transform.position.y;
            Vector3 direction = point - transform.position;
            direction.y = 0f;
            transform.rotation = Quaternion.LookRotation(direction);
            d = direction.magnitude;
        }

        //the arrow indicating where exactly the shot will go
        canvasSystem.transform.rotation = transform.rotation;
        aimArrow.value = d;
        arrow.localScale = new Vector3(ScaleArrow(d), 1, 2);
    }

    /*
     * scales the given value to a value between 0.5 and 1.5 reaching its minimum when 0 or less is given and its maximum when 50 or more is given
     * 
     * @param c the value to be scaled
     */
    private float ScaleArrow(float c)
    {
        if (c <= 0) return 0f;
        if (c >= 50) return 1.5f;
        float ret = c * 0.02f + 0.5f;
        return ret;
    }
}
