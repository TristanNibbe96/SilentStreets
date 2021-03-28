using UnityEngine;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{

    public Transform buttonParents;

    private int currentSelection = 0;
    private Button[] navButtons;

    // Start is called before the first frame update
    void Start()
    {
        navButtons = buttonParents.GetComponentsInChildren<Button>();
        CycleCurrentSelection(0);
    }

    void CycleCurrentSelection(int direction) //+ for right; - for left
    {
        if (direction > 0)
        {
            currentSelection++;
            if(currentSelection >= navButtons.Length)
            {
                currentSelection = 0;
            }
        }else if(direction < 0)
        {
            currentSelection--;
            if(currentSelection < 0)
            {
                currentSelection = navButtons.Length - 1;
            }
        }
        navButtons[currentSelection].Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CycleCurrentSelection(-1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CycleCurrentSelection(+1);
        }
    }
}
