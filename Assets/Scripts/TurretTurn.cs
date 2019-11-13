using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTurn : MonoBehaviour
{

    public int rotationSpeed;
    public Rigidbody rb;
    public Camera cam;
    public Display display1;

    // Start is called before the first frame update
    void Start()
    {
        display1 = Display.main;
    }

    // Update is called once per frame
    void Update()
    {
        SetRotation();
    }

    void SetRotation()
    {
        if (!Input.mousePresent)
        {
            print("no mouse found!");
            return;
        }
        /*
        Vector3 mousePos = Input.mousePosition;
        Vector3 tankPos = rb.transform.position;
        Vector3 mouseDirection = cam.ScreenToWorldPoint(new Vector3(mousePos.x, cam.nearClipPlane, mousePos.y));
        //Vector3 direction = mousePos - tankPos;
        Quaternion rotation = Quaternion.Euler(mouseDirection);
        rb.transform.SetPositionAndRotation(tankPos, rotation);
        print(mouseDirection.ToString());
        */

        
        Vector3 mousePos = Input.mousePosition;
        mousePos.x += 25;
        mousePos.z = mousePos.y + 25;
        mousePos.y = 1.1f;
        mousePos.z = (mousePos.z / Screen.height) * 30;
        mousePos.x = (mousePos.x / Screen.width) * 46;
        Vector3 tankPos = rb.transform.position;
        //tankPos.x /= display1.systemHeight;
        //tankPos.y = tankPos.z / display1.systemWidth;
        Vector3 direction = tankPos - mousePos;
        //direction = Vector3.Cross(new Vector3(direction.x, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, direction.z));
        print(mousePos.ToString() + ", " + tankPos.ToString() + ", " + direction.ToString());
        //print("h: " + display1.systemHeight);
        rb.transform.SetPositionAndRotation(tankPos, Quaternion.Euler(direction));
    }
}
