using UnityEngine;

public class EnhanceButton : MonoBehaviour
{
    [SerializeField] private EnhanceUIManager enhanceUIManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TopUIManager topUIManager;
    [SerializeField] private GameManager gm;
    [SerializeField] private EnhanceManager enhanceManager;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private DialogueUI dialogueUI;

    public void EnhanceButtonOnClick()
    {
        enhanceManager.RequestEnhance(true); // 강화 시작 요청
        SoundManager.Inst.PlaySFX(ESfx.Button_Click);
        enhanceUIManager.EnhanceActivePanel(false);

        //enhanceUIManager.SetActiveMiniGame(true); 미니게임
        enhanceUIManager.ActiveEnhanceUIBar(true, (gm.GetProbability() * 100).ToString("F1"));
        uiManager.SetBackGround(BgType.Enhance);
        topUIManager.ActiveTopUIPanel(false);
        dialogueUI.ShowBlackSmith(false, Dir.Left);
        uiManager.ShowNpc(false);
        uiManager.ShowCounterImage(false);
    }

    public void ConfirmButtonOnClick()
    {
        uiManager.SetBackGround(BgType.Blacksmith);
        topUIManager.ActiveTopUIPanel(true);
        enhanceUIManager.ActiveEnhanceUIBar(false);
        dialogueController.OnClickNextBtn(); // 강화 이후 대사 실행
        dialogueUI.ContentBoxTrueTrigger();
        enhanceUIManager.ActiveConfirmButton(false);
        enhanceManager.EnhancementImageActive(false); // EnhancementImage를 끄도록 요청
    }
}
