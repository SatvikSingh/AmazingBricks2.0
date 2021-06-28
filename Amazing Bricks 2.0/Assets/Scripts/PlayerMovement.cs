using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector3 force;
    private bool isDead = false;
    public PlayerMovement movement;
    private int score = 0;
    private Vector3 camPos;
    public GameObject gate;
    public GameObject obstacle;

    public float xRangeLow;
    public float xRangeHigh;

    public float yRangeLow;
    public float yRangeHigh;

    public AudioSource jumpSound;
    public AudioSource hitSound;

    public Animator camAnim;

    void Awake()
    {
        rb.isKinematic = true;
        float halfScreenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x - 2f;

        Vector3 curPos = Vector3.zero;
        Vector3 prevPos = Vector3.zero;
        Vector3 deltaPos = Vector3.up * 10f;

        for (int i = 1; i<100; i++)
        {
            curPos = prevPos + deltaPos;
            curPos = new Vector3(Random.Range(-halfScreenWidth, halfScreenWidth), curPos.y, 0);
            Instantiate(gate, curPos, Quaternion.identity);
            Instantiate(obstacle, new Vector3(curPos.x + Random.Range(xRangeLow, xRangeHigh), curPos.y + Random.Range(yRangeLow, yRangeHigh)), Quaternion.identity);
            Instantiate(obstacle, new Vector3(curPos.x + Random.Range(xRangeLow, xRangeHigh), curPos.y - Random.Range(yRangeLow, yRangeHigh)), Quaternion.identity);
            prevPos = new Vector3(0, curPos.y);
        }
    }

    void Update()
    {
        CameraFollow();

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        if (pos.x > Screen.width - 10 || pos.x < 10)
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = false;
            rb.AddForce(force, ForceMode2D.Impulse);
            jumpSound.Play();
            
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            rb.isKinematic = false;
            rb.AddForce(new Vector2(-force.x, force.y), ForceMode2D.Impulse);
            jumpSound.Play();
        }

        if (pos.y < 10)
        {
            Die();
        }

        if (isDead == true)
        {
            movement.enabled = false;
            hitSound.Play();
            camAnim.SetTrigger("shake");
        }
    }

    void Die()
    {
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = -Vector3.up * 10f;
        rb.AddTorque(10f);
    }

    void OnCollisionEnter2D (Collision2D other)
    {
        Die();
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        other.enabled = false;
        score++;
        Debug.Log(score);
        
    }

    void CameraFollow()
    {
        camPos = Camera.main.transform.position;

        if (transform.position.y > camPos.y)
        {
            Camera.main.transform.position += new Vector3(camPos.x, transform.position.y, camPos.z);
        }
    }
    
}
