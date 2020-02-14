using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TOR1 : MonoBehaviour
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
        dashMessages.text = "Please Take Over";
        
        StartCoroutine(ClearDashMessage());


    }
    private IEnumerator ClearDashMessage()
    {
        yield return new WaitForSeconds(2f);
        dashMessages.text = "";
    }
}
