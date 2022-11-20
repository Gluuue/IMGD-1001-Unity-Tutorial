using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashUnlock : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject player = collision.gameObject;
            Player playerScript = player.GetComponent<Player>();

            if (playerScript)
            {
                playerScript.unlockDash();
            }
            Destroy(gameObject);
        }
    }
}
