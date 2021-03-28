using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LocationManager : MonoBehaviour
{

    public static LocationManager shared;

    private enum delimiters  { locationPointer = '>', mainText = '_', dialogueOption = '|'}

    void Start()
    {
        if(shared == null)
        {
            shared = this;
        }
        else
        {
            Destroy(shared);
            shared = this;
        }
    }

    public void loadNewScene(string newScene)
    {
        SceneManager.LoadScene(newScene);
    }



    private char[] DelimAsCharArray(delimiters delim) //needed because Split() must take in a char array 
    {
        char[] delimAsArray = {(char) delim};
        return delimAsArray;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
