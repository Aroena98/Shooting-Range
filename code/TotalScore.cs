using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : MonoBehaviour
{

    public Text score; //score UI tekst
    public GameObject scoreMenu; //scoremenu object
    
    void Start()
    {
        scoreMenu.SetActive(true); //laat de totale score zien
        score.text = PlayerPrefs.GetInt("highScore1") + PlayerPrefs.GetInt("highScore2") + PlayerPrefs.GetInt("highScore3") + "MS"; //de totale score is de highscores uit de 3 scenes opgeteld (lager is beter)
    }
}
