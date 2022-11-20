using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Collider2D[] results;
    private Vector2 direction;
    private bool grounded;
    private bool climbing;
    public float moveSpeed = 1f;
    public float jumpStrength = 1f;
    

    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite climbSprite;
    public Sprite jumpSprite;
    public Sprite dashSprite;
    private int spriteIndex;

    //Dash Variables
    public bool dashAvailable = false;
    public float numberOfDashes;
    private float maxNumOfDashes;
    private bool dashing;
    public float dashingPower;
    public float dashingDuration;
    public float dashCooldown;

    //Input Management
    private Vector2 input;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        results = new Collider2D[4];
        spriteRenderer = GetComponent<SpriteRenderer>();
        instance = this;
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(AnimateSprite), 1f / 6f, 1f / 6f);
    }

    private void OnDisable()
    {

    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (!grounded)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else if (dashing)
        {
            //spriteRenderer.sprite = jumpSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex++;

            if(spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = collider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        int amount = Physics2D.OverlapBoxNonAlloc(transform.position, size, 0f, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            if (hit.layer == LayerMask.NameToLayer("Ground"))
            {
                grounded = hit.transform.position.y < (transform.position.y - 0.5f);

                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hit.layer == LayerMask.NameToLayer("Ladder")) 
                {
            climbing = true;
                }
        }
    }

    private void Update()
    {
        //Input Managing 2.0
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        CheckCollision();

        //Dashing Lock
        if (dashing)
        {
            direction.y = Mathf.Max(direction.y, -1f);
            return;
        }

        //Climb Input
        if (climbing) 
            {
            direction.y = input.y * moveSpeed;
            }
            else if (grounded && Input.GetButtonDown("Jump"))
            {
                direction = Vector2.up * jumpStrength;
            }
            else
            {
                direction += Physics2D.gravity * Time.deltaTime;
            }

            //Dash Input
            if (Input.GetKeyDown(KeyCode.LeftShift) && numberOfDashes > 0f && input.x != 0 && dashAvailable)
            {
            StartCoroutine(Dash());
            }

        //Move PLayer based on horizontal input
        direction.x = input.x * moveSpeed;

        //Reset gravity on ground??
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        //Rotate Player to face right direction
        if (direction.x > 0f)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
 
    }

    private void FixedUpdate()
    {
        if (dashing)
        {
            return;
        }


        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Objective")) {

            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();

        } else if (collision.gameObject.CompareTag("Obstacle")) {

            enabled = false;
            FindObjectOfType<GameManager>().LevelFail();
        } else if (collision.gameObject.CompareTag("Upgrade"))
        {
   
        }
    }

    private IEnumerator Dash()
    {
        //set dash booleans to apropriate state
        dashAvailable = false;
        numberOfDashes--;
        dashing = true;

        //store and disable gravity for dash
        float originalGravity = rigidbody.gravityScale;
        rigidbody.gravityScale = 0f;
        
        //begin dashing momentup
        rigidbody.velocity = new Vector2(input.x * dashingPower, input.y * dashingPower);
        //Debug.Log("dash");

        //wait for dash to end
        yield return new WaitForSeconds(dashingDuration);
        
        //end dash
        rigidbody.gravityScale = originalGravity;
        dashing = false;
        dashAvailable = true;

        //wait for cooldown before new dash
        yield return new WaitForSeconds(dashCooldown);
        numberOfDashes++;
        
    }

    private void useDash()
    {
        return;
    }

    private void rechargeDash()
    {
        return;
    }

    //Dash Upgrade Methods
    public void unlockDash()
    {
        dashAvailable = true;
        if (maxNumOfDashes == 0)
        {
            maxNumOfDashes = 1;
            numberOfDashes = 1;
        }
        
    }

    public void upgradeDash()
    {
        if (maxNumOfDashes == 0)
        {
            maxNumOfDashes = 2;
            numberOfDashes = 2;
        }
        else
        {
            maxNumOfDashes++;
            numberOfDashes++;
        }
    }

    
}
