using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot_rotation : MonoBehaviour
{
    public GameObject[] arms =
    {

    };

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
       for (int i = 0; i < arms.Length; ++i){
          Rotate(i, -0.3f);
       }
    }

    public void Rotate(int arm_index, float alpha)
    {
      if (arm_index < 0 || arm_index > arms.Length)
        return;
      arms[arm_index].transform.Rotate(0, (arm_index > 0) ? 0 : alpha, (arm_index > 0) ? alpha : 0);

    }
}
