using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives;
    public int startLevel;
    public int startLives;
    public int score;
    public int highscore;
    private int level;
    private int count;
    public static GameManager instance;

    private void Awake()
    {
        //count = 0;
        
        
        DontDestroyOnLoad(gameObject);
      
    }

    private void Start()
    {
        //GameManager.instance.highscore = 0;
        

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            GameManager.instance.highscore = 0;
        }
        
        //DontDestroyOnLoad(gameObject);
        //LoadLevel(1);
    }

    public void ButtonStart()
    {
        NewGame();
    }

    public void ButtonTest()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    public void ButtonQuit() {
        Application.Quit();
    }

    private void NewGame()
    {
        GameManager.instance.lives = 3;
        GameManager.instance.score = 0;

        LoadLevel(startLevel);

    }

    /*
    public void LoadLevelPub(int index)
    {
        LoadLevel(index);
    }
    */

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;

        if (camera != null) { 
            camera.cullingMask = 0;
        }

        Invoke(nameof(LoadScene), 1f); 
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(level);
    }

    public void LevelComplete()
    {
        //Points for completion
        score += 1000;

        //Points for extra lives
        for(int i = lives; i > 0; i--) {
            score += 500;
        }

        //Check Highscore
        ScoreManager.instance.checkNewScore(score);
            if(highscore < score) {
                highscore = score;
            }
            score = 0;

        
        //Return to main menu
        LoadLevel(2);  


        //old level loop system
        /*
        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        } else
        {
            LoadLevel(3);
        }
        */
        //end
        
    }

    public void LevelFail()
    {
        lives = lives - 1;

        if (lives <= 0)
        {
            lives = 0;
            ScoreManager.instance.checkNewScore(score);
            if(highscore < score) {
                highscore = score;
            }
            score = 0;
            //Return to main menu
            LoadLevel(2);
        } else
        {
            LoadLevel(startLevel);
        }
    }



}
