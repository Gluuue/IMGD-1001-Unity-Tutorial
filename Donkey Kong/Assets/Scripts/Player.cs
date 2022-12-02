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

    //New
    //Barrel Jump Variables
    public bool barrelJumpAvailable = false;
    public static bool holdingBarrel = false;
    public float barrelJumpModifier = 2.0f;
    
    //Wall Climb Variables
    private bool onWall;


    //End New


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
        else if (!grounded  && !onWall)
        {
            spriteRenderer.sprite = jumpSprite;
        }
        else if (dashing)
        {
            //enable this line of code once we implement a dash sprite
            //spriteRenderer.sprite = dashSprite;
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

    private void CheckCollision() {

        onWall = false;
        grounded = false;
        climbing = false;

        //Grounded collider size & position (A small rectangle centered vertically on the bottommost pixel of the player collider, width equal to player, extending a little below the player)
        Vector2 playerBottom = new Vector2 (transform.position.x, transform.position.y - 0.5f);
        Vector2 sizeGrounded = collider.bounds.size;
        sizeGrounded.y -= 0.8f;
        Collider2D isGround = Physics2D.OverlapBox(playerBottom, sizeGrounded, 0f, LayerMask.GetMask("Ground"));

        //If the player is in contact with at least one object with a "ground" layer on the bottom
        if (isGround != null) {
            Debug.Log("Touching ground");
            grounded = isGround.transform.position.y <= (transform.position.y - 0.5f);
        }

        //Ceiling collider size & position (A small rectangle centered vertically on the topmost pixel of the player collider, width equal to player, extending a little above the player)
        Vector2 playerTop = new Vector2 (transform.position.x, transform.position.y + 0.5f);
        Vector2 sizeCeiling = collider.bounds.size;
        sizeCeiling.y -= 0.8f;
        Collider2D isCeiling = Physics2D.OverlapBox(playerTop, sizeCeiling, 0f, LayerMask.GetMask("Ground"));

        //If the player is in contact with at least one object with a "ground" layer on the top
        if (isCeiling != null) {
            Debug.Log("Touching Ceiling");
            Physics2D.IgnoreCollision(collider, isCeiling, !grounded); 
        }

        //Ladder collider size & position (A small rectangle centered on the player collider, width half of player, extends vertically matching collider)
        Vector2 playerCenter = transform.position;
        Vector2 sizeLadder = collider.bounds.size;
        sizeLadder.x /= 2.0f;
        sizeLadder.y += 0.1f;
        Collider2D isLadder = Physics2D.OverlapBox(playerCenter, sizeLadder, 0f, LayerMask.GetMask("Ladder"));

        //If the player is in contact with at least one object with a "Ladder" layer
        if (isLadder != null) {
            Debug.Log("Touching Ladder");
            climbing = true; 
        }

        //Wall collider size & position (A small rectangle centered on the player collider, width slightly wider than player, height about half of player)
        Vector2 playerSides = new Vector2 (transform.position.x, transform.position.y + 0.5f);
        Vector2 sizeWall = collider.bounds.size;
        sizeWall.x += 0.1f;
        sizeCeiling.y /= 2.0f;
        Collider2D isWall = Physics2D.OverlapBox(playerSides, sizeWall, 0f, LayerMask.GetMask("Wall"));

        //If the player is in contact with at least one object with a "ground" layer on the top
        if (isWall != null && Input.GetKey(KeyCode.L)) {
            Debug.Log("Touching Wall");
            climbing = true; 
        }

    }

    /*
    private void CheckCollision()
    {
        onWall = false;
        grounded = false;
        climbing = false;

        //Grounded collider size
        Vector2 sizeGrounded = collider.bounds.size;
        sizeGrounded.y += 0.1f;
        sizeGrounded.x += 0.1f;

        //WallClimb collider size
        Vector2 sizeWallClimb = collider.bounds.size;
        sizeWallClimb.y /= 2f;
        sizeWallClimb.x += 0.1f;

        int amountGrounded = Physics2D.OverlapBoxNonAlloc(transform.position, sizeGrounded, 0f, results);
        int amountWallClimb = Physics2D.OverlapBoxNonAlloc(transform.position, sizeWallClimb, 0f, results);
        //Debug.Log("Grounded:" + amountGrounded);
        //Debug.Log("Wall:" + amountWallClimb);

        Physics2D.OverlapBox();

        //Check grounded-type collisions
        for (int i = 0; i < amountGrounded; i++){
            GameObject hitGround = results[i].gameObject;
            Debug.Log(hitGround.layer);
            Debug.Log(hitGround);

            
            if (hitGround.layer == LayerMask.NameToLayer("Ground")) {

                //grounded if colliding with ground on lower half of character
                Debug.Log(hitGround.transform.position.y);
                Debug.Log(transform.position.y - 0.5f);
                grounded = hitGround.transform.position.y <= (transform.position.y - 0.5f);
                //Debug.Log("Grounded, grounded");

                Physics2D.IgnoreCollision(collider, results[i], !grounded);
            }
            else if (hitGround.layer == LayerMask.NameToLayer("Ladder")) {
                //Debug.Log("Oh, a ladder!");
                //if (Input.GetKey(KeyCode.W)) {
                climbing = true;
                //}
            } 
            else if (hitGround.layer == LayerMask.NameToLayer("Wall")) {
                onWall = true;
                if (Input.GetKey(KeyCode.L)) {
                    climbing = true;
                }

            }
            
        }

    }
    */

    private void Update()
    {

        Debug.Log("Grounded" + grounded);
        //Debug.Log("onWall" + onWall);
        //Debug.Log("climbing" + climbing);

        //Input Managing 2.0
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        CheckCollision();

        //Dashing Lock
        if (dashing)
        {
            direction.y = Mathf.Max(direction.y, -1f);
            return;
        }

        //Climb Input / Jump / Fall
        if (climbing) 
        {
        direction.y = input.y * moveSpeed;
        }
        else if (grounded && Input.GetButtonDown(buttonName: "Jump"))
        {
            direction = Vector2.up * jumpStrength;
        }
        else if (grounded && Input.GetKeyDown(KeyCode.C) && holdingBarrel)
        {
            direction = Vector2.up * jumpStrength * barrelJumpModifier;
            holdingBarrel = false;
        }
        //Default gravity
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
            
            //New
                //Input.GetKey(KeyCode.P)
            if (!holdingBarrel && barrelJumpAvailable) {
                holdingBarrel = true;
                Destroy(collision.gameObject);
            } else {
                enabled = false;
                FindObjectOfType<GameManager>().LevelFail();
            }
            //End New

            //Commented out for new code
            //enabled = false;
            //FindObjectOfType<GameManager>().LevelFail();
        } 
        
        else if (collision.gameObject.CompareTag("Upgrade"))
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

    //Barrel Jump Upgrade Methods
    public void unlockBarrelJump()
    {
        barrelJumpAvailable = true; 
    }






}
