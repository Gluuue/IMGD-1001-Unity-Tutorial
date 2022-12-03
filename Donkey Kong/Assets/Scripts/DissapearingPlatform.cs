using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearingPlatform : MonoBehaviour
{
    
    public float disintegrateTime;
    
    public bool fall;
    public float fallDelay;
    //public float fallSpeed;
    public float gravityEffect;

    private SpriteRenderer spriteRenderer;
    public Sprite[] fallSprites;
    public Sprite[] breakSprites;
    private int spriteIndex;
    private bool breaking;
    private bool falling;

    public ParticleSystem ps;


    
    [SerializeField] private Rigidbody2D rb;


    private void OnTriggerEnter2D(Collider2D collision){


        if (collision.tag == "Player" && Player.instance.checkIfGrounded()) {
            Debug.Log("Player Land");
            StartCoroutine(Disintegrate());
        }
    }

    private void AnimateSprite() 
    {
        if (falling) 
        {
            spriteIndex++;

            if(spriteIndex >= fallSprites.Length)
            {
                //spriteIndex = 0;
                spriteIndex = fallSprites.Length;
            }

            spriteRenderer.sprite = fallSprites[spriteIndex];
        }
        else if (breaking)
        {
            spriteIndex++;

            if(spriteIndex >= breakSprites.Length)
            {
                //spriteIndex = 0;
                spriteIndex = breakSprites.Length;
            }

            spriteRenderer.sprite = breakSprites[spriteIndex];
        }

    }

    private IEnumerator Disintegrate() {
        Debug.Log("Disintegrate");
        
        var emission = ps.emission;
        

        if (fall) {
            emission.enabled = true;
            yield return new WaitForSeconds(fallDelay);
            rb.gravityScale = gravityEffect;
            rb.bodyType = RigidbodyType2D.Dynamic;
            falling = true;
            emission.enabled = false;
        }
        else if (!fall) {
            emission.enabled = true;
            breaking = true;
        }
        
        
        yield return new WaitForSeconds(disintegrateTime);
        emission.enabled = false;
        
        Destroy(gameObject);
    }



}
