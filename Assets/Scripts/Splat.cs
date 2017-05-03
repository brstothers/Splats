using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour
{
    public float animLength;
    public float maxTableScaler;
    public float maxWallScaler;
    public float collisionOffset;

    private static float currOffset;

    private string thing;
    private bool animating;
    private Vector3 scaler;
    private float startTime;
    private float size;

	// Use this for initialization
	void Start ()
    {
        if (thing == "Left Wall")
        {
            this.transform.position = new Vector3(transform.position.x + currOffset, transform.position.y, transform.position.z);
            currOffset += collisionOffset * 2f;
        }
        else if (thing == "Right Wall")
        {
            this.transform.position = new Vector3(transform.position.x - currOffset, transform.position.y, transform.position.z);
            currOffset += collisionOffset * 2f;
        }

        animating = true;
        startTime = Time.time;
        this.transform.localScale = new Vector3(0f, 0.01f, 0f);

        determineScaler();

        InvokeRepeating("animate", 0, .01667f);
	}

    public void determineScaler()
    {
        if(thing == "Table")
        {
            scaler = new Vector3(maxTableScaler * size, 0f, maxTableScaler * size);
        }
        else if(thing == "Left Wall" || thing == "Right Wall")
        {
            scaler = new Vector3(maxWallScaler * size, 0f, maxWallScaler * size);
        }
    }

    void animate()
    {
        if(Time.time - startTime < animLength && animating)
        {
            this.transform.localScale += scaler;
        }
        else
        {
            CancelInvoke("animate");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sphere(Clone)")
        {
            this.transform.position = new Vector3(transform.position.x, transform.position.y - collisionOffset, transform.position.z);
        }
        else if(other.gameObject.name == "Left Side" || other.gameObject.name == "Right Side" || other.gameObject.name == "Top Side" || other.gameObject.name == "Bottom Side")
        {
            animating = false;
        }
    }

    public void setThing(string thing)
    {
        this.thing = thing;
    }

    public void setSize(float size)
    {
        this.size = size;
    }
}
