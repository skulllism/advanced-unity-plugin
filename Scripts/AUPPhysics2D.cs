using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUPPhysics2D
{
    public Rigidbody2D Rigidbody2D { private set; get; }
    public AUPRigidbody2DUtility Rigidbody2DController { private set; get; }
    public AUPMove2D Move2D { private set; get; }
    public AUPForward Forward { private set; get; }
    public Collider2D[] Colliders { get; private set; }


    public AUPPhysics2D(Rigidbody2D rigidbody2D, Transform transform, string motionColliderPath, eHorizontalDirection startDirection)
    {
        this.Rigidbody2D = rigidbody2D;
        Forward = new AUPForward();
        Forward.Init(transform, startDirection);

        Rigidbody2DController = new AUPRigidbody2DUtility(rigidbody2D, Forward);

        Colliders = GameObject.Instantiate(Resources.Load<GameObject>(motionColliderPath), transform, false).GetComponents<Collider2D>();

        Move2D = new AUPMove2D(rigidbody2D, Colliders[0]);
    }

    
}
