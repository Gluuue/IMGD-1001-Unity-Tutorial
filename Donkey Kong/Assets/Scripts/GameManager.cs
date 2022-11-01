using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int lives;
    private int score;

    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        //load level

    }

    public void LevelComplete()
    {
        score += 1000;

        //Load Next Level

    }

    public void LevelFail()
    {
        lives--;

        if (lives <= 0)
        {
            NewGame();
        } else
        {
            //Reload Current Level
        }
    }



}
