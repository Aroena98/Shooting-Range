using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Dit script wordt gebruikt om de loopbewegingen van de player in de scene desert en moon mogelijk te maken
public class FPSnormal : MonoBehaviour
{

    public CharacterController controller; //controller van de player
    public float speed = 12f; //loopsnelheid
    public Transform groundCheck; //gameobject dat groundcollisions detecteert
    public float groundDistance; //geeft de afstand tot de ground aan
    public LayerMask groundMask; //geeft aan welke layer in de gate gehouden moet worden voor groundcollisions (terrain object moet dus op layer "ground" zitten)
    public float jumpHeight = 3f; //hoogte die gesprongen kan worden
    public float gravity = -9.81f; //hier wordt geen gebruik gemaakt van een rigidbody dus is dit de variabele voor de zwaartekracht

    Vector3 currentSpeed; //snelheid in x,y,z richtingen
    private bool isGrounded; //raakt de speler de grond?

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //kijk of de controller de grond raakt doormiddel van een daarvoor aangemaakte sphere

        if(isGrounded && currentSpeed.y < 0){ //wanneer de speler op de grond staat en zijn y beweging minder dan 0 is
            currentSpeed.y = -1f; //geef de speler een verticale snelheid van -1f. Dit is een soort zwaartekracht. Variabele mag niet te groot worden anders wordt de speler teveel naar beneden getrokken
        }

        float x = Input.GetAxis("Horizontal"); //A&D en arrows input
        float z = Input.GetAxis("Vertical"); //W&S en arrows input
        Vector3 move = transform.right * x + transform.forward * z; //plaats in een variabele welke beweegrichting de input van de gebruiker aangeeft
        controller.Move(move * speed * Time.deltaTime); //voer de beweging soepel uit met de move methode van Unity. Alleen X en Z richting

        if(Input.GetButtonDown("Jump") && isGrounded){ //wanneer de gebruiker op "springen" drukt (spatie) en de speler op de grond staat
            currentSpeed.y = Mathf.Sqrt(jumpHeight * -2f * gravity); //natuurkundige formule, laat de speler springen
        }

        currentSpeed.y +=  gravity * Time.deltaTime; //tel iedere tijdsstap "zwaartekracht" op bij de speler
        controller.Move(currentSpeed * Time.deltaTime); //voert nu ook de beweging in de y richting uit
    }
}
