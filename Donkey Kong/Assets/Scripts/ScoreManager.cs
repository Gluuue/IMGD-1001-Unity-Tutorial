
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int highscore;
    public static ScoreManager instance;


    private void Awake()
    {
        instance = this;
    }
        private void Start()
    {
        
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(1);
        //GameManager.instance.LoadLevelPub(1);
    }

    
    void Update()
    {
        
    }

    public void checkNewScore(int score)
    {
        if (highscore < score)
        {
            highscore = score;
        }

    }
}
