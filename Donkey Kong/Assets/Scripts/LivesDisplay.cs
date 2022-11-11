using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LivesDisplay : MonoBehaviour
{
    //Objects
    public GameObject textmeshpro_livesdisplay;
    public GameObject textmeshpro_scoredisplay;
    public GameObject textmeshpro_highscoredisplay;



    //variables;
    public int lives;// = GameManager.instance.lives;
    public int score;
    public int highscore;

    //text components
    public TextMeshProUGUI livesdisplay_text;
    public TextMeshProUGUI scoredisplay_text;
    public TextMeshProUGUI highscoredisplay_text;

    void Start()
    {
        livesdisplay_text = textmeshpro_livesdisplay.GetComponent<TextMeshProUGUI>();
        scoredisplay_text = textmeshpro_scoredisplay.GetComponent<TextMeshProUGUI>();
        highscoredisplay_text = textmeshpro_highscoredisplay.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        lives = GameManager.instance.lives;
        score = GameManager.instance.score;
        highscore = GameManager.instance.highscore;
        //string livesS = lives.ToString;
        //Update Text
        livesdisplay_text.text = "LIVES  :  " + lives;
        scoredisplay_text.text = "SCORE  :  " + score;
        highscoredisplay_text.text = "HIGHSCORE  :  " + highscore;
    }
}
