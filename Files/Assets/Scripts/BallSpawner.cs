using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject ball;
    private Rigidbody rigidBody;

    public float speed;

	void Start ()
    {
        InvokeRepeating("launchBall", 2f, 3f);
    }

    void launchBall()
    {
        rigidBody = Instantiate(ball.GetComponentInChildren<Rigidbody>(), this.transform.position, Quaternion.identity);
        rigidBody.velocity += new Vector3(0f, 0f, speed);
    }
}
