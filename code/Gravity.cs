using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public float grav = 0f; //-9,81 aarde, -1,62 maan, 0 ruimte
    void Start()
    {
        Physics.gravity = new Vector3( 0, grav, 0); //geef Unity de gewenste zwaartekracht door (y-as)
    }
}
