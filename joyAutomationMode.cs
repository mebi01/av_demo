using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joyAutomationMode : MonoBehaviour
{
    public GameObject myCar;

    private bool activateAIs;


    // Update is called once per frame
    void Update()
    {
        

        

        if (Input.GetKey("joystick " + 1 + " button " + 5))
        {
            myCar.GetComponent<PlayerCar_Script>().enabled = false;
            myCar.GetComponent<AICar_Script>().enabled = true;

            //automationMode_blinkG.GetComponent<Renderer>().enabled = false;
            //automationMode_blinkR.GetComponent<Renderer>().enabled = false;
            //automationMode_r.GetComponent<Renderer>().enabled = false;


            //automationMode_g.GetComponent<Renderer>().enabled = true;

            //Debug.Log("Enabled AI on my car..");
            //automationMode.GetComponent<Renderer>().material.SetColor("_Color", Color.green);

            Debug.Log("Enabled AI on my car..");

        }


        if (Input.GetKey("joystick " + 1 + " button " + 7))
        {
            myCar.GetComponent<PlayerCar_Script>().enabled = true;
            myCar.GetComponent<AICar_Script>().enabled = false;
            Debug.Log("Enabled AI on my car..");

            //automationMode_blinkR.GetComponent<Renderer>().enabled = false;
            //automationMode_r.GetComponent<Renderer>().enabled = false;
            //automationMode_g.GetComponent<Renderer>().enabled = false;

            //automationMode_blinkG.GetComponent<Renderer>().enabled = true;
            //automationMode_blinkG.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            //InvokeRepeating("FlashLabel", .3f, 1);
            Debug.Log("Disable AI on my car..");



            //automationMode.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }



        if (System.Math.Abs(Input.GetAxis("Horizontal")) > 0 || System.Math.Abs(Input.GetAxis("Vertical")) > 0)
        {

            myCar.GetComponent<PlayerCar_Script>().enabled = true;
            myCar.GetComponent<AICar_Script>().enabled = false;
            Debug.Log("Disable AI on my car..");



        }






    }


}



