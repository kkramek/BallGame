using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour {

    public Rigidbody rb;
    public int rollSide = 1;

    void Update()
    {
        rb = GetComponent<Rigidbody>();

        //Ruch kuli
        Vector3 movement = new Vector3(10 * rollSide, 0, 0);
        rb.AddForce(movement * 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //W przypaku kolizji ze ścianą lub piłką zmieniamy kierunek toczenia
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("PkBall"))
        {
            rollSide = rollSide * -1;
        }
    }
}
