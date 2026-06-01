using UnityEngine;

public class StartNextDayButton : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Animator animator;
    public void OnStartNextDayButton()
    {
        animator.SetTrigger("StartTrigger");
        uiManager.ShowStartNextDayText(true, "다시 하루가 밝았다.");
    }
}
