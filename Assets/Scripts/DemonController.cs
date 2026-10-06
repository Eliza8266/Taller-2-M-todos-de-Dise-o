using UnityEngine;

public class DemonController : MonoBehaviour
{
    Vector2 movement;
    public float enemySpeed;
    public Rigidbody2D enemyRb;
    public float detectionRadius = 0.5f;

    public Transform actualObjetive;

    public Transform[] enemyMovementPoints;
    public Animator enemyAnimator;

    [SerializeField] private float enemyDamage;
    [SerializeField] private float enemyStrength;

    public bool isFacingRight;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        actualObjetive = enemyMovementPoints[0];
        isFacingRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToObjetive = Vector2.Distance(transform.position, actualObjetive.position);

        if (distanceToObjetive < detectionRadius)
        {
            if (actualObjetive == enemyMovementPoints[0])
            {
                actualObjetive = enemyMovementPoints[1];
            }
            else if (actualObjetive == enemyMovementPoints[1])
            {
                actualObjetive = enemyMovementPoints[0];
            }
            
        }

        Vector2 direction = (actualObjetive.position - transform.position).normalized;

        int roundDirection = Mathf.RoundToInt(direction.x);

        movement = new Vector2(roundDirection, 0);

        if (roundDirection < 0 && isFacingRight)
        {
            Flip();
        }
        else if(roundDirection > 0 && !isFacingRight)
        {
            Flip();
        }

        enemyRb.MovePosition(enemyRb.position + movement * enemySpeed * Time.deltaTime);

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.TakeDamage(enemyDamage);
            player.hitTime = 0.5f;
            player.hitForce = enemyStrength;

            if(collision.transform.position.x <= transform.position.x)
            {
                player.hitFromRight = true;
            }
            else if (collision.transform.position.x > transform.position.x)
            {
                player.hitFromRight = false;
            }

        }
    }
}
