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
                GameManager.Inst.UIManager.ShowDialogueBox(true);
                break;
            case AnimatorState.NpcVisit:
                animator.SetTrigger("WelcomeSequence");
                break;
            case AnimatorState.NpcWelcomeSequence:
                GameManager.Inst.UIManager.PlayWelcomeSequence(false);
                break;
            case AnimatorState.NpcExit:
                animator.SetTrigger("WelcomEvent");
                break;
            case AnimatorState.NpcWelcomEvent:
                GameManager.Inst.WelcomeAnimation();
                break;
        }
    }
}
