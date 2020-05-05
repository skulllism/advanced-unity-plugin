using UnityEngine;

public class AUPPhysics2D
{
    public Rigidbody2D Rigidbody2D { private set; get; }
    public AUPRigidbody2DUtility Rigidbody2DController { private set; get; }
    public AUPMove2D Move2D { private set; get; }
    public Collider2D[] Colliders { get; private set; }
    
    public AUPPhysics2D(Rigidbody2D rigidbody2D, Transform transform, string motionColliderPath)
    {
        Rigidbody2D = rigidbody2D;

        Rigidbody2DController = new AUPRigidbody2DUtility(rigidbody2D);

        Colliders = Object.Instantiate(Resources.Load<GameObject>(motionColliderPath), transform, false).GetComponents<Collider2D>();

        Move2D = new AUPMove2D(rigidbody2D);
    }
}
