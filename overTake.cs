using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overTake : MonoBehaviour
{


   

    AudioSource overTakeMessage;


    // Start is called before the first frame update
    void Start()
    {

        overTakeMessage = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") ;
        overTakeMessage.Play();


    }




}
