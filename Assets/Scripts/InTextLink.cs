using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InTextLink : MonoBehaviour
{
    public string textToAppend;
    public bool flashText;
    public string textToFlash;
    public string[] flagsToSet;

    private bool pressed;
    private Button button;
    private RectTransform rect; 

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
    }

    public void ShiftElement(float shiftAmount)
    {
        Vector3 newPos = new Vector3(rect.localPosition.x, rect.localPosition.y, rect.localPosition.z);
        newPos.y += shiftAmount/2;
        rect.localPosition = newPos;
    }


    public void ButtonAction()
    {
        if (!pressed)
        {
            UIController.shared.AppendToMainText(textToAppend);
            if (flashText)
            {
                UIController.shared.SetFlashingTagText(textToFlash);
            }
            pressed = true;
            button.interactable = false;
            SetFlags();
        }
    }

    void SetFlags()
    {
        for(int i = 0; i < flagsToSet.Length; i++)
        {
            DatabaseManager.shared.SetFlagState(flagsToSet[i],true);
        }
    }
}
