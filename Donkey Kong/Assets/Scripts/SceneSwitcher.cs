using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitcher : MonoBehaviour
{
   public int SceneNumber;
   public int xValue;
   public int yValue;
   private void OnCollisionEnter2D(Collision2D collision) {
          SceneManager.LoadScene(SceneNumber);
          GameObject player = collision.gameObject;
          player.transform.position = new Vector2(xValue,yValue);

          Player.updateCurrentPosition(xValue, yValue);
    }

    public static void ReloadScene(int level, Vector2 position, Player player) {
        SceneManager.LoadScene(level);
        player.transform.position = position;
    }

    
}


