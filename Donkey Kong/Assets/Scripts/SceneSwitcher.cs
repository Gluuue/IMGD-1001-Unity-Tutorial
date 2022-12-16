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
    }
}


