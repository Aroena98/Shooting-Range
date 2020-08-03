using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//dit script wordt gebruikt om de prestaties van de player bij te houden: score bijhouden, score opslaan, feedback teruggeven, spellen resetten, volgende scene laden bij hoog genoege score etc.
public class Missions : MonoBehaviour
{
    //markers waar de speler heen moet lopen
    public ParticleSystem target1;
    public ParticleSystem target2;
    public ParticleSystem target3;
    public ParticleSystem target4;

    //de score in milliseconden die voor een bepaald aantal "sterren" behaald moet worden (lager is beter)
    public int miliStar1;
    public int miliStar2;
    public int miliStar3;

    public int totalTargets; //totaal aantal targets in de scene
    public int currentScene; //index van de huidige scene
    public Text counter; //GUI text voor de stopwatch
    public Text highScoreText; //GUI text voor de highscore
    public Text console; //GUI text voor de console
    public GameObject normalVersion; //de normale (niet gebroken) versie van de targets
    public GameObject loadingScreen; //object die de loading slider bevat
    public Slider slider; //slider die aangeeft hoe ver het volgende level geladen is
    public int nextScene; //de volgende scene index 
    public Animator deurAnimator; //de animator die gebruikt wordt om de deur te openen in level 3 (space)

    //teksten die getoond worden wanneer er scores behaald worden of targets aangeraakt worden
    public string firstText;
    public string target1Text;
    public string target2Text;
    public string target3Text;
    public string target4Text;
    public string miliStar1Text;
    public string miliStar2Text;
    public string miliStar3Text;

    private GameObject[] boxes; //een array met alle gebroken targets in de huidige scene
    private TimeSpan timePlaying; //tijd die de speler er over doet om alle targets te raken
    private float startTime; //tijd dat de speler het eerste target raakte
    private float elapsedTime; //verstreken tijd
    private float seconds, minutes; //tijd in seconden en minuten
    private bool timerIsInUse; //staat de timer "aan"?
    private bool nextLevelAllowed; //mag de speler al naar het volgende level?
    private int highScore; //de highscore in dit level
    
    void Start()
    {
       counter.text = "Time: 00:00.0"; //waneer de counter nog niet gestart is
       console.text = firstText; //toon de begintekst

       //haal de highscore op die behoort tot de huidige scene
       if (currentScene == 1){
       highScore = PlayerPrefs.GetInt("highScore1");
       highScoreText.text = "Highscore: " + highScore + " ms";
       } else if (currentScene == 2){
       highScore = PlayerPrefs.GetInt("highScore2");
       highScoreText.text = "Highscore: " + highScore + " ms";
       } else if (currentScene == 3){
       highScore = PlayerPrefs.GetInt("highScore3");
       highScoreText.text = "Highscore: " + highScore + " ms";
       }

       timerIsInUse = false; //de timer loopt nog niet
       nextLevelAllowed = false; //de player mag nog niet naar het volgende level
    }

