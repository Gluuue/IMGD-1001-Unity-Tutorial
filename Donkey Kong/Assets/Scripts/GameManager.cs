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
    public Player player;

    //Audio Fields
    GameObject soundEffectsSource;
    public AudioClip buttonSFX;



    private void Awake()
    {
        //count = 0;
        
        
        DontDestroyOnLoad(gameObject);
        //GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayMusic();
      
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

    private void Update() {
        soundEffectsSource = GameObject.FindGameObjectWithTag("Sound");
    }


    public void ButtonStart()
    {
        if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(buttonSFX);
        }
        NewGame();
    }

    public void ButtonTest()
    {
        lives = 3;
        score = 0;

        LoadLevel(1);
    }

    public void ButtonQuit() {
        if (soundEffectsSource != null) {
                soundEffectsSource.GetComponent<AudioSource>().PlayOneShot(buttonSFX);
        }
        Application.Quit();
    }

    private void NewGame()
    {
        GameManager.instance.lives = 3;
        GameManager.instance.score = 0;

        LoadLevel(startLevel);

    }

    public void LoadFirstLevel() {

        level = 15;
        LoadScene();
        Instantiate(this.player, new Vector3(-14, -7, 0), Quaternion.identity);

    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;

        if (camera != null) { 
            camera.cullingMask = 0;
        }

        LoadScene();
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

    public void LevelFail(int level, Vector2 position, Player player)
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

            Destroy(GameObject.FindGameObjectWithTag("Player"));

            //Return to main menu
            LoadLevel(2);
        } else
        {
            //LoadLevel(startLevel);
            SceneSwitcher.ReloadScene(level, position, player);
        }
    }



}
