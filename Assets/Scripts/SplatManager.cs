using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatManager : MonoBehaviour
{
    public Material[] materials;
    public float magnitudeDivisor;
    public float wallOffset;
    public float maxWallSplats;

    private Transform leftWall;
    private Transform rightWall;
    private GameObject splat;
    private float size;

    private const float tabletop = 30f;

    private void Start()
    {
        leftWall = GameObject.Find("Walls/Left Wall Splats").transform;
        rightWall = GameObject.Find("Walls/Right Wall Splats").transform;
    }

    public void GenerateSplat(string thing, Vector3 position, Vector3 velocity)
    {
        size = (velocity.magnitude / magnitudeDivisor);

        splat = Instantiate(Resources.Load("Splat") as GameObject, setupPosition(thing, position), Quaternion.identity);
        splat.GetComponent<Splat>().setThing(thing);
        splat.GetComponent<Splat>().setSize(size);
        splat.GetComponent<Renderer>().material = materials[Random.Range(0, 5)];
        splat.transform.parent = setupParent(thing);

        if (thing == "Right Wall")
        {
            splat.transform.localRotation *= Quaternion.Euler(90f, 90f, 0f);

            if (rightWall.childCount > maxWallSplats)
            {
                Destroy(rightWall.GetChild(0).gameObject);
            }
        }
        else if (thing == "Left Wall")
        {
            splat.transform.localRotation *= Quaternion.Euler(90f, 90f, 0f);

            if (leftWall.childCount > maxWallSplats)
            {
                Destroy(leftWall.GetChild(0).gameObject);
            }
        }
    }

    private Transform setupParent(string thing)
    {
        Transform transform = null;

        if(thing == "Table")
        {
            transform = this.transform;
        }
        else if(thing == "Left Wall")
        {
            transform = leftWall.transform;
        }
        else if(thing == "Right Wall")
        {
            transform = rightWall.transform;
        }

        return transform;
    }

    private Vector3 setupPosition(string thing, Vector3 position)
    {
        Vector3 finalPos;

        if(thing == "Table")
        {
            position.y = tabletop;
            finalPos = position;
        }
        else if(thing == "Left Wall")
        {
            position.x += wallOffset;
            finalPos = position;
        }
        else if (thing == "Right Wall")
        {
            position.x -= wallOffset;
            finalPos = position;
        }

        return position;
    }
}
