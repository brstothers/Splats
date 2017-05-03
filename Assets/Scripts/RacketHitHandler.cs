using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketHitHandler : MonoBehaviour
{
    Racket racket;

	void Start ()
    {
        racket = this.GetComponentInParent<Racket>();
	}

    void OnCollisionEnter(Collision col)
    {
        racket.HandleCollision(col);
    }
}
