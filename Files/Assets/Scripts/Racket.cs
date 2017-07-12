using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Racket : MonoBehaviour
{
    public float zDepth;
    public float maxPower;
    public float swingPercent;
    public Vector3 hitHelper;
    public float maxCurve;

    private Animator animator;
    private AnimatorStateInfo currAnim;

    private float curveRight;
    private float curveRightStart;
    private float curveLeft;
    private float curveLeftStart;

    private float hitPower;
    private float swingFactor;
    private float finalPower;

	// Use this for initialization
	void Start ()
    {
        Cursor.visible = false;
        animator = this.GetComponentInChildren<Animator>();
        hitPower = 0f;
        swingFactor = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zDepth;
        this.transform.position = Camera.main.ScreenToWorldPoint(mousePos);

        Curve();
        Swing();
    }

    private void Curve()
    {
        if (Input.GetKeyDown("a"))
        {
            curveLeft = 0.0f;
            curveLeftStart = Time.time;
        }

        if (Input.GetKeyDown("d"))
        {
            curveRight = 0.0f;
            curveRightStart = Time.time;
        }

        if (Input.GetKey("a"))
        {
            curveLeft = Time.time - curveLeftStart;
        }

        if (Input.GetKey("d"))
        {
            curveRight = Time.time - curveRightStart;
        }

        if (Input.GetKeyUp("a") || Input.GetKeyUp("d"))
        {
            curveLeft = 0f;
            curveRight = 0f;
        }
    }

    private void FixedUpdate()
    {
        currAnim = animator.GetCurrentAnimatorStateInfo(0);

        if (currAnim.IsName("Forehand Start") || currAnim.IsName("Backhand Swing"))
        {
            hitPower = currAnim.normalizedTime; // base the power of the hit on the wind up time of the racket
        }
        else if (currAnim.IsName("Forehand Swing") || currAnim.IsName("Backhand Start"))
        {
            swingFactor = currAnim.normalizedTime;
        }
        else
        {
            hitPower = 0f;
            swingFactor = 0f;
        }
    }

    public void HandleCollision(Collision col)
    {
        currAnim = animator.GetCurrentAnimatorStateInfo(0);

        if(hitPower > 1f)
        {
            hitPower = 1f;
        }

        if (swingFactor > 1f)
        {
            swingFactor = 1f;
        }

        finalPower = maxPower * (hitPower + ((1f - swingFactor) * swingPercent));

        if (currAnim.IsName("Forehand Swing") || currAnim.IsName("Backhand Swing"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(hitHelper);
            col.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0f, 0f, finalPower);
        }
        else if (currAnim.IsName("Forehand Start") || currAnim.IsName("Backhand Start"))
        {
            col.gameObject.GetComponent<Rigidbody>().velocity += new Vector3(0f, 0f, finalPower / 2f);
        }

        if(curveLeft > 2f)
        {
            curveLeft = maxCurve;
        }

        if(curveRight > 2f)
        {
            curveRight = maxCurve;
        }

        col.gameObject.GetComponent<Ball>().setCurve(curveRight, curveLeft);
    }

    private void Swing()
    {
        // 0 = no swing, 1 = forehand swing, 2 = backhand swing

        if ((Input.GetButton("Fire1") && animator.GetInteger("currSwing") == 1) || Input.GetButton("Fire1") && animator.GetInteger("currSwing") == 0)
        {
            animator.SetBool("forehandStart", true);
            animator.SetInteger("currSwing", 1);

             if (Input.GetButtonDown("Fire2"))
             {
                 animator.SetTrigger("forehandSwing");
                 animator.SetBool("forehandStart", false);
             }
        }
        else if (Input.GetButtonUp("Fire1") && animator.GetInteger("currSwing") == 1)
        {
            animator.SetBool("forehandStart", false);
            animator.SetTrigger("forehandDone");
            animator.SetInteger("currSwing", 0);
        }

        if ((Input.GetButton("Fire2") && animator.GetInteger("currSwing") == 2) || Input.GetButton("Fire2") && animator.GetInteger("currSwing") == 0)
        {
            animator.SetBool("backhandStart", true);
            animator.SetInteger("currSwing", 2);

            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("backhandSwing");
                animator.SetBool("backhandStart", false);
            }
        }
        else if (Input.GetButtonUp("Fire2") && animator.GetInteger("currSwing") == 2)
        {
            animator.SetBool("backhandStart", false);
            animator.SetTrigger("backhandDone");
            animator.SetInteger("currSwing", 0);
        }
    }

    private void resetState()
    {
        animator.SetInteger("currSwing", 0);

        animator.ResetTrigger("forehandDone");
        animator.SetBool("forehandStart", false);
        animator.ResetTrigger("forehandSwing");

        animator.ResetTrigger("backhandDone");
        animator.SetBool("backhandStart", false);
        animator.ResetTrigger("backhandSwing");
    }
}
