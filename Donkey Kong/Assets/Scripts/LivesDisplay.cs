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
    public GameObject textmeshpro_dashcountdisplay;
    public GameObject textmeshpro_dashcooldowndisplay;
    //New
    public GameObject hbIcon;
    //End new


    //variables;

    public int lives;// = GameManager.instance.lives;
    public int score;
    public int highscore;
    public float dashCount;
    public float dashCooldown;

    //text components
    public TextMeshProUGUI livesdisplay_text;
    public TextMeshProUGUI scoredisplay_text;
    public TextMeshProUGUI highscoredisplay_text;
    public TextMeshProUGUI dashcountdisplay_text;
    public TextMeshProUGUI dashcooldowndisplay_text;

    void Start()
    {
        livesdisplay_text = textmeshpro_livesdisplay.GetComponent<TextMeshProUGUI>();
        scoredisplay_text = textmeshpro_scoredisplay.GetComponent<TextMeshProUGUI>();
        highscoredisplay_text = textmeshpro_highscoredisplay.GetComponent<TextMeshProUGUI>();
        dashcountdisplay_text = textmeshpro_dashcountdisplay.GetComponent<TextMeshProUGUI>();
        //dashcooldowndisplay_text = textmeshpro_dashcooldowndisplay.GetComponent <TextMeshProUGUI>();
        
        //New
        hbIcon.SetActive(false);
        //End New


    }
    void Update()
    {
        //lives = GameManager.instance.lives;
        //score = GameManager.instance.score;
        //highscore = ScoreManager.instance.highscore;
        //dashCount = Player.instance.numberOfDashes;

        //string livesS = lives.ToString;
        //Update Text
        livesdisplay_text.text = "LIVES  :  " + lives;
        scoredisplay_text.text = "SCORE  :  " + score;
        highscoredisplay_text.text = "HIGHSCORE  :  " + highscore;
        dashcountdisplay_text.text = "DASHES : " + dashCount;


        //New
        if (Player.holdingBarrel) {
            hbIcon.SetActive(true);
        } else {
            hbIcon.SetActive(false);
        }
        //End new
    }
}
