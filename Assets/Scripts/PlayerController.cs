using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpforce = 700f;
    int jumpCount = 0;
    bool isGrounded = false;
    bool isDead = false;

    Rigidbody2D playerRigidBody;
    Animator animator;
    AudioSource playerAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
            return;

        if (Input.GetMouseButtonDown(0) && jumpCount < 2)
        {
            jumpCount++;
            playerRigidBody.velocity = Vector2.zero;
            playerRigidBody.AddForce(new Vector2(0, jumpforce));
            playerAudio.Play();
        }
        else if (Input.GetMouseButtonUp(0) && playerRigidBody.velocity.y > 0)
        {
            playerRigidBody.velocity = playerRigidBody.velocity * 0.5f;
        }

        animator.SetBool("Grouded", isGrounded);
    }

    void Die()
    {
        animator.SetTrigger("Die");
        playerAudio.clip = deathClip;
        playerAudio.Play();

        playerRigidBody.velocity = Vector2.zero;
        isDead = true;

        GameManager.instance.OnPlayerDead();
        GameManager.OnMessage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