    void Update()
    {
        if (Input.GetButtonDown("nextLevel") && nextLevelAllowed && currentScene !=3){ //wanneer de speler op L drukt (load next level), hij naar het volgende level mag en de huidige scene niet 3 (space eindlevel) is
            StartCoroutine(LoadAsynchronously(nextScene)); //laad de volgende scene asynchroon in een coroutine (thread), dit geeft de mogelijkheid om een laadbalk weer te geven
        }

        if (Input.GetButtonDown("reset")){ //wanneer de speler op R (reset) drukt
            boxes = GameObject.FindGameObjectsWithTag("target"); //zoek alle objecten die de tag "target" hebben, dit zijn instanties van gebroken targets
            foreach(GameObject boxes in boxes){
                Instantiate(normalVersion, boxes.transform.position, boxes.transform.rotation); //zet nieuwe en hele targets neer op de plekken van de gebroken targets
                GameObject.Destroy(boxes); //verwijder alle gebroken targets
            }

        timerIsInUse = false; //timer wordt niet gebruikt
        }

        boxes = GameObject.FindGameObjectsWithTag("target"); //zoek alle objecten die de tag "target" hebben, dit zijn instanties van gebroken targets
        if (boxes.Length>0 && boxes.Length<totalTargets){ //wanneer het aantal kapotte targets groter is dan 1 maar kleiner dan het totaal aantal targets
            if(!timerIsInUse){ //wanneer de timer niet in gebruik is
                BeginTimer(); //start de timer
            }
            
        }else if(boxes.Length>=totalTargets){ //wanneer alle targets geraakt zijn
            if (timerIsInUse && highScore > Convert.ToInt32(timePlaying.TotalMilliseconds) || highScore == 0){//wanneer de timer in gebruik is en de huidige score beter is dan de highscore of als er nog geen highscore is (highscore == 0)
                //sla de highscore op onder PlayerPrefs en onder de juiste scene
                if (currentScene == 1){
                    PlayerPrefs.SetInt("highScore1", Convert.ToInt32(timePlaying.TotalMilliseconds));
                    highScoreText.text = "Highscore: " + Convert.ToInt32(timePlaying.TotalMilliseconds) + " ms";
                } else if (currentScene == 2){
                    PlayerPrefs.SetInt("highScore2", Convert.ToInt32(timePlaying.TotalMilliseconds));
                    highScoreText.text = "Highscore: " + Convert.ToInt32(timePlaying.TotalMilliseconds) + " ms";
                } else if (currentScene == 3){
                    PlayerPrefs.SetInt("highScore3", Convert.ToInt32(timePlaying.TotalMilliseconds));
                    highScoreText.text = "Highscore: " + Convert.ToInt32(timePlaying.TotalMilliseconds) + " ms";
                } 
            }

            if(timerIsInUse && timePlaying.TotalMilliseconds<=miliStar3 ){ //wanneer de timer in gebruik is en de huidige score is beter of gelijk aan de 3 sterren score
                console.text = miliStar3Text; //geef tekst aan die hoort bij 3 sterren
                nextLevelAllowed = true; //de speler ag nu naar het volgende level
            } else if(timerIsInUse && timePlaying.TotalMilliseconds<=miliStar2 ){ //wanneer de timer in gebruik is en de huidige score is beter of gelijk aan de 2 sterren score
                console.text = miliStar2Text; //geef tekst aan die hoort bij 2 sterren
            } else if(timerIsInUse && timePlaying.TotalMilliseconds<=miliStar1 ){ //wanneer de timer in gebruik is en de huidige score is beter of gelijk aan de 1 ster score
                console.text = miliStar1Text; //geef tekst aan die hoort bij 1 ster
            } else if(timerIsInUse) { //wanneer de timer in gebruik is maar de score niet hoog genoeg is voor een ster
                console.text = "Veel langzamer kan het bijna niet! Druk op R om het opnieuw te proberen.";
            }

            timerIsInUse = false; //stop de timer
        }
    }

    public void BeginTimer(){
        timerIsInUse = true; //de timer is nu in gebruik
        elapsedTime = 0f; //er is nog geen tijd verstreken
        StartCoroutine(UpdateTimer()); //start de coroutine van de timer
    }

    public IEnumerator LoadAsynchronously (int nextScene){
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene); //laad de scene asynchroon

        loadingScreen.SetActive(true); //laat het laadbalkje zien

        while (!operation.isDone){ //zolang het level niet geladen is
            float progress = Mathf.Clamp01(operation.progress / .9f); //progress loopt van 0 naar 0.9, hierna zet unity alles aan en op de juiste plek. progress wordt door operation.progress maar geupdated tot 0.9
            slider.value = progress; //laat de progress zien
            yield return null;
        }
    }

    public IEnumerator UpdateTimer(){
        while(timerIsInUse){ //zolang de timer in gebruik is
            elapsedTime += Time.deltaTime; //tel bij de verstreken tijd de tussentijds verstreken tijd op
            timePlaying = TimeSpan.FromSeconds(elapsedTime); //hoelang de speler bezig is met de targets
            string timePlayingStr = "Time: " + timePlaying.ToString("mm':'ss'.'ff"); //maak een string die de tijd aangeeft
            counter.text = timePlayingStr; //laat deze string nu zien in de GUI
            yield return null;
        }
    }

    void OnTriggerEnter(Collider c){ //wanneer de speler een collider aanraakt die gemarkeerd is als trigger
        if(c.gameObject.name == "marker"){ //wanneer het object marker heet
            target1.Stop(); //zet het target uit
            console.text = target1Text; //laat de bijbehordende tekst zien
        } else if(c.gameObject.name == "marker2"){ //wanneer het object marker2 heet
            target2.Stop(); //zet het target uit
            console.text = target2Text; //laat de bijbehordende tekst zien
        } else if(c.gameObject.name == "marker3"){ //wanneer het object marker3 heet
            target3.Stop(); //zet het target uit
            console.text = target3Text; //laat de bijbehordende tekst zien
            deurAnimator.SetBool("DoorOpen", true);
        } else if(c.gameObject.name == "marker4"){ //wanneer het object marker4 heet
            target4.Stop(); //zet het target uit
            console.text = target4Text; //laat de bijbehordende tekst zien
            Cursor.visible = true; //maak de cursor weer zichtbaar voor het laatste menu
            Cursor.lockState = CursorLockMode.None; //maak de cursor los van het midden
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single); //laad de volgende scene, de eindscene     
        }
    }
}
