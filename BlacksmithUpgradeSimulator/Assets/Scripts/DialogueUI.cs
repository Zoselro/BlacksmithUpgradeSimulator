using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("DialogueBox")]
    [SerializeField] private DialogueBoxUI dialogueBoxUI;
    [SerializeField] private Image[] imageUI;
    //[SerializeField] private Animator[] imageAnimators;

    private Dir currentDir;

    public void Show(string name, string text, Sprite sprite, Speaker speak, Dir dir)
    {
        dialogueBoxUI.Show(name, text, dir);
        
        if(speak == Speaker.BlackSmith)
            ShowImage(dir, sprite);
    }

    // 방향에 따라 이미지를 보여주는 메서드
    public void ShowImage(Dir dir, Sprite sprite)
    {
        if (currentDir != dir)
        {
            imageUI[(int)currentDir].color = new Color(255f, 255f, 255f, 0f);
            currentDir = dir;
            imageUI[(int)currentDir].color = new Color(255f, 255f, 255f, 255f);

        }
         imageUI[(int)currentDir].sprite = sprite;
    }

    public void EnterCharacter(Dir dir)
    {
        //imageAnimators[(int)dir].SetTrigger("Entrance");
    }


    public void ShowImageAnimation(Dir dir, Sprite sprite)
    {

    }

    public void HideImage(Dir dir)
    {

    }

    public void ShowBlackSmith(bool active, Dir dir)
    {
        if (dir == Dir.Left)
        {
            imageUI[(int)Dir.Left].color = new Color(255f, 255f, 255f, active ? 255f : 0f);
        }
        else
        {
            imageUI[(int)Dir.Right].color = new Color(255f, 255f, 255f, active ? 255f : 0f);
        }
    }
}
