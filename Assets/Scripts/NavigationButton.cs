using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
    public string[] flagsToSet;
    public string[] conditions;
    public string locationToLoad;
    public enum actionType {SceneChange, DisplayElement,LoadGame,NewGame,SaveGame,AppendToMainText, QuitGame}
    public actionType optionActionType;
    public GameObject elementToDisplay;
    public string textToAppend;
    public int dialogueIndex;

    private Button button;
    private TextMeshProUGUI buttonText;

    void OnEnable()
    {

       
    }

    public void SetUpButton(int index)
    {
        if (!CheckConditions())
        {
            UIController.shared.RemoveDialogueOptionFromList(dialogueIndex);
            this.gameObject.SetActive(false);
        }
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        dialogueIndex = index;
    }

    bool CheckConditions()
    {
        bool conditionsSatisfied = true;
        int i = 0;
        
        while (conditionsSatisfied && i < conditions.Length)
        {
            conditionsSatisfied = DatabaseManager.shared.GetFlagState(conditions[i]);
            i++;
        }
        return conditionsSatisfied;
    }

    void SetFlags()
    {
        for(int i = 0; i < flagsToSet.Length; i++)
        {
            DatabaseManager.shared.SetFlagState(flagsToSet[i], true);
        }
    }

    public void Action()
    {
        SetFlags();
        switch (optionActionType)
        {
            case actionType.SceneChange:
                LocationManager.shared.loadNewScene(locationToLoad);
                break;
            case actionType.DisplayElement:
                UIController.shared.TogglePopup();
                break;
            case actionType.LoadGame:
                DatabaseManager.shared.LoadFlagDatabaseFromFile();
                LocationManager.shared.loadNewScene(locationToLoad);
                break;
            case actionType.NewGame:
                DatabaseManager.shared.ResetFlagDatabase();
                LocationManager.shared.loadNewScene(locationToLoad);
                break;
            case actionType.SaveGame:
                DatabaseManager.shared.SaveFlagDatabaseFromFile();
                break;
            case actionType.AppendToMainText:
                UIController.shared.AppendToMainText(textToAppend);
                UIController.shared.RemoveDialogueOptionFromList(dialogueIndex);
                this.gameObject.SetActive(false);
                break;
            case actionType.QuitGame:
                break;
        }
    }

    public void HighLightButton()
    {
        button.Select();
    }

    public void toggleButtonInteractable()
    {
        button.interactable = !button.interactable;
    }
}
