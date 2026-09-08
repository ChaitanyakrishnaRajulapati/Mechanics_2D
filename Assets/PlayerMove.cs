using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public Vector2 moveInput;
    public Rigidbody2D rb;
    [Header("Movement And jumping")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private bool isJumping = false;
    private int facingDirection = 1;

    [Header("Dashing")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    private bool canDash = true;
    public bool isDashing;
    public float coolDownTime_D = 0.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isDashing == true)
        {
            return;
        }
        float moveX = Input.GetAxisRaw("Horizontal");
        moveInput = new Vector2(moveX, 0);
        if(moveX >.1 && facingDirection < 0)
        {
            Flip();
        }
        else if (moveX < -.1 && facingDirection > 0)
        {
            Flip();
        }
        if (Input.GetButtonDown("Jump") && !isJumping)   //Jumping 
        {
            rb.AddForce(new Vector2(moveInput.x, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
           if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate()
    {
        if (isDashing == true) //making movement false if dashing
        {
            return;
        }
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);  

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
    private void Flip()
    {
        facingDirection *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator Dash()  //Dare and Dashing
    {
        Debug.Log("DASH STARTED");

        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = new Vector2(facingDirection * dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(coolDownTime_D);

        canDash = true;

        Debug.Log("DASH READY");
    }


}
