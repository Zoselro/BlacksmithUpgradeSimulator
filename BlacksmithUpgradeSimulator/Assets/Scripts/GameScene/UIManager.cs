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
public enum Direction
{
    Left,
    Right
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
    [SerializeField] private GameObject settlementWindow;
    [SerializeField] private TextMeshProUGUI startNextDayText;

    [Header("CharactorSprite")] 
    [SerializeField] private Image rightSprite;
    [SerializeField] private Image leftSprite;
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
    [SerializeField] private Animator animator;

    Dictionary<BgType, Sprite> bgDictionary = new Dictionary<BgType, Sprite>();

    // 왼쪽 스프라이트 활성화 여부 설정 메서드
    public void ShowleftSprite(bool active)
    {
        leftSprite.gameObject.SetActive(active);
    }

    // 오른쪽 스프라이트 활성화 여부 설정 메서드
    public void ShowRightSprite(bool active)
    {
        rightSprite.gameObject.SetActive(active);
    }

    public void ShowDialogueBox2(bool active)
    {
        dialogueBox2.gameObject.SetActive(active);
    }

    // 이미지와 활성화 여부를 동시에 설정하는 메서드
    public void ShowSprite(bool active, Sprite sprite, Direction dir)
    {
        if (dir == Direction.Left)
        {
            leftSprite.sprite = sprite;
            leftSprite.gameObject.SetActive(active);
        }
        else
        {
            rightSprite.sprite = sprite;
            rightSprite.gameObject.SetActive(active);
        }
    }

    // 이미지 활성화 여부만 설정하는 메서드
    public void ShowSprite(bool active, Direction dir)
    {
        if (dir == Direction.Left)
        {
            leftSprite.gameObject.SetActive(active);
        }
        else
        {
            rightSprite.gameObject.SetActive(active);

            // "Visit" 상태를 0번 레이어에서 재생하되, 
            // 진행도를 1.0f(100%)로 설정해서 마지막 위치에 고정시킵니다.
            animator.Play("Visit", 0, 1.0f);
        }
    }

    // NPC 방문 트리거 메서드
    public void NPCVisitTrigger()
    {
        animator.SetTrigger("Visit");
    }

    public void NPCExit(bool state)
    {
        animator.SetBool("Exit", state);
    }

    // 이름, 대사, 이미지, 방향, 활성화 여부를 동시에 설정하는 메서드
    public void OutPutSprite(string name, string text, Sprite sprite, Direction dir, bool active)
    {
        npcName.text = name;
        content.text = text;
        if(dir == Direction.Left)
        {
            leftSprite.sprite = sprite;
            leftSprite.gameObject.SetActive(active);
            leftContentBox.gameObject.SetActive(active);
            rightContentBox.gameObject.SetActive(!active);
        }
        else
        {
            rightSprite.sprite = sprite;
            rightSprite.gameObject.SetActive(active);
            rightContentBox.gameObject.SetActive(active);
            leftContentBox.gameObject.SetActive(!active);
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

    // 정산 창 활성화 여부 설정 메서드
    //public void ShowSettlementWindow(bool active)
    //{
    //    settlementWindow.gameObject.SetActive(active);

    //}

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
        settlementWindow.gameObject.SetActive(active);
    }

    public void ShowFadeImage(bool active)
    {
        fadeImage.gameObject.SetActive(active);
        dayCnt.text = gm.Days.ToString() + " 일차 정산 일지";
        successCnt.text = gm.CurrentSuccessCnt.ToString();
        greatSuccessCnt.text = gm.CurrentGreatSuccessCnt.ToString();
        failCnt.text = gm.CurrentFailCnt.ToString();
        goldCnt.text = gm.Gold.ToString();
    }

    public void ShowStartNextDayText(bool active, string startNextDayText)
    {
        this.startNextDayText.gameObject.SetActive(active);
        this.startNextDayText.text = startNextDayText;
        Debug.Log("실행");
    }
}
