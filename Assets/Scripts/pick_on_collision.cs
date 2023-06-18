using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_on_collision : MonoBehaviour
{
    //robot game object needed to enable grabbing
    public GameObject robot;

    private void OnTriggerEnter(Collider cube)
    {
        if (cube.gameObject.tag == "Pickable")
        {
            robot_rotation robot_script = robot.GetComponent<robot_rotation>();
            robot_script.CanGrab(cube, this.gameObject);
        }
    }

    private void OnTriggerExit(Collider cube)
    {
        if (cube.gameObject.tag == "Pickable")
        {
            robot_rotation robot_script = robot.GetComponent<robot_rotation>();
            robot_script.CantGrab();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
