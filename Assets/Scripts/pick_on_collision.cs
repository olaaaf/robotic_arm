using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pick_on_collision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Pickable")
        {
            print("picked Brick");
            other.transform.parent = transform;
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
