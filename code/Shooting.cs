using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Camera camera; //camera object
    public float range = 100f; //afstand die de raycast af scant (bij nader inzien niet heel relevant, wellicht voor uitbreidingen)
    public ParticleSystem MuzzleFlash; //muzzleflash voor tijdens het schieten
    public GameObject impact; //rook en puin wanneer een kogel een collider raakt
    public float impactForce = 300f; //kracht die de kogel meegeeft aan een rigidbody wanneer hij deze raakt
    public float fireRate = 5f; //hoeveel keer schieten per seconde
    public Animator animator; //het animator object dat de schietanimatie verzorgt
    public AudioSource audio; //geluidsbron voor schietgeluid
    public GameObject casingPrefab; //huls die uitgeworpen wordt
    public Transform casingExitLocation; //plek waar de huls uitgeworpen wordt

    private float nextTimeToFire = 0f; //wanneer er weer gevuurd mag worden

    void Update()
    {
        animator.SetBool("Shooting", false); //er wordt niet geschoten
        
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire){ //waneer de speler op vuren drukt en hij ook mag vuren
           nextTimeToFire = Time.time + 1f /fireRate; //wanneer er weer geschoten mag worden
           Shoot(); //voer het schieten uit
        } 
    }
    
    void Shoot(){
        MuzzleFlash.Play(); //laat de muzzleflash zien
        audio.Play(); //laat schietgeluid horen
        animator.SetBool("Shooting", true); //voer schietanimatie uit
        CasingRelease(); //werp de huls uit

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range)){ //voer een raycast uit met een bepaalde range en werp de informatie over deze raycast uit in variabele hit
            
           Target target = hit.transform.GetComponent<Target>(); //kijk wat de raycast raakt

           if (target != null){ //waneer een target geraakt wordt
               target.Break(); //voer het target script uit (spawn een gebroken instantie op dezelfde plek)
           }

           if(hit.rigidbody != null){ //wanneer de raycast een rigidbody raakt
               hit.rigidbody.AddForce(-hit.normal * impactForce); //geef de rigidbody een kracht (impactforce) mee
           }
           Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal)); //plaats een impact effect op de plek waar de raycast een collider raakt. plaats het effect precies in de tegenovergestelde richting (normaalrichting)
        }
        
    }

    void CasingRelease()
    {
        GameObject casing;
        casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject; //maak een instantie aan van de prefab van de huls op de exitlocatie van het wapen
        //geef de huls een kracht mee en een random torque om het natuurlijk te laten lijken
        casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 10f);
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
    }

}
