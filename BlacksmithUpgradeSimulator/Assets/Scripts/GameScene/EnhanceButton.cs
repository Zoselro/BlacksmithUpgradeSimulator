using UnityEngine;

public class EnhanceButton : MonoBehaviour
{
    [SerializeField] private EnhanceUIManager enhanceUIManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private TopUIManager topUIManager;
    [SerializeField] private GameManager gm;
    [SerializeField] private EnhanceManager enhanceManager;
    [SerializeField] private DialogueController dialogueController;
    public void EnhanceButtonOnClick()
    {
        enhanceManager.RequestEnhance(true); // 강화 시작 요청

        enhanceUIManager.EnhanceActivePanel(false);

        //enhanceUIManager.SetActiveMiniGame(true); 미니게임
        enhanceUIManager.ActiveEnhanceUIBar(true, (gm.GetProbability() * 100).ToString("F1"));
        uiManager.SetBackGround(BgType.Enhance);
        topUIManager.ActiveTopUIPanel(false);
        //uiManager.ShowSprite(false, Speaker.Npc);
        //uiManager.ShowSprite(false, Speaker.BlackSmith);
        uiManager.ShowBlackSmith(false, Dir.Left);
        uiManager.ShowNpc(false);
        uiManager.ShowCounterImage(false);
    }

    public void ConfirmButtonOnClick()
    {
        uiManager.SetBackGround(BgType.Blacksmith);
        topUIManager.ActiveTopUIPanel(true);
        enhanceUIManager.ActiveEnhanceUIBar(false);
        uiManager.ShowBlackSmith(true, Dir.Right);
        dialogueController.OnClickNextBtn(); // 강화 이후 대사 실행

        enhanceUIManager.ActiveConfirmButton(false);
        uiManager.ShowDialogueBox(true);
        enhanceManager.EnhancementImageActive(false); // EnhancementImage를 끄도록 요청
    }
}
