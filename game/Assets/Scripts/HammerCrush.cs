using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerCrush : MonoBehaviour {

    public float movementDownDirection;
    // Use this for initialization
    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + 1 * movementDownDirection, 
            transform.position.z
            );

        if(transform.position.y < 5)
        {
            movementDownDirection = 1;
        }

        if (transform.position.y > 20)
        {
            movementDownDirection = -1;
        }
    }
}
