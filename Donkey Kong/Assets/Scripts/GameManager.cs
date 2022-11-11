using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int lives;
    public int score;
    public int highscore;
    private int level;
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        DontDestroyOnLoad(gameObject);
    }

    public void ButtonStart()
    {
        NewGame();
    }

    private void NewGame()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);

    }

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
            LoadLevel(1);
        }
        
    }

    public void LevelFail()
    {
        lives--;

        if (lives <= 0)
        {
            if(score > highscore)
            {
                highscore = score;
            }
            LoadLevel(0);
        } else
        {
            LoadLevel(level);
        }
    }



}
