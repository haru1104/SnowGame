using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float turnSpeed=100.0f;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -1 * turnSpeed);
    }
}
