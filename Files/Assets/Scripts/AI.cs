using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    public GameObject ball;

    public float serveSpeed;
    public Vector3 servePosition;

    public float minXPos;
    public float maxXPos;

    public float minTableEdge;
    public float maxTableEdge;

    public float minCurve;
    public float maxCurve;

    public float minHitPower;
    public float maxHitPower;
    public Vector3 hitHelper;

    private float serveCurve;
    private Rigidbody ballBody;

    void Start()
    {
        Serve();
    }

	void Update()
    {
        if(ballBody != null)
        {
            transform.position = new Vector3(Mathf.Clamp(ballBody.transform.position.x, minXPos, maxXPos), transform.position.y, transform.position.z);
        }
    }

    public void Serve()
    {
        StartCoroutine(MoveToServe());
    }

    IEnumerator MoveToServe()
    {
        Vector3 start = transform.position;
        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(start, servePosition, time);

            if(transform.position == servePosition)
            {
                ballBody = Instantiate(ball.GetComponentInChildren<Rigidbody>(), this.transform.position, Quaternion.identity);
                ballBody.velocity += new Vector3(0f, 0f, serveSpeed);
                yield break;
            }

            yield return null;
        }
    }

    public void HandleCollision(Collision col)
    {
        Ball ball = ballBody.GetComponent<Ball>();
        col.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, Random.Range(minHitPower, maxHitPower));
        col.gameObject.GetComponent<Rigidbody>().AddForce(hitHelper);

        if(ball.transform.position.x < minTableEdge)
        {
            ball.setCurve(Random.Range(0.25f, maxCurve), 0f);
        }
        else if(ball.transform.position.x > maxTableEdge)
        {
            ball.setCurve(0f, Random.Range(0.25f, maxCurve));
        }
        else
        {
            float rand = Random.Range(0f, 1f);

            if (rand > 0.6f)
            {
                rand = Random.Range(0f, 1f);

                if (rand < 0.5f)
                {
                    ball.setCurve(0f, Random.Range(minCurve, maxCurve + 1f));
                }
                else
                {
                    ball.setCurve(Random.Range(minCurve, maxCurve + 1f), 0f);
                }
            }
        }
    }
}
