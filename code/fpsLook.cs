using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fpsLook : MonoBehaviour
{

    public float sensitivity = 100f; //look sensitivity over x- en y-as
    public Transform playerBody; //"lichaam" van de player
    public float speed = 20f; //snelheid

    private float ammount; //de grootte van de draai die de player maakt met Q en E
    private float xRotation = 0f; //rotatie over de x-as
    private float zRotation = 0f; //rotatie over de z-as

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //zorg dat de cursor in het midden van het scherm vastzit
        Cursor.visible = false; //maak de cursor onzichtbaar
    }

    void Update()
    {
        if (Input.GetButton("rotateRight")){ //Wanneer de speler E indrukt
        
         ammount = 10 * Time.deltaTime * -speed;
         zRotation += ammount; //de roatie die in de z-richting gemaakt moet worden
        } 
        if (Input.GetButton("rotateLeft")){ //Wanneer de speler Q indrukt
        
         ammount =  10 * Time.deltaTime * -speed;
         zRotation -= ammount; //de roatie die in de z-richting gemaakt moet worden
        } 
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime; //muisbeweging x-as rotatie
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime; //muisbeweging y-as rotatie

        xRotation -= mouseY; //de rotatie wordt hier niet tot maximaal 90 graden beperkt zoals in fpsLooknormal.cs, omdat je in een gewichtsloze situatie gemakkelijk "salto's" kunt maken
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation); //voer de rotatie ten opzichte van de player zelf uit (hier dus ook in z-richting)
        
        playerBody.Rotate(Vector3.up * mouseX); //roteer de speler links en rechtsom, via muisbeweging aangestuurd
        
        
    }
}
