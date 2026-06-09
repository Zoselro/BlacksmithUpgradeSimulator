using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("DialogueBox")]
    [SerializeField] private DialogueBoxUI dialogueBoxUI;
    [SerializeField] private Image[] blackSmithImageUI;

    private Dir currentDir;

    public void Show(string name, string text, Sprite sprite, Speaker speak, Dir dir)
    {
        dialogueBoxUI.Show(name, text, dir);


        if (speak == Speaker.BlackSmith)
        {
            if(currentDir != dir)
            {
                blackSmithImageUI[(int)currentDir].color = new Color(255f, 255f, 255f, 0f);
                currentDir = dir;
                blackSmithImageUI[(int)currentDir].color = new Color(255f, 255f, 255f, 255f);
            }
        }
        if (speak == Speaker.BlackSmith)
            blackSmithImageUI[(int)currentDir].sprite = sprite;

        //characterImageUI[(int)dir].sprite = sprite;
    }

    public void ShowBlackSmith(bool active, Dir dir)
    {
        if (dir == Dir.Left)
        {
            blackSmithImageUI[(int)Dir.Left].color = new Color(255f, 255f, 255f, active ? 255f : 0f);
        }
        else
        {
            blackSmithImageUI[(int)Dir.Right].color = new Color(255f, 255f, 255f, active ? 255f : 0f);
        }
    }
}
