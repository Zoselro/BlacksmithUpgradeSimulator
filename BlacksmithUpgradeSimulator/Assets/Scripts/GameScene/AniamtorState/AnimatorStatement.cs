using UnityEngine;

public class AnimatorStatement : StateMachineBehaviour
{
    [SerializeField] AnimatorState animatorState;
    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        switch (animatorState)
        {
            case AnimatorState.Open:
                animator.SetTrigger("ShowContent");
                break;
            case AnimatorState.OnContent:
                GameManager.Inst.DialogueBoxUI.ShowContentBox(true);
                break;
            case AnimatorState.NpcVisit:
                animator.SetTrigger("WelcomeSequence");
                break;
            case AnimatorState.NpcWelcomeSequence:
                GameManager.Inst.UIManager.PlayWelcomeSequence(false);
                break;
            case AnimatorState.NpcExit:
                if (GameManager.Inst.Visitors < 8)
                    animator.SetTrigger("WelcomEvent");
                else
                {
                    animator.SetTrigger("Close");
                    Debug.Log("손님 퇴장 후 이벤트 발생");
                }
                break;
            case AnimatorState.NpcWelcomEvent:
                GameManager.Inst.WelcomeAnimation();
                break;
            case AnimatorState.Close:
                animator.SetTrigger("Close");
                GameManager.Inst.HandleEndOfDay();
                break;
            case AnimatorState.NewDay:
                animator.SetTrigger("NewDay");
                GameManager.Inst.NewDay();
                break;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("끝난 후 : " + animatorState);
    }
}
