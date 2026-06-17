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
    [SerializeField] DialogueBoxUI dialogueBoxUI;

    [Header("SettlementWindow")]
    [SerializeField] SettlementWindow settlementWindow;

    [Header("CharactorSprite")] 
    [SerializeField] private CanvasGroup npcCanvasGroup;
    [SerializeField] private Image welcomImg;
    [SerializeField] private TextMeshProUGUI welcomText;

    [Header("CounterImg")]
    [SerializeField] private Image counterImg;

    [Header("BackGround")]
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite closeCounterImg;
    [SerializeField] private Sprite openCounterImg;
    [SerializeField] private Sprite enhanceImg;
    [SerializeField] private Sprite blacksmithBackground;

    [Header("Animator")]
    [SerializeField] private Animator npcAnimator;
    [SerializeField] private Animator leftImageAnimator;
    [SerializeField] private Animator welcomPopupAnimator;

    Dictionary<BgType, Sprite> bgDictionary = new Dictionary<BgType, Sprite>();

    // NPC 스프라이트 활성화 여부 설정 메서드
    public void ShowNpc(bool active)
    {
        npcCanvasGroup.alpha = active ? 1f : 0f; // 활성화 여부에 따라 투명도 조절
    }

    private void BlackSmithEnter() // animator
    {
        //dialogueUI.EnterCharacter(Dir.Left);
    }

    private void ShowDialgoue()
    {

    }

    public void BlackSmithEntranceTrigger()
    {
        npcAnimator.Play("Idle", 0, 0f);
        leftImageAnimator.SetTrigger("Entrance");
    }

    public void BlackSmithResetTrigger()
    {
        leftImageAnimator.SetTrigger("Reset");
    }


    // NPC 방문 트리거 메서드
    public void NPCVisitTrigger()
    {
        npcAnimator.SetTrigger("Visit");
    }

    public void NPCExit(bool state)
    {
        npcAnimator.SetBool("Exit", state);
        //npcAnimator.SetTrigger("Exit");
    }

    // 이름과 대사, 활성화 여부를 설정하는 메서드
    public void PlayWelcomeSequence(bool active, string text)
    {
        welcomText.text = text;
        welcomImg.gameObject.SetActive(active);
        dialogueBoxUI.ShowContentBox(!active);
    }

    // 이름과 대사, 활성화 여부를 설정하는 메서드
    public void PlayWelcomeSequence(bool active)
    {
        welcomImg.gameObject.SetActive(active);
        dialogueBoxUI.ShowContentBox(!active);
    }

    // 카운터 이미지 활성화 여부 설정 메서드
    public void ShowCounterImage(bool active)
    {
        counterImg.gameObject.SetActive(active);
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


    // 애니메이터의 속도를 0으로 설정하여 애니메이션을 멈추는 메서드
    public void StopAnimator()
    {
        npcAnimator.speed = 0f;
        leftImageAnimator.speed = 0f;
        welcomPopupAnimator.speed = 0f;
        settlementWindow.FadeAnimatorSpeed(0f);
    }

    // 애니메이터의 속도를 1로 설정하여 애니메이션을 재생하는 메서드
    public void StartAnimator()
    {
        npcAnimator.speed = 1f;
        leftImageAnimator.speed = 1f;
        welcomPopupAnimator.speed = 1f;
        settlementWindow.FadeAnimatorSpeed(1f);
    }
}
