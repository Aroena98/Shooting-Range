using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dit script wordt gebruikt om de zweefbewegingen van de player in de scene space mogelijk te maken
public class FPS : MonoBehaviour
{

    public CharacterController controller; //controller van de player
    public float speed = 12f; //beweegsnelheid
    
    void Update()
    {
        float x = Input.GetAxis("Horizontal"); //A&D en arrows input
        float z = Input.GetAxis("Vertical"); //W&S en arrows input

        Vector3 move = transform.right * x + transform.forward * z; //plaats in een variabele welke beweegrichting de input van de gebruiker aangeeft
        controller.Move(move * speed * Time.deltaTime); //voer de beweging soepel uit met de move methode van Unity. Alleen X en Z richting

    }
}
