using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SDAvailable2 : MonoBehaviour
{

	AudioSource torMessage;
	public TextMesh dashMessages;


	// Start is called before the first frame update
	void Start()
	{

		torMessage = GetComponent<AudioSource>();

	}

	// Update is called once per frame


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") ;
		torMessage.Play();
		dashMessages.text = "Self-Driving \n is Available!";
		StartCoroutine(ClearDashMessage());


	}
	private IEnumerator ClearDashMessage()
	{
		yield return new WaitForSeconds(2f);
		dashMessages.text = "";
	}

	
	
}
