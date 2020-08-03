using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused = false; //is de game gepauzeerd?
    public GameObject menuUI; //container van alle menu elementen
    public GameObject otherUI; //container van alle niet-menu elementen
    public GameObject crossHair; //de crosshair om te richten
    public int resetScene; //de huidige scene die opnieuw moet worden geladen bij een reset
    public int mainMenuScene; //de scene van het hoofdmenu

    void Start(){
        menuUI.SetActive(false); //zet het pauzemenu uit 
        Cursor.lockState = CursorLockMode.Locked; //lock de muis in het midden van het scherm
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){ //wanneer de gebruiker op pauze (escape) drukt
            if (gameIsPaused){ //wanneer de game is gepauzeerd
                Resume(); //hervat de game
            } else { //zo niet
                Pause(); //pauzeer de game
            }
        }
    }

    public void Resume(){
        Cursor.lockState = CursorLockMode.Locked; //zet de cursor weer vast in het midden
        Cursor.visible = false; //maak de cursor weer onzichtbaar
        menuUI.SetActive(false); //maak het menu inactief
        otherUI.SetActive(true); //maak de andere GUI elementen weer actief
        crossHair.SetActive(true); //maak de crosshair weer actief
        Time.timeScale = 1f; //laat de tijd weer doorlopen
        gameIsPaused = false; //de game is niet meer gepauzeerd
    }

    void Pause(){
        Cursor.lockState = CursorLockMode.None; //maak de cursor los van het midden
        Cursor.visible = true; //maak de cursor zichtbaar
        menuUI.SetActive(true); //maak het menu actief
        otherUI.SetActive(false); //maak alle andere GUI elementen inactief
        crossHair.SetActive(false); //maak de crosshair inactief
        Time.timeScale = 0f; //zet de tijd stil
        gameIsPaused = true; //de game is nu gepauzeerd
    }

    public void ResetHighscore(){ //reset de score per level
        PlayerPrefs.DeleteKey("highScore1");
        PlayerPrefs.DeleteKey("highScore2");
        PlayerPrefs.DeleteKey("highScore3");
    }

    public void Quality(int quality){ //kwaliteitslevel dat gekozen wordt via een dropdown menu
        QualitySettings.SetQualityLevel(quality);
    }

    public void LoadMenu(){
        Time.timeScale = 1f; //laat de tijd weer doorlopen
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single); //laad het hoofdmenu
    }

    public void Resetlevel(){
        Time.timeScale = 1f; //laat de tijd weer doorlopen
        SceneManager.LoadScene(resetScene, LoadSceneMode.Single); //laad dezelfde scene opnieuw (reset scene bij bijvoorbeeld een bug)
    }

    public void QuitGame(){ //sluit de gehele game af
        Application.Quit();
    }
}
