using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public AudioClip[] racketHits;
    public AudioClip[] tableHits;
    public float explosionLength;
    public Vector3 scaleIncrease;

    private Text playerScore;
    private Text aiScore;
    private Text playerSet;
    private Text aiSet;

    private bool serving;

    private int playerSideHits;
    private int aiSideHits;
    private int lastPlayerHit;

    private GameObject ai;

    private bool curve;
    private float curveRight;
    private float curveLeft;

    private float lastHit;

    private const int PLAYER = 0;
    private const int AI = 1;

    private Rigidbody secondaryExplosion;

    private void Start()
    {
        ai = GameObject.Find("AI");
        curve = false;
        lastHit = 0.0f;

        lastPlayerHit = AI;
        playerSideHits = 0;
        aiSideHits = 0;

        serving = true;

        playerScore = GameObject.Find("UI/Score/Player").GetComponent<Text>();
        aiScore = GameObject.Find("UI/Score/Opponent").GetComponent<Text>();
        playerSet = GameObject.Find("UI/Set/Player").GetComponent<Text>();
        aiSet = GameObject.Find("UI/Set/Opponent").GetComponent<Text>();
    }

    private void Update()
    {
        if(curve)
        {
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.left * curveLeft, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * curveRight, ForceMode.Impulse);
        }

        if(int.Parse(playerScore.text) >= 13)
        {
            playerSet.text = (int.Parse(playerSet.text) + 1).ToString();
            resetScore();
        }
        else if (int.Parse(aiScore.text) >= 13)
        {
            aiSet.text = (int.Parse(aiSet.text) + 1).ToString();
            resetScore();
        }
    }

    private void resetScore()
    {
        playerScore.text = "0";
        aiScore.text = "0";
    }

    private void OnCollisionEnter(Collision col)
    {
        if((col.gameObject.name == "ID18" || col.gameObject.name == "Model" || col.gameObject.name == "Cube") && Time.time - lastHit > 0.1f)
        {
            lastHit = Time.time;
            AudioSource.PlayClipAtPoint(racketHits[Random.Range(0, 16)], col.transform.position);
            
            if(col.gameObject.name == "Cube")
            {
                if (aiSideHits == 0 && lastPlayerHit == PLAYER)
                {
                    aiScore.text = (int.Parse(aiScore.text) + 1).ToString();
                    ballDeath();
                }
                else
                {
                    lastPlayerHit = AI;
                    ResetHits();
                }
            }
            else
            {
                if(serving)
                {
                    serving = false;
                }

                if (playerSideHits == 0 && lastPlayerHit == AI)
                {
                    playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
                    ballDeath();
                }
                else
                {
                    lastPlayerHit = PLAYER;
                    ResetHits();
                }
            }
        }

        if(col.gameObject.name == "Table")
        {
            AudioSource.PlayClipAtPoint(tableHits[Random.Range(0, 17)], col.transform.position);

            curve = false;
            this.curveLeft = 0f;
            this.curveRight = 0f;
        }

        if(col.gameObject.name == "Floor")
        {
            if(aiSideHits >= 1 && lastPlayerHit == PLAYER)
            {
                playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
            }
            else if(aiSideHits == 0 && lastPlayerHit == PLAYER)
            {
                aiScore.text = (int.Parse(aiScore.text) + 1).ToString();
            }
            else if(playerSideHits >= 1 && lastPlayerHit == AI)
            {
                aiScore.text = (int.Parse(aiScore.text) + 1).ToString();
            }
            else if (playerSideHits == 0 && lastPlayerHit == AI)
            {
                playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
            }

            ballDeath();
        }
    }

    private void ResetHits()
    {
        playerSideHits = 0;
        aiSideHits = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player Side")
        {
            playerSideHits++;

            if (lastPlayerHit == PLAYER)
            {
                aiScore.text = (int.Parse(aiScore.text) + 1).ToString();
                ballDeath();
            }

            if (playerSideHits > 1 && lastPlayerHit == AI)
            {
                aiScore.text = (int.Parse(aiScore.text) + 1).ToString();
                ballDeath();
            }
        }
        else if (other.gameObject.name == "AI Side")
        {
            aiSideHits++;

            if (lastPlayerHit == AI && !serving)
            {
                playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
                ballDeath();
            }

            if (aiSideHits > 1 && lastPlayerHit == PLAYER)
            {
                playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
                ballDeath();
            }
        }
    }

    private void ballDeath()
    {
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        this.GetComponent<Collider>().enabled = false;

        secondaryExplosion = Instantiate(this.GetComponent<Rigidbody>(), this.transform.parent);
        secondaryExplosion.constraints = RigidbodyConstraints.FreezeAll;
        secondaryExplosion.GetComponent<Collider>().enabled = false;
        secondaryExplosion.transform.position = new Vector3(secondaryExplosion.transform.position.x, secondaryExplosion.transform.position.y, secondaryExplosion.transform.position.z - 25f);

        StartCoroutine(ballExplosion());
    }

    IEnumerator ballExplosion()
    {
        float startTime = Time.time;
        float time;
        
        while (true)
        {
            time = Time.time - startTime;
            this.GetComponent<Rigidbody>().transform.localScale += scaleIncrease;
            secondaryExplosion.transform.localScale += scaleIncrease / 2f;

            if (time > explosionLength)
            {
                Destroy(secondaryExplosion.gameObject);
                Destroy(this.gameObject);

                if (ai != null)
                {
                    ai.GetComponent<AI>().Serve();
                }

                yield break;
            }

            yield return null;
        }
    }

    public void setCurve(float curveRight, float curveLeft)
    {
        this.curve = true;
        this.curveLeft = curveLeft;
        this.curveRight = curveRight;
    }
}
