using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
   public float debrisLength;

    public SpriteRenderer sprite;
    public BoxCollider2D bc;

    public ParticleSystem ps;

    private bool intact;

    private void Awake() {

        intact = true;
    }

   

    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log("Collision");  
        var emission = ps.emission;
        if (collision.tag == "Player" && Player.instance.checkIfDashing() && intact) {
            //Debug.Log("Player Dash Collision");
            bc.enabled = false;
            sprite.enabled = false;
            intact = false;
            StartCoroutine(Debris(collision));


        }

        
    }

    private IEnumerator Debris(Collider2D collision) {
        var emission = ps.emission;
        var sh = ps.shape;
        emission.enabled = true;
        if (Player.instance.inputDirection() > 0) {
            sh.rotation = new Vector3(0, 0, 225);
        }
        else if (Player.instance.inputDirection() < 0) {
            sh.rotation = new Vector3(0, 0, 45);
        }
        
        yield return new WaitForSeconds(debrisLength);
        emission.enabled = false;
    }
}
