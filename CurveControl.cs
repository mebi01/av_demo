using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveControl : MonoBehaviour
{



    AudioSource curveControlMessage;
    public GameObject myCar;

    // Start is called before the first frame update
    void Start()
    {
        curveControlMessage = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
    
        if (myCar.GetComponent<AICar_Script>().enabled == true)
        {
            if (other.gameObject.tag == "Player") ;
            
            curveControlMessage.Play();

        }
    }




}
