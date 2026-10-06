using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float direction;
    public float speed;
    public Rigidbody2D rb;

    [SerializeField]
    private float jumpForce;
    public bool canJump;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    public Animator playerAnimator;
    public bool isFacingRight;

    public GameObject Cream;
    public GameObject Glass;
    public GameObject Donut;

    public float health;
    [SerializeField] private float maxHealth;

    public float hitForce;
    public float hitTime;
    public bool hitFromRight;

    public TextMeshProUGUI healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        healthText.text = $"Health: {health}/{maxHealth}";
    }

    // Update is called once per frame
    void Update()
    {
        canJump = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius,groundLayer);

        if (hitTime<=0)
        {
          rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocityY);
          playerAnimator.SetFloat("Direction", direction);
        }
        else
        {
            if (hitFromRight) //me pegaron por la derecha, voy para la izquierda
            {
                rb.AddForce(new Vector2(-hitForce, hitForce),ForceMode2D.Impulse);
            }
            else if (!hitFromRight) //me pegaron por la izquierda, voy para la derecha
            {
                rb.AddForce(new Vector2(hitForce, hitForce),ForceMode2D.Impulse); 
            }
            hitTime-= Time.deltaTime;
            
        }

        if (!isFacingRight && direction > 0f)
        {
            Flip();
        }
        // Si el jugador va hacia la izquierda pero está mirando a la derecha -> voltear
        else if (isFacingRight && direction < 0f)
        {
            Flip();
        }

    }

    public void Move(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>().x;
    }
    public void Jump (InputAction.CallbackContext context)
    {
        if(context.performed && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Cream"))
        {
            collision.gameObject.SetActive(false);
            Glass.SetActive(false);

        }
        if ( collision.gameObject.CompareTag("Donut"))
        { 
            collision.gameObject.SetActive(false);
        }
        

    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthText.text = $"Health: {health}/{maxHealth}";
    }

    public void AddHealth(float _health)
    {
        if (health + _health > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += _health;
        }
        healthText.text = $"Health: {health}/{maxHealth}";
    }
}
