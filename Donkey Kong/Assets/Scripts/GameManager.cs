using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives;
    public int score;
    public int highscore;
    private int level;
    private int count;
    public static GameManager instance;

    private void Awake()
    {
        //count = 0;
        
        highscore = 0;
        DontDestroyOnLoad(gameObject);
      
    }

    private void Start()
    {
              
        if (instance != null)
        {
            DestroyObject(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(3);

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
        score += 1000;

        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel);
        } else
        {
            LoadLevel(3);
        }
        
    }

    public void LevelFail()
    {
        lives--;

        if (lives <= 0)
        {
            lives = 3;
            ScoreManager.instance.checkNewScore(score);
            score = 0;
            LoadLevel(2);
        } else
        {
            LoadLevel(level);
        }
    }



}
