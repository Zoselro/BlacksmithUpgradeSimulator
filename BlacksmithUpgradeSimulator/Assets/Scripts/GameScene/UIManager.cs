using System.Collections;
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
    [SerializeField] private GameManager gm;

    [Header("DialogueBox")]
    [SerializeField] private GameObject namePanel;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI npcName;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private Image leftContentBox;
    [SerializeField] private Image rightContentBox;
    [SerializeField] private GameObject dialogueBox2;

    [Header("SettlementWindow")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private TextMeshProUGUI dayCnt;
    [SerializeField] private TextMeshProUGUI successCnt;
    [SerializeField] private TextMeshProUGUI greatSuccessCnt;
    [SerializeField] private TextMeshProUGUI failCnt;
    [SerializeField] private TextMeshProUGUI goldCnt;
    [SerializeField] private Button confirmButton;
    [SerializeField] private GameObject settlementWindowObj;
    [SerializeField] private GameObject settlementWindow;
    [SerializeField] private GameObject startNextDayTextObj;
    [SerializeField] private TextMeshProUGUI startNextDayText;

    [Header("CharactorSprite")] 
    [SerializeField] private CanvasGroup npcCanvasGroup;
    [SerializeField] private Image leftBlackSmithSprite;
    [SerializeField] private Image rightBlackSmithSprite;
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
    [SerializeField] private Animator rightImageAnimator;
    [SerializeField] private Animator leftImageAnimator;
    [SerializeField] private Animator welcomPopupAnimator;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private Animator startDayTextAnimator;

    Dictionary<BgType, Sprite> bgDictionary = new Dictionary<BgType, Sprite>();

    // 왼쪽 스프라이트 활성화 여부 설정 메서드
    public void ShowBlackSmith(bool active, Dir dir)
    {
        if(dir == Dir.Left)
        {
            leftBlackSmithSprite.gameObject.SetActive(active);
            rightBlackSmithSprite.gameObject.SetActive(false);
        }
        else
        {
            rightBlackSmithSprite.gameObject.SetActive(active);
            leftBlackSmithSprite.gameObject.SetActive(false);
        }
    }

    // 오른쪽 스프라이트 활성화 여부 설정 메서드
    public void ShowNpc(bool active)
    {
        npcCanvasGroup.alpha = active ? 1f : 0f; // 활성화 여부에 따라 투명도 조절
    }

    public void PlayWelcomePopupAnimation()
    {
        welcomPopupAnimator.SetTrigger("Play");
    }

    public void BlackSmithEntranceTrigger()
    {
        rightImageAnimator.Play("Idle", 0, 0f);
        leftImageAnimator.SetTrigger("Entrance");
    }

    public void BlackSmithResetTrigger()
    {
        leftImageAnimator.SetTrigger("Reset");
    }

    public void ShowDialogueBox2(bool active)
    {
        dialogueBox2.gameObject.SetActive(active);
    }


    // NPC 방문 트리거 메서드
    public void NPCVisitTrigger()
    {
        rightImageAnimator.SetTrigger("Visit");
    }

    public void NPCExit(bool state)
    {
        rightImageAnimator.SetBool("Exit", state);
    }

    // 이름, 대사, 이미지, 방향, 활성화 여부를 동시에 설정하는 메서드
    public void OutPutSprite(string name, string text, Sprite sprite, Speaker speak, Dir dir)
    {
        npcName.text = name;
        content.text = text;

        if(dir == Dir.Left)
        {
            if(speak == Speaker.BlackSmith)
            {
                Debug.Log($"왼쪽 말하는 주체 대장장이");
                leftBlackSmithSprite.sprite = sprite;
                leftBlackSmithSprite.gameObject.SetActive(true);
                rightBlackSmithSprite.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"왼쪽 말하는 주체 NPC");
                ShowNpc(true);
            }
            leftContentBox.gameObject.SetActive(true);
            rightContentBox.gameObject.SetActive(false);
        }
        else
        {
            if (speak == Speaker.BlackSmith)
            {
                Debug.Log($"오른쪽 말하는 주체 대장장이");
                rightBlackSmithSprite.sprite = sprite;
                rightBlackSmithSprite.gameObject.SetActive(true);
                leftBlackSmithSprite.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log($"오른쪽 말하는 주체 NPC");
                ShowNpc(true);
            }
            rightContentBox.gameObject.SetActive(true);
            leftContentBox.gameObject.SetActive(false);
        }
    }


    // 이름과 대사, 활성화 여부를 설정하는 메서드
    public void PlayWelcomeSequence(bool active, string text)
    {
        welcomText.text = text;
        welcomImg.gameObject.SetActive(active);
        dialogueBox.gameObject.SetActive(!active);
    }

    // 이름과 대사, 활성화 여부를 설정하는 메서드
    public void PlayWelcomeSequence(bool active)
    {
        welcomImg.gameObject.SetActive(active);
        dialogueBox.gameObject.SetActive(!active);
    }

    // 이름과 대사 활성화 여부를 설정하는 메서드
    public void ShowDialogueBox(bool active)
    {
        dialogueBox.gameObject.SetActive(active);
    }

    // 카운터 이미지 활성화 여부 설정 메서드
    public void ShowCounterImage(bool active)
    {
        counterImg.gameObject.SetActive(active);
    }

    // 다음 버튼 활성화 여부 설정 메서드
    public void ShowNextBtn(bool active)
    {
        nextBtn.gameObject.SetActive(active);
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

    public void ShowSettlementWindow(bool active)
    {
        settlementWindowObj.gameObject.SetActive(active);
        settlementWindow.gameObject.SetActive(active);
    }

    public void ShowFadeImage(bool active)
    {
        fadeImage.gameObject.SetActive(active);
        dayCnt.text = gm.Days.ToString() + " 일차 정산 일지";
        successCnt.text = gm.CurrentSuccessCnt.ToString();
        greatSuccessCnt.text = gm.CurrentGreatSuccessCnt.ToString();
        failCnt.text = gm.CurrentFailCnt.ToString();
        goldCnt.text = gm.CurrentGold.ToString();
    }

    public void ShowStartNextDayText(bool active, string startNextDayText)
    {
        startNextDayTextObj.SetActive(active);
        this.startNextDayText.text = startNextDayText;
    }

    public void ShowStartNextDayBtn(bool active)
    {
        startNextDayTextObj.gameObject.SetActive(active);
    }

    // 애니메이터의 속도를 0으로 설정하여 애니메이션을 멈추는 메서드
    public void StopAnimator()
    {
        rightImageAnimator.speed = 0f;
        leftImageAnimator.speed = 0f;
        welcomPopupAnimator.speed = 0f;
        fadeAnimator.speed = 0f;
        startDayTextAnimator.speed = 0f;
    }

    // 애니메이터의 속도를 1로 설정하여 애니메이션을 재생하는 메서드
    public void StartAnimator()
    {
        rightImageAnimator.speed = 1f;
        leftImageAnimator.speed = 1f;
        welcomPopupAnimator.speed = 1f;
        fadeAnimator.speed = 1f;
        startDayTextAnimator.speed = 1f;
    }
}
