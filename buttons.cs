using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class buttons : MonoBehaviour
{
    // Start is called before the first frame update
   public void Next()
    {
        SceneManager.LoadScene("TestScene");
    }
    public void Back()
    {
        SceneManager.LoadScene("first_scene");
    }

}

