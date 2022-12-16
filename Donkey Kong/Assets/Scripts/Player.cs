using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance;

    private new Rigidbody2D rigidbody;
    private new Collider2D collider;
    private Collider2D[] results;
    private Vector2 direction;
    private bool grounded;
    private bool climbing;
    public float moveSpeedDefault = 3.0f;
    private float moveSpeedModified;
    public float jumpStrength = 1f;

    //Player Respawn Fields
    private int currentLevel;
    private static Vector2 currentPos; 
    

    private SpriteRenderer spriteRenderer;
    public Sprite[] runSprites;
    public Sprite[] jumpSprites;
    public Sprite[] ladderSprites;
    public Sprite[] dashSprites;
    public Sprite[] wallClimbSprites;
    public Sprite fallSprite;
    public Sprite idleSprite;
    private int spriteIndex;

    //Dash Fields
    public bool dashAvailable = false;
    public float numberOfDashes;
    private float maxNumOfDashes;
    private bool dashing;
    public float dashingPower;
    public float dashingDuration;
    public float dashCooldown;
    //public Vector2 dashDirection;

    //Barrel Jump Fields
    public bool barrelJumpAvailable = false;
    public static bool holdingBarrel = false;
    public float barrelJumpModifier = 2.0f;
    
    //Wall Climb Fields
    private bool onLeftWall;
    private bool onRightWall;

    //Mud Fields
    private bool muddy;
    
    //Audio Fields
    GameObject soundEffectsSource;
    public AudioClip jumpSFX;
    public AudioClip dashSFX;
    public AudioClip barrelJumpSFX;
    public AudioClip deathSFX;




    //Input Management
    private Vector2 input;

    private void Awake()
    {
        DontDestroyOnLoad(this);    
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        //Should no longer need this array
        //results = new Collider2D[4];
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
        if (climbing && !onLeftWall && !onRightWall)
        {
            //Debug.Log("ladder anim");
            spriteIndex++;

            if(spriteIndex >= ladderSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = ladderSprites[spriteIndex];
        } else if (onLeftWall) {
            //Debug.Log("left anim");
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
            spriteIndex++;

            if(spriteIndex >= wallClimbSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = wallClimbSprites[spriteIndex];
        } else if (onRightWall) {
            //Debug.Log("right anim");
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            spriteIndex++;

            if(spriteIndex >= wallClimbSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = wallClimbSprites[spriteIndex];
        }
        else if (dashing)
        {
            spriteIndex++;

            if(spriteIndex >= dashSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = dashSprites[spriteIndex];

        } //Falling
        else if (!grounded  && !onLeftWall && !onRightWall)
        {
            //Debug.Log("falling anim");
            spriteRenderer.sprite = fallSprite;
        } //Moving
        else if (direction.x != 0f)
        {
            spriteIndex++;

            if(spriteIndex >= runSprites.Length)
            {
                spriteIndex = 0;
            }

            spriteRenderer.sprite = runSprites[spriteIndex];

        }
        //If not moving and not using any other sprite, go to default sprite
        else if (direction.x == 0f) {
            //Debug.Log("idle anim");
            spriteRenderer.sprite = idleSprite;
        }
    }

    private void CheckCollision() {

        onLeftWall = false;
        onRightWall = false;
        grounded = false;
        climbing = false;
        muddy = false;

        //Grounded collider size & position (A small rectangle centered vertically on the bottommost pixel of the player collider, width equal to player, extending a little below the player)
        Vector2 playerBottom = new Vector2 (transform.position.x, transform.position.y - 0.5f);
        Vector2 sizeGrounded = collider.bounds.size;
        sizeGrounded.y -= 0.8f;
        Collider2D isGround = Physics2D.OverlapBox(playerBottom, sizeGrounded, 0f, LayerMask.GetMask("Ground", "Mud"));

        //If the player is in contact with at least one object with a "ground" layer on the bottom
        if (isGround != null) {
            //Debug.Log("Touching ground");
            //isGround.transform.position.y <= (transform.position.y - 0.5f)
            grounded = true;
        }

        
        //Ceiling collider size & position (A small rectangle centered vertically on the topmost pixel of the player collider, width equal to player, extending a little above the player)
        Vector2 playerTop = new Vector2 (transform.position.x, transform.position.y + 0.5f);
        Vector2 sizeCeiling = collider.bounds.size;
        sizeCeiling.y -= 0.8f;
        Collider2D isCeiling = Physics2D.OverlapBox(playerTop, sizeCeiling, 0f, LayerMask.GetMask("Ground"));


        /*
        //If the player is in contact with at least one object with a "ground" layer on the top
        if (isCeiling != null) {
            //Debug.Log("Touching Ceiling");

        }
        */

        //Ladder collider size & position (A small rectangle centered on the player collider, width half of player, extends vertically matching collider)
        Vector2 playerCenter = transform.position;
        Vector2 sizeLadder = collider.bounds.size;
        sizeLadder.x /= 2.0f;
        sizeLadder.y += 0.1f;
        Collider2D isLadder = Physics2D.OverlapBox(playerCenter, sizeLadder, 0f, LayerMask.GetMask("Ladder"));

        //If the player is in contact with at least one object with a "Ladder" layer
        if (isLadder != null) {
            //Debug.Log("Touching Ladder");
            climbing = true; 
        }


        //Wall collider size & position (A small rectangle centered on the player collider, width slightly wider than player, height about half of player)
        Vector2 playerLeft = new Vector2 (transform.position.x - 0.5f, transform.position.y);
        Vector2 playerRight = new Vector2 (transform.position.x + 0.5f, transform.position.y);
        Vector2 sizeWall = collider.bounds.size;
        sizeWall.x -= 0.8f;
        sizeWall.y -= 0.05f;
        Collider2D isWallLeft = Physics2D.OverlapBox(playerLeft, sizeWall, 0f, LayerMask.GetMask("Wall"));
        Collider2D isWallRight = Physics2D.OverlapBox(playerRight, sizeWall, 0f, LayerMask.GetMask("Wall"));

        //Check all four directions for mud
        Collider2D isMuddyBottom = Physics2D.OverlapBox(playerBottom, sizeGrounded, 0f, LayerMask.GetMask("Mud"));
        Collider2D isMuddyTop = Physics2D.OverlapBox(playerTop, sizeGrounded, 0f, LayerMask.GetMask("Mud"));
        Collider2D isMuddyLeft = Physics2D.OverlapBox(playerLeft, sizeGrounded, 0f, LayerMask.GetMask("Mud"));
        Collider2D isMuddyRight = Physics2D.OverlapBox(playerRight, sizeGrounded, 0f, LayerMask.GetMask("Mud"));

        if (isMuddyBottom != null || isMuddyTop != null || isMuddyLeft != null || isMuddyRight != null) {
            //Debug.Log("Touching mud");
            muddy = true;
            moveSpeedModified = moveSpeedDefault * 0.7f;
        } else {
            moveSpeedModified = moveSpeedDefault;
        }

        //If the player is in contact with at least one object with a "ground" layer on the top
        if (isWallLeft != null && !isMuddyLeft && Input.GetKey(KeyCode.LeftControl)) {
            //Debug.Log("Touching Wall");
            climbing = true;
            onLeftWall = true;
        } else if (isWallRight != null && !isMuddyRight && Input.GetKey(KeyCode.LeftControl)) {
            climbing = true;
            onRightWall = true;
        }

        
        //Chcecking only the top/sides for spikes. Don't want the bottom to kill players.
        Collider2D spikesBottom = Physics2D.OverlapBox(playerBottom, sizeGrounded, 0f, LayerMask.GetMask("Spikes"));
         if (spikesBottom != null) {
            if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(deathSFX);
            }
            //enabled = false;
            FindObjectOfType<GameManager>().LevelFail(currentLevel, currentPos, instance);
         }

        Collider2D iswaterBottom = Physics2D.OverlapBox(playerBottom, sizeGrounded, 0f, LayerMask.GetMask("Water"));
        Collider2D iswaterTop = Physics2D.OverlapBox(playerTop, sizeGrounded, 0f, LayerMask.GetMask("Water"));
        Collider2D iswaterLeft = Physics2D.OverlapBox(playerLeft, sizeGrounded, 0f, LayerMask.GetMask("Water"));
        Collider2D iswaterRight = Physics2D.OverlapBox(playerRight, sizeGrounded, 0f, LayerMask.GetMask("Water"));

        if (iswaterBottom != null || iswaterTop != null || iswaterLeft != null || iswaterLeft != null) {
            if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(deathSFX);
            }
            //enabled = false;
            FindObjectOfType<GameManager>().LevelFail(currentLevel, currentPos, instance);
        }

    }


    private void Update()
    {

        soundEffectsSource = GameObject.FindGameObjectWithTag("Sound");
        currentLevel = SceneManager.GetActiveScene().buildIndex;

        //Debug.Log("Grounded" + grounded);
        //Debug.Log("onLeftWall" + onLeftWall);
        //Debug.Log("onRightWall" + onRightWall);
        //Debug.Log("climbing" + climbing);
        //Debug.Log("Mud" + muddy);

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
        direction.y = input.y * moveSpeedModified;
        }
        else if (grounded && Input.GetButtonDown(buttonName: "Jump") && !muddy)
        {
            if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(jumpSFX);
            }

            direction = Vector2.up * jumpStrength;
            StartCoroutine(animateJump());
        }
        //grounded && (Removed this code so barrel jump can occur in air)
        else if (Input.GetKeyDown(KeyCode.C) && holdingBarrel)
        {
            if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(barrelJumpSFX);
            }
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
            if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(dashSFX);
            }
            StartCoroutine(Dash());
        }

        //Move PLayer based on horizontal input
        direction.x = input.x * moveSpeedModified;

        //Reset gravity on ground??
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        //Rotate Player to face right direction
        if (direction.x < 0f && !climbing)
        {
            transform.eulerAngles = Vector3.zero;
        }
        else if (direction.x > 0f && !climbing)
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

        //Debug.Log(direction);

        
        rigidbody.MovePosition(rigidbody.position + direction * Time.fixedDeltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Objective")) {

            enabled = false;
            FindObjectOfType<GameManager>().LevelComplete();

        } else if (collision.gameObject.CompareTag("Obstacle")) {
            
            
            if (!holdingBarrel && barrelJumpAvailable) {
                holdingBarrel = true;
                Destroy(collision.gameObject);
            } else {
                //enabled = false;
                if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(deathSFX);
                }
                FindObjectOfType<GameManager>().LevelFail(currentLevel, currentPos, instance);
            }
        } else if (collision.gameObject.CompareTag("Upgrade")) {

        } 
        /*
        else if (collision.gameObject.CompareTag("Projectile")) {
            enabled = false;
            FindObjectOfType<GameManager>().LevelFail();
        }
        */
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.CompareTag("Projectile")) {
            //enabled = false;
            if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(deathSFX);
            }
            FindObjectOfType<GameManager>().LevelFail(currentLevel, currentPos, instance);
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

    private IEnumerator animateJump() {

        for (int i = 0; i < jumpSprites.Length; i++) {
                spriteRenderer.sprite = jumpSprites[i];
                yield return new WaitForSeconds(0.1f);
        }

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

    public bool checkIfDashing() {
        return dashing;
    }

    public bool checkIfGrounded() {
        return grounded;
    }

    public float inputDirection() {
        return direction.x;
    }

    public static void updateCurrentPosition(float x, float y) {
        currentPos = new Vector2(x, y);
    }


}
