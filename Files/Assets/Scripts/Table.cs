using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    private SplatManager splats;
    private Vector3 leftWallPos;
    private Vector3 rightWallPos; 

	void Start ()
    {
        splats = this.GetComponentInChildren<SplatManager>();
        leftWallPos = GameObject.Find("Walls/Left Wall").transform.position;
        rightWallPos = GameObject.Find("Walls/Right Wall").transform.position;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Sphere(Clone)")
        {
            splats.GenerateSplat("Table", col.transform.position, col.relativeVelocity);

            int random = Random.Range(0, 10);

            if(random <= 5)
            {
                splats.GenerateSplat("Left Wall", leftWallPos, col.relativeVelocity);
            }
            else
            {
                splats.GenerateSplat("Right Wall", rightWallPos, col.relativeVelocity);
            }
        }
    }
}
