using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BgType
{
    CloseCounter, // 닫혀 있을 때 카운터
    OpenCounter, // 열려있을 때 카운터
    Enhance, // 작업실
    Blacksmith // 대장간
}

public class UIManager : MonoBehaviour
{
    [Header("DialogueBox")]
    [SerializeField] DialogueUI dialogueUI;

    [Header("SettlementWindow")]
    [SerializeField] SettlementWindow settlementWindow;

    [Header("TopUI")]
    [SerializeField] TopUIManager topUIManager;

    [Header("CharactorSprite")] 
    [SerializeField] private CanvasGroup npcCanvasGroup;
    [SerializeField] private TextMeshProUGUI welcomText;

    [Header("CounterImg")]
    [SerializeField] private GameObject counterImg;

    [Header("BackGround")]
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite closeCounterImg;
    [SerializeField] private Sprite openCounterImg;
    [SerializeField] private Sprite enhanceImg;
    [SerializeField] private Sprite blacksmithBackground;


    Dictionary<BgType, Sprite> bgDictionary = new Dictionary<BgType, Sprite>();

    public void Adjustment()
    {
        settlementWindow.Adjustment();
    }

    // NPC 스프라이트 활성화 여부 설정 메서드
    public void ShowPrefab(bool active)
    {
        npcCanvasGroup.alpha = active ? 1f : 0f; // 활성화 여부에 따라 투명도 조절
    }

    // 이름과 대사, 활성화 여부를 설정하는 메서드
    public void WelcomNextNpc(string text)
    {
        welcomText.text = text;
        dialogueUI.NPCVisitTrigger();
    }

    // 카운터 이미지 활성화 여부 설정 메서드
    public void ShowCounterImage(bool active)
    {
        counterImg.SetActive(active);
    }

    // 키 값에 따라 Value를 얻는 형식으로 구현
    public void SetBackGround()
    {
        bgDictionary.Add(BgType.OpenCounter, openCounterImg);
        bgDictionary.Add(BgType.CloseCounter, closeCounterImg);
        bgDictionary.Add(BgType.Enhance, enhanceImg);
        bgDictionary.Add(BgType.Blacksmith, blacksmithBackground);
    }

    // 배경 이미지 설정 메서드
    public void SetBackGround(BgType bgType)
    {
        backGround.sprite = bgDictionary[bgType];
    }

    public void SetBackGroundCloseCounter()
    {
        backGround.sprite = closeCounterImg;
    }

    public void SetBackGroundOpenCounter()
    {
        GameManager.Inst.HandlePreEnhancementFlow(1);
        GameManager.Inst.SetVisitor(1);
        topUIManager.TopBarDisPlay(); // 방문자 수 갱신
        backGround.sprite = openCounterImg;
        SoundManager.Inst.PlaySFX(ESfx.Bell);
    }
    // 애니메이터의 속도를 0으로 설정하여 애니메이션을 멈추는 메서드
    public void StopAnimator()
    {
        //npcAnimator.speed = 0f;
        //leftImageAnimator.speed = 0f;
        //welcomPopupAnimator.speed = 0f;
        settlementWindow.FadeAnimatorSpeed(0f);
    }

    // 애니메이터의 속도를 1로 설정하여 애니메이션을 재생하는 메서드
    public void StartAnimator()
    {
        //npcAnimator.speed = 1f;
        //leftImageAnimator.speed = 1f;
        //welcomPopupAnimator.speed = 1f;
        settlementWindow.FadeAnimatorSpeed(1f);
    }
}
