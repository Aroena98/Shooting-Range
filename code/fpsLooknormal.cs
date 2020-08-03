using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dit script wordt gebruikt om de kijkbewegingen van de player in de scene desert en moon mogelijk te maken
public class fpsLooknormal : MonoBehaviour
{

    public float sensitivity = 100f; //look sensitivity over x- en y-as
    public Transform playerBody; //"lichaam" van de player
    public float speed = 20f; //snelheid

    private float xRotation = 0f; //rotatie over de x-as
    private float zRotation = 0f; //rotatie over de z-as

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //zorg dat de cursor in het midden van het scherm vastzit
        Cursor.visible = false; //maak de cursor onzichtbaar
    }

    void Update()
    {
    
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; //muisbeweging x-as rotatie
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; //muisbeweging y-as rotatie

        xRotation -= mouseY; //haal de nieuwe input van de rotatie af van de vorige rotatie
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //zorg dat de speler niet meer dan 90 graden naar onder of boven kan kijken
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //voer de rotatie ten opzichte van de player zelf uit
        
        playerBody.Rotate(Vector3.up * mouseX); //roteer de speler links en rechtsom, via muisbeweging aangestuurd
        
        
    }
}
