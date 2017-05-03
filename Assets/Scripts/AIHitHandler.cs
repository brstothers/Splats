using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHitHandler : MonoBehaviour
{
    AI ai;

    void Start()
    {
        ai = this.GetComponentInParent<AI>();
    }

    void OnCollisionEnter(Collision col)
    {
        ai.HandleCollision(col);
    }
}
