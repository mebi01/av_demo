using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class AVAvailable : MonoBehaviour
{

    public TextMesh dashMessages;
    // public GameObject myCar;
    //public Material myMat;
    // public Color myCol;

    AudioSource avavailable1;


    public GameObject automationMode;


    private void Start()
    {
        avavailable1 = GetComponent<AudioSource>();


    }
    // Use this for initialization
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player");
        dashMessages.text = "Self-Driving \n is Available!";
        avavailable1.Play();
        automationMode.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        InvokeRepeating("FlashLabel", .5f, 1);
        StartCoroutine(ClearDashMessage());

    }


    private IEnumerator ClearDashMessage()
    {
        yield return new WaitForSeconds(4f);
        dashMessages.text = "";
    }

    void FlashLabel()
    {
        if (automationMode.activeSelf)
            automationMode.SetActive(false);
        else
            automationMode.SetActive(true);
    }


}

