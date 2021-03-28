using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum menuSelectionType { dialogue, navBar, popUp ,mainText}
    public static UIController shared;

    public GameObject inlineTextParent;

    public Transform dialogueOptionParent;
    public Transform navbarOptionParent;

    public Animator flashingTag;
    public Animator navBar;
    public Animator PopUp;
    public GameObject mainTextObj;

    private TextMeshProUGUI mainText;
    private Scrollbar mainTextScrollBar;
    private Image mainTextImage;
    public  RectTransform mainTextRect;
    
    private NavigationButton[] dialogueOptions;
    private NavigationButton[] navBarOptions;
    private InTextLink[] inLineTextLinks;

    private Stack<menuSelectionType> previousMenuSelections = new Stack<menuSelectionType>();    
    private menuSelectionType currentMenuSelection;
    
    private int currentDialogueSelection = 0;
    private int currentNavBarSelection = 0;

    private bool mainTextShifting;
    private float mainTextStartingHeight;
   
    void Start()
    {
        if(shared == null)
        {
            shared = this;
        }
        SetUpMainTextElements();
        currentMenuSelection = menuSelectionType.dialogue;
        getNavigationOptions();
        CycleSelectedOption(0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            CycleSelectedOption(-1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            CycleSelectedOption(+1);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(mainTextScrollBar.size < 1)
            {
                previousMenuSelections.Push(currentMenuSelection);
                currentMenuSelection = menuSelectionType.mainText;
                SelectMainText();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMenuSelection == menuSelectionType.mainText)
            {
                currentMenuSelection = previousMenuSelections.Pop();
                DeselectMainText();
                CycleSelectedOption(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleNavBar();
        }
        if (mainTextShifting)
        {
            float endingHeight = mainTextRect.rect.height;
            if(endingHeight != mainTextStartingHeight)
            {
                mainTextShifting = false;
                ShiftInlineTextElements(endingHeight);
            }
        }

    }

    void ShiftInlineTextElements(float endHeight)
    {
        float difference = endHeight - mainTextStartingHeight;
        for(int i = 0; i < inLineTextLinks.Length; i++)
        {
            inLineTextLinks[i].ShiftElement(difference);
        }
    }

    void getNavigationOptions()
    {
        dialogueOptions = dialogueOptionParent.GetComponentsInChildren<NavigationButton>();
        navBarOptions = navbarOptionParent.GetComponentsInChildren<NavigationButton>();
        SetDialogueOptionIndexes();
        SetUpNavBarButtons();
        inLineTextLinks = inlineTextParent.GetComponentsInChildren<InTextLink>();
    }

    void SetUpNavBarButtons()
    {
        for(int i = 0; i < navBarOptions.Length; i++)
        {
            navBarOptions[i].SetUpButton(i);
        }
    }

    void SetDialogueOptionIndexes()
    {
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            dialogueOptions[i].SetUpButton(i);
        }
    }

    void SetUpMainTextElements()
    {
        mainText = mainTextObj.GetComponentInChildren<TextMeshProUGUI>();
        mainTextScrollBar = mainTextObj.GetComponentInChildren<Scrollbar>();
        mainTextImage = mainTextObj.GetComponent<Image>();
    }

    void CycleSelectedOption(int direction) //+ for down; - for up; 0 for staying the same
    {
        switch (currentMenuSelection)
        {
            case menuSelectionType.dialogue:
                cycleDialogueSelection(direction);
                break;
            case menuSelectionType.navBar:
                cycleNavBarSelection(direction);
                break;
            case menuSelectionType.popUp:
                cycleDialogueSelection(0);
                break;
            case menuSelectionType.mainText:
                SelectMainText();
                break;
        }

    }

    void cycleDialogueSelection(int direction)
    {
        if (direction > 0)
        {
            currentDialogueSelection++;
            if (currentDialogueSelection > dialogueOptions.Length - 1)
            {
                currentDialogueSelection = 0;
            }
        }
        else if (direction < 0)
        {
            currentDialogueSelection--;
            if (currentDialogueSelection < 0)
            {
                currentDialogueSelection = dialogueOptions.Length - 1;
            }
        }

        dialogueOptions[currentDialogueSelection].HighLightButton();
    }

    void cycleNavBarSelection(int direction)
    {
        if (direction > 0)
        {
            currentNavBarSelection++;
            if (currentNavBarSelection > navBarOptions.Length - 1)
            {
                currentNavBarSelection = 0;
            }
        }
        else if (direction < 0)
        {
            currentNavBarSelection--;
            if (currentNavBarSelection < 0)
            {
                currentNavBarSelection = navBarOptions.Length - 1;
            }

        }
        navBarOptions[currentNavBarSelection].HighLightButton();
    }

    public void RemoveDialogueOptionFromList(int dialogueToRemove)
    {
        NavigationButton[] newDialogueOptions = new NavigationButton[dialogueOptions.Length - 1];
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            if(dialogueToRemove != i)
            {
                if(dialogueToRemove > i)
                {
                    newDialogueOptions[i] = dialogueOptions[i];
                }
                else
                {
                    newDialogueOptions[i - 1] = dialogueOptions[i];
                }
            }
        }

        dialogueOptions = newDialogueOptions;
        SetDialogueOptionIndexes();
        CycleSelectedOption(-1);
    }

    void SelectMainText()
    {
        mainTextImage.color = new Color(1,1,1,.5f);
    }

    void DeselectMainText()
    {
        mainTextImage.color = new Color(1, 1, 1, 1);
    }

    public void TogglePopup()
    {
        if (PopUp == null)
        {
            print("ERROR: Trying to toggle a popup on a scene that doesn't have one");
        }
        else
        {
            PopUp.SetBool("Out", !PopUp.GetBool("Out"));
            if (currentMenuSelection != menuSelectionType.popUp)
            {
                currentMenuSelection = menuSelectionType.popUp;
            }
            else
            {
                currentMenuSelection = menuSelectionType.dialogue;
            }
            toggleInteractiveDialogueOptions();
        }
    }
    public void ToggleNavBar()
    {
        if (navBar == null)
        {
            print("ERROR: Trying to toggle a navbar on a scene that doesn't have one");
        }
        else
        {
            navBar.SetBool("NavBarOut", !navBar.GetBool("NavBarOut"));
            if (currentMenuSelection != menuSelectionType.navBar)
            {
                previousMenuSelections.Push(currentMenuSelection);
                currentMenuSelection = menuSelectionType.navBar;
                ToggleOffAllUnselectedElements();
            }
            else
            {
                currentMenuSelection = previousMenuSelections.Pop();
                CycleSelectedOption(0);
            }
        }
        currentNavBarSelection = 0;
        CycleSelectedOption(0);
    }

    void ToggleOffAllUnselectedElements()
    {
        DeselectMainText();
    }

    void toggleInteractiveDialogueOptions()
    {
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            if (i != currentDialogueSelection)
            {
                dialogueOptions[i].toggleButtonInteractable();
            }
        }
    }

    public void AppendToMainText(string textToAppend)
    {
        mainTextStartingHeight = mainTextRect.rect.height;
        mainText.text += '\n';
        mainText.text += '\n';
        mainText.text += textToAppend;
        if (inlineTextParent != null)
        {
            mainTextShifting = true;
        }
    }

    public void SetFlashingTagText(string newText)
    {
        flashingTag.GetComponentInChildren<TextMeshProUGUI>().text = newText;
        flashingTag.SetTrigger("FlashAnimation");
    }

}
