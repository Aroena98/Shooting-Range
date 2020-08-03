using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject destroyedVersion; //een verwijzing naar de "gebroken" prefab van het target
    
    public void Break(){
       Instantiate(destroyedVersion, transform.position, transform.rotation); //plaats de prefab precies op dezelfde plaats als het target
       Destroy(gameObject); //verwijder het target
    }
}
