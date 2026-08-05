using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    [Header("DialogueBox")]
    [SerializeField] private DialogueBoxUI dialogueBoxUI;
    [SerializeField] private Image[] imageUI;
    [SerializeField] private Animator animator;
    [SerializeField] private BlackSmithData blackSmithData;
    [SerializeField] private UIManager uiManager;

    private Dir currentDir;

    public void Show(string name, string text, Dir dir)
    {
        dialogueBoxUI.Show(name, text, dir);
    }

    #region 이미지를 설정하는 메서드들
    // UI 이미지의 알파값을 설정하는 메서드
    public void SetImageUIAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    // 방향에 따라 이미지를 바꿔주는 메서드
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

    // 방향에 따라 이미지를 바꾸는 메서드

    public void ChangeSprite(Dir dir, Sprite sprite)
    {
        if(currentDir == dir)
        {
            imageUI[(int)currentDir].sprite = sprite;
        }
        else
        {
            currentDir = dir;
            imageUI[(int)currentDir].sprite = sprite;
        }
    }
    #endregion

    public void NextTrigger()
    {
        animator.SetTrigger("NextTrigger");
    }

    public void AdjustmentTrigger()
    {
        animator.SetTrigger("Adjustment");
        dialogueBoxUI.SetEndContentBox(gm.DialogueClosePlayerData);
    }

    public void PlayAniamtion(string animation)
    {
        animator.Play(animation);
    }
}
