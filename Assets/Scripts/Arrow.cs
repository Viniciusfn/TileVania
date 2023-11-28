using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 100f;

    float xSpeed;

    Rigidbody2D myRigidbody;
    PlayerMovement player;
    bool wasUsed = false;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
        transform.localScale = new Vector3(player.transform.localScale.x,1f,1f);
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(xSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy" && !wasUsed) // Enemy hit
        {
            Destroy(other.gameObject);
            wasUsed = true;
            FindObjectOfType<GameSession>().UpdateScore("Enemy");
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}