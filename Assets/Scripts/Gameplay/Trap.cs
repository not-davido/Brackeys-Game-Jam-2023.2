using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public Transform positionAfterHit;

    [SerializeField] protected float knockBackForce = 15;

    protected virtual void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.TryGetComponent(out Health health)) {
            if (health.TakeDamage(1, null, positionAfterHit)) {
                if (collision.collider.attachedRigidbody != null) {
                    var collisionNormal = collision.GetContact(0).normal;

                    if (collisionNormal.y == 0) {
                        collisionNormal.y = -1;
                    }

                    var playerDirection = collision.collider.transform.right;

                    var knockbackDirection = new Vector2(-playerDirection.x, -collisionNormal.y).normalized;
                    print(knockbackDirection);

                    collision.collider.attachedRigidbody.AddForce(knockbackDirection * knockBackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
