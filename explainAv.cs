using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explainAv : MonoBehaviour

{

	public GameObject myCar;

	public TextMesh dashMessages;
	AudioSource explainAVmessage;


	private void Start()
	{
		explainAVmessage = GetComponent<AudioSource>();
        dashMessages.text = "";


    }
	// Use this for initialization
	void OnTriggerEnter(Collider other)
	{
        if (myCar.GetComponent<AICar_Script>().enabled == true)
        {

            if (other.gameObject.tag == "Player") ;
		//dashMessages.text = "Self-Driving";
		StartCoroutine(ClearDashMessage());
            //myMat.color = myCol;
        explainAVmessage.Play();


	}
	}


	private IEnumerator ClearDashMessage()
	{
		yield return new WaitForSeconds(2f);
		//dashMessages.text = "";
	}



}
