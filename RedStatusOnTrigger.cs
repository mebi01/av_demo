using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStatusOnTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    // public GameObject AutomationStatus;
    public Color myCal2;
    public Material myMat;


    void Start()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player") ;

        myMat.color = myCal2;

    }
    }

