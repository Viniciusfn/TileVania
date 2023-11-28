using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 100f;

    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed * Time.deltaTime, 0);
    }

    private void OnTriggerExit2D(Collider2D other)        
    {
        // Flip enemy
        transform.localScale = new Vector2(-transform.localScale.x,
                                            transform.localScale.y);
        moveSpeed = -moveSpeed;
    }
}
