using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    public TextMeshProUGUI mainText;
    public string toAppend;
    public RectTransform thisRect;

    private bool startUpdating;
    private float startingHeight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startUpdating)
        {
            print("updating");
            float endingHeight = mainText.GetComponent<RectTransform>().rect.height;
            if (endingHeight != startingHeight)
            {
                print("changingPos");
                changePos(endingHeight);
            }
        }
    }

    void changePos(float endHeight)
    {
        startUpdating = false;
        float difference = endHeight - startingHeight;

        Vector3 newRect = new Vector3(thisRect.localPosition.x, thisRect.localPosition.y + difference/2, thisRect.localPosition.z);
        print("x: " + newRect.x + " y: " + newRect.y + " diff: " + difference + " start: " + startingHeight + " end: " + endHeight);
        thisRect.localPosition = newRect;
    }
    public void ButtonAction()
    {
        startingHeight = mainText.GetComponent<RectTransform>().rect.height;
        mainText.text += toAppend;
        startUpdating = true;
    }
}
