using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stopSDafterTOR1 : MonoBehaviour
{
    public GameObject myCar;
    // AudioSource explainTor1;
    public TextMesh dashMessages;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player");
        myCar.GetComponent<AICar_Script>().enabled = true;

            //  myCar.GetComponent<AICar_Script>().StopSelfDriving();
        myCar.GetComponent<PlayerCar_Script>().SetCarSpeed();

        dashMessages.text = "Emergency Stop";

    }

}

