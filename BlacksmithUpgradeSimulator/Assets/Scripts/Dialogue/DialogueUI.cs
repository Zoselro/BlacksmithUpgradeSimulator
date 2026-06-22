using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("DialogueBox")]
    [SerializeField] private DialogueBoxUI dialogueBoxUI;
    [SerializeField] private Image[] imageUI;
    [SerializeField] private Animator animator;

    private Dir currentDir;

    public void Show(string name, string text, Dir dir)
    {
        dialogueBoxUI.Show(name, text, dir);
    }

    #region 이미지의 알파값을 설정하는 메서드들
    // UI 이미지의 알파값을 설정하는 메서드
    public void SetImageUIAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    // 방향에 따라 이미지를 보여주는 메서드
    public void ShowImage(Dir dir, Sprite sprite)
    {
        if (currentDir != dir)
        {
            SetImageUIAlpha(imageUI[(int)currentDir], 0f);
            currentDir = dir;
            SetImageUIAlpha(imageUI[(int)currentDir], 255f);
        }
         imageUI[(int)currentDir].sprite = sprite;
    }

    // 방향에 따라 이미지를 보여주는 메서드
    public void ShowImage(Dir dir, bool active)
    {
        if (currentDir != dir)
        {
            SetImageUIAlpha(imageUI[(int)currentDir], 0f);
            currentDir = dir;
            SetImageUIAlpha(imageUI[(int)currentDir], active ? 255f : 0f);
        }
        else
        {
            SetImageUIAlpha(imageUI[(int)currentDir], active ? 255f : 0f);
        }
    }
    #endregion

    //public void BlackSmithAppearTrigger()
    //{
    //    animator.SetTrigger("Next");
    //}

    public void NPCVisitTrigger()
    {
        animator.SetTrigger("NpcVisit");
    }

    public void NPCExitTrigger()
    {
        animator.SetTrigger("NpcExit");
    }

    //public void BlackSmithResetTrigger()
    //{
    //    animator.SetTrigger("Reset");
    //}

    public void ContentBoxFalseTrigger()
    {
        animator.SetTrigger("ContentBoxFalse");
    }

    public void ContentBoxTrueTrigger()
    {
        animator.SetTrigger("ContentBoxTrue");
    }

    public void AdjustmentTrigger()
    {
        animator.SetTrigger("Adjustment");
        dialogueBoxUI.SetEndContentBox("오늘은 이쯤 할까 ...");
    }

    //public void ShowBlackSmith(bool active, Dir dir)
    //{
    //    if (dir == Dir.Left)
    //    {
    //        imageUI[(int)Dir.Left].color = new Color(255f, 255f, 255f, active ? 255f : 0f);
    //    }
    //    else
    //    {
    //        imageUI[(int)Dir.Right].color = new Color(255f, 255f, 255f, active ? 255f : 0f);
    //    }
    //}
}
