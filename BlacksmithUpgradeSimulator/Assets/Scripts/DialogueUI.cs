using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("DialogueBox")]
    [SerializeField] private DialogueBoxUI dialogueBoxUI;
    [SerializeField] private Image[] imageUI;
    [SerializeField] private Animator animator;

    private Dir currentDir;

    //public void Show(string name, string text, Sprite sprite, Speaker speak, Dir dir)
    //{
    //    dialogueBoxUI.Show(name, text, dir);
    //}

    public void Show(string name, string text, Dir dir)
    {
        dialogueBoxUI.Show(name, text, dir);
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

    public void BlackSmithAppearTrigger()
    {
        animator.SetTrigger("Next");
    }

    public void NPCVisitTrigger()
    {
        animator.SetTrigger("NpcVisit");
    }

    public void NPCExit()
    {
        animator.SetTrigger("NpcExit");
    }

    public void BlackSmithResetTrigger()
    {
        animator.SetTrigger("Reset");
    }

    public void BlackSmithEntranceTrigger()
    {
        animator.SetTrigger("Entrance");
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

    public void ContentBoxFalseTrigger()
    {
        animator.SetTrigger("ContentBoxFalse");
    }

    public void ContentBoxTrueTrigger()
    {
        animator.SetTrigger("ContentBoxTrue");
    }
}
