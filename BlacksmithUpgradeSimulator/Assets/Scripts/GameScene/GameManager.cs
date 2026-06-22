using UnityEngine;
// 감정 상태를 나타내는 enum
public enum Emotion
{
    Normal,
    Sad,
    Happy
}
public enum NpcState
{
    Enter,              // NPC 등장
    Request,  // 의뢰 수행 중
    //CompleteSuccess,     // 의뢰 완료(성공)
    //CompleteFail,        // 의뢰 완료(실패)
    ExitSuccess,        // NPC 퇴장(성공)
    ExitFail            // NPC 퇴장(실패)
}

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    private const int FIRST_DIALOGUE_NUM = 1;
    private const int SECOND_DIALOGUE_NUM = 3;
    private const int THIRD_DIALOGUE_NUM = 1;
    private const int FOURTH_DIALOGUE_NUM = 2;

    [SerializeField] private int gold;
    public int Gold => gold;

    [SerializeField] private int visitors;
    public int Visitors => visitors;
    public void SetVisitor(int visit)
    {
        visitors += visit;
    }

    string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
    string weekday = "";
    int dayindex = 0;
    public string Weekday => weekday;
    [SerializeField] private int days = 0;
    public int Days => days;
    private int successCnt = 0;
    private int greatSuccessCnt = 0;
    private int failCnt = 0;
    public int SuccessCnt => successCnt;
    public int GreatSuccessCnt => greatSuccessCnt;
    public int FailCnt => failCnt;

    private int currentGold = 0; // 정산 화면에서 보여지는 현재 골드 양
    private int currentSuccessCnt = 0; // 정산 화면에서 보여지는 현재 성공 횟수
    private int currentGreatSuccessCnt = 0; // 정산 화면에서 보여지는 현재 대 성공 횟수
    private int currentFailCnt = 0; // 정산 화면에서 보여지는 현재 실패 횟수

    public int CurrentGold => currentGold;
    public int CurrentSuccessCnt => currentSuccessCnt;
    public int CurrentGreatSuccessCnt => currentGreatSuccessCnt;
    public int CurrentFailCnt => currentFailCnt;

    [Header("Dialogue Management")]
    [SerializeField] private DialogueController dialogueController;

    [Header("Dependencies")]
    [SerializeField] private ScriptReader scriptReader;
    [SerializeField] private EnhanceChanceCalculator enhanceChanceCalculator;
    [SerializeField] private UIManager uiManager;
    public UIManager UIManager => uiManager;
    [SerializeField] private EnhanceUIManager enhanceUIManager;
    [SerializeField] private BlackSmithData blackSmithData;
    [SerializeField] private TopUIManager topUIManager;
    [SerializeField] private NpcGenerator npcGenerator;
    [SerializeField] private EnhanceManager enhanceManager;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private TypingManager typingManager;
    [SerializeField] private SettlementWindow settlementWindow;

    [Header("Options")]
    [SerializeField] private float enhanceTime = 3f;
    public float EnhanceTime => enhanceTime;
    [SerializeField] private float eventPopUpTime = 1f; // NPC 등장 팝업창이 보여지는 시간

    [Header("NPCConroller")]
    [SerializeField] private NpcController npcController;

    private float probability = 0f; // 강화 확률
    private AdventurerType[] adventurerTypeArr = 
                                    { AdventurerType.Beginner, AdventurerType.Intermediate, AdventurerType.Advanced}; // 가중치에 따라서 리스트중 Npc 타입 결정
    private AdventurerType adventurerType; // 결정된 npc 타입

    private NpcData npcData;

    DialogueSet[] dialogueSet = new DialogueSet[4];
    private DialogueLine[][] buffer = new DialogueLine[4][];

    private bool isPaused; // 게임이 일시정지 상태인지 여부를 나타내는 변수
    public bool IsPaused => isPaused;

    // --------------- 대사 변수 -----------------
    // 처음 대사
    private string dialogueOpeningData = "";
    private string dialogueWelcomPlayerData = "";
    private string dialogueVisitNpcData = "";
    private string dialogueRequestNpcData =  "";

    // 강화에 성공이였을 때
    private string dialoguePlayerSuccessData = "";
    private string dialogueNPCSuccessExitData = "";

    // 강화에 실패했을 때
    private string dialoguePlayerFailData = "";
    private string dialogueNPCFailExitData = "";

    // 강화에 대 성공 했을 때
    private string dialoguePlayerGreateSuccessData = "";

    // 무기를 건내주는 대장장이의 대사
    private string dialogueBlackSmithDeliverData = "";

    // 정산 할 때 대사
    private string dialogueClosePlayerData = "";
    // --------------- 대사 변수 -----------------

    private void Awake()
    {
        Inst = this;
    }
    private void Start()
    {
        SoundManager.Inst.PlayBGM(EBgm.Counter_music); // BGM 재생
        InitDialogueSet(); // 대화 객체 저장공간 초기화
        initializeBuffer(); // 대사 버퍼 저장공간 초기화
        UpdateDayUI(); // 일 수 갱신
        
        // 대장장이 등장
        //dialogueUI.EnterCharacter(Dir.Left); // 대장장이 등장 애니메이션 재생

        dialogueUI.BlackSmithAppearTrigger(); // 왼쪽 스프라이트 트리거 -> 게임 시작하자마자 대장장이 등장 연출

        uiManager.SetBackGround();
        // 대장장이 이미지 띄우도록 지시
        //uiManager.SetActiveImg(true, blackSmithData.BackSprite, Direction.Left)
        HandlePreEnhancementFlow(0); // 강화 하기전 흐름 처리와 첫 방문 대사 세팅

        //string[] abc = { dialogueOpeningData, dialogueWelcomPlayerData, dialogueVisitNpcData, dialogueRequestNpcData,
        //    dialoguePlayerSuccessData, dialogueNPCSuccessExitData, dialoguePlayerFailData, dialogueNPCFailExitData,
        //    dialoguePlayerGreateSuccessData, dialogueBlackSmithDeliverData, dialogueClosePlayerData};

        //typingManager.SendText(abc); // 첫 방문 대사 타이핑 시작
    }

    private void Update()
    {
        if (!enhanceManager.IsEnhancing) return;
        enhanceManager.PlayEnhance(probability, adventurerType,
                                                npcGenerator.WeaponController, gold,
                                                failCnt, successCnt,
                                                greatSuccessCnt,
                                                currentGold,
                                                currentSuccessCnt,
                                                currentGreatSuccessCnt,
                                                currentFailCnt);
    }

    public void NewDay()
    {
        dialogueUI.ShowBlackSmith(true, Dir.Left);
        dialogueUI.BlackSmithResetTrigger(); // 대장장이 리셋 트리거 -> 새로운 날이 시작하자마자 대장장이 등장 연출
        settlementWindow.ShowStartNextDayBtn(false);

        settlementWindow.ShowFadeImage(false);

        uiManager.ShowNpc(true); // NPC 활성화

        ResetGame(); // 하루가 끝났을 때, 각종 리셋 하는 함수
    }

    // 하루가 끝났을 때, 각종 리셋 하는 함수
    public void ResetGame()
    {
        visitors = 0; // 정산 화면에서 보여진 후 방문자 수 초기화
        currentFailCnt = 0; // 정산 화면에서 보여진 후 현재 실패 횟수 초기화
        currentSuccessCnt = 0; // 정산 화면에서 보여진 후 현재 성공 횟수 초기화
        currentGreatSuccessCnt = 0; // 정산 화면에서 보여진 후 현재 대 성공 횟수 초기화
        currentGold = 0; // 정산 화면에서 보여진 후 현재 골드 양 초기화

        UpdateDayUI(); // 일 수 갱신

        // 첫 번째 방문이지만, 첫 날이 아니라면,
        // 오프닝 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해준다.
        initializeBuffer();
        HandlePreEnhancementFlow(0); // 강화 하기전 흐름 처리와 첫 방문 대사 세팅
    }

    public void HandlePreEnhancementFlow(int startIndex)
    {
        npcData = SetupNpc(); // Npc 세팅

        SetPreEnhancementDialogue(); // 대사 불러오기

        // GameManager의 대사 세팅 함수에서 대사 세팅이 끝나면,
        // 대화 컨트롤러에게 대화 객체를 전달해준다.
        dialogueController.SetDialogue(SetDialogue(), startIndex);
    }


    public void UpdateDayUI()
    {
        days += 1;
        dayindex = (days - 1) % weekdays.Length;
        weekday = weekdays[dayindex];
        topUIManager.TopBarDisPlay(); // 일 수 갱신
    }
    private NpcData SetupNpc()
    {
        adventurerType = GetWeightedCustomer(); // 방문 고객수에 따라 방문 고객의 등급이 정해진다.
        npcData = npcGenerator.Setting(adventurerType); // 고객의 타입을 Generator에 알려준 후 , 타입을 갖고 그 고객의 전반적인 세팅을 한다.
        probability = enhanceChanceCalculator.GetRandomEnhanceChance(adventurerType);//NpcGenerator.AdventurerType); // 고객의 타입에 따라 강화 확률이 결정이 된다.

        return npcData;
    }

    public float GetProbability()
    {
        return probability;
    }

    public void SetProbability(float probability)
    {
        this.probability += probability;
        if(this.probability >= 1f)
        {
            this.probability = 1f;
        }
    }

    private AdventurerType GetWeightedRandom(AdventurerType[] values, float[] weights)
    {
        float rand = Random.Range(0f, 1f);
        if(rand < weights[0])
        {
            return values[0];
        }
        else if (rand < weights[0] + weights[1])
        {
            return values[1];
        }
        else
        {
            return values[2];
        }
    }

    private AdventurerType GetWeightedCustomer()
    {
        AdventurerType adventurerType;
        switch (visitors)
        {
            case 0:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 1f, 0f, 0f });
                break;
            case 1: 
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0.9f, 0.08f, 0.02f });
                break;
            case 2:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0.7f, 0.25f, 0.05f });
                break;
            case 3:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0.45f, 0.45f, 0.1f });
                break;
            case 4:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0.2f, 0.6f, 0.2f });
                break;
            case 5:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0.1f, 0.45f, 0.45f });
                break;
            case 6:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0f, 0.45f, 0.55f });
                break;
            case 7:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0f, 0.3f, 0.7f });
                break;
            default:
                adventurerType = GetWeightedRandom(adventurerTypeArr, new float[] { 0f, 0.1f, 0.9f });
                break;
        }

        return adventurerType;
    }

    public void SetGold(int amount)
    {
        gold += amount;
        if (gold <= 0)
        {
            gold = 0;
        }
    }

    public void SetSuccessCnt(int successCnt)
    {
        this.successCnt += successCnt;
        if(this.successCnt < 0)
        {
            this.successCnt = 0;
        }
    }

    public void SetGreatSuccessCnt(int greatSuccessCnt)
    {
        this.greatSuccessCnt += greatSuccessCnt;
        if(this.greatSuccessCnt < 0)
        {
            this.greatSuccessCnt = 0;
        }
    }

    public void SetFailCnt(int failCnt)
    {
        this.failCnt += failCnt;
        if(this.failCnt < 0)
        {
            this.failCnt = 0;
        }
    }

    public void SetCurrentGold(int currentGold)
    {
        this.currentGold += currentGold;
        if(this.currentGold < 0)
        {
            this.currentGold = 0;
        }
    }

    public void SetCurrentSuccessCnt(int currentSuccessCnt)
    {
        this.currentSuccessCnt += currentSuccessCnt;
        if(this.currentSuccessCnt < 0)
        {
            this.currentSuccessCnt = 0;
        }
    }

    public void SetCurrentGreatSuccessCnt(int currentGreatSuccessCnt)
    {
        this.currentGreatSuccessCnt += currentGreatSuccessCnt;
        if(this.currentGreatSuccessCnt < 0)
        {
            this.currentGreatSuccessCnt = 0;
        }
    }

    public void SetCurrentFailCnt(int currentFailCnt)
    {
        this.currentFailCnt += currentFailCnt;
        if(this.currentFailCnt < 0)
        {
            this.currentFailCnt = 0;
        }
    }


    #region NPC 퇴장 연출
    // 나가는 연출
    public void ExitAnimation()
    {
        dialogueUI.NPCExitTrigger(); // NPC 나가는 연출 트리거

        if(visitors > 7)
        {
            Debug.Log("손님 끝");
            dialogueUI.AdjustmentTrigger();
        }
    }
    #endregion

    #region 무기 정보를 확인하는 메서드
    public void StartEnhanceInteraction(WeaponController weapon)
    {
        dialogueUI.ContentBoxFalseTrigger(); // 대화 내용창 끄는 트리거
        enhanceUIManager.OnClickActiveEnhanceActivePanel(true);
        enhanceUIManager.Initialized(weapon.PrevEnhancementLevel, weapon.Sprite, weapon.GetWeaponTypeName(), weapon.GetWeaponRankName());
    }
    #endregion

    #region 무기 강화 결과후 배경 및 카운터 이미지 세팅
    public void SetBackGroundOpenCounter()
    {
        uiManager.SetBackGround(BgType.OpenCounter);
        uiManager.ShowCounterImage(true);
    }
    #endregion

    #region ------------------------------- 대사 세팅 함수 -------------------------

    private void InitDialogueSet()
    {
        for (int i = 0; i < dialogueSet.Length; i++)
        {
            dialogueSet[i] = new DialogueSet();
        }
    }
    public void initializeBuffer()
    {
        // 만약, 첫 번째 방문이라면, 대사 버퍼에 대사 객체를 새로 만들어준다.
        if (visitors <= 0 && days == 0)
        {
            Debug.Log("첫 번째 날에 첫 번째 방문입니다. 대사 버퍼에 대사 객체를 새로 만들어줍니다.");
            buffer[0] = new DialogueLine[FIRST_DIALOGUE_NUM];
            buffer[1] = new DialogueLine[SECOND_DIALOGUE_NUM];
            buffer[2] = new DialogueLine[THIRD_DIALOGUE_NUM];
            buffer[3] = new DialogueLine[FOURTH_DIALOGUE_NUM];

            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j < buffer[i].Length; j++)
                {
                    buffer[i][j] = new DialogueLine();
                }
            }
        }
        // 첫 번째 방문이지만, 첫 날이 아니라면,
        // 오프닝 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해준다.
        else if (visitors <= 0 && days > 0)
        {
            Debug.Log("첫 번째 방문이지만, 첫 날이 아닙니다. 오프닝 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해줍니다.");
            for (int i = 0; i < buffer.Length; i++)
            {
                for (int j = 0; j < buffer[i].Length; j++)
                {
                    buffer[i][j].Reset();
                }
            }
        }
        // 첫 번째 방문이 아니라면, 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해준다.
        else
        {
            Debug.Log("첫 번째 방문이 아닙니다. 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해줍니다.");
            for (int i = 1; i < buffer.Length; i++)
            {
                for (int j = 0; j < buffer[i].Length; j++)
                {
                    buffer[i][j].Reset();
                }
            }
        }
    }

    // 강화 하기전 대장장이와 NPC의 대사 세팅
    public void SetPreEnhancementDialogue()
    {
        if(visitors <= 0) // 첫 번째 방문이라면, 오프닝 대사와 정산 대사 세팅
        {
            dialogueOpeningData = scriptReader.ReadPlayer(blackSmithData.OpenID);
            dialogueClosePlayerData = scriptReader.ReadPlayer(blackSmithData.CloseID);
        }

        dialogueWelcomPlayerData = scriptReader.ReadPlayer(blackSmithData.WelcomeID);

        dialogueVisitNpcData = scriptReader.ReadNPC(npcData.GetAdventurerType().ToString(),
                                            npcData.NpcTendency.ToString(),
                                            NpcState.Enter, npcData.GetGender());

        dialogueRequestNpcData = scriptReader.ReadNPC(npcData.GetAdventurerType().ToString(),
                                                npcData.NpcTendency.ToString(),
                                                NpcState.Request, npcData.GetGender());
    }

    public DialogueSet[] SetDialogue()
    {
        if (visitors == 0)
        {
            buffer[0][0].Set(blackSmithData.NameID, dialogueOpeningData,
                                        blackSmithData.BackSprite, Speaker.BlackSmith, Dir.Left);
            dialogueSet[0].SetDialogueLines(buffer[0]);

            dialogueSet[0].SetEndFunc(() => uiManager.WelcomNextNpc("누군가가 방문 했습니다."));
        }
        else // 첫 번째 방문이 아닐 때는 오픈 대사 제외 나머지 초기화
        {
            Debug.Log("첫 번째 방문이 아닙니다. 오픈 대사를 제외한 나머지 초기화");
            initializeBuffer();
        }

        buffer[1][0].Set(blackSmithData.NameID, dialogueWelcomPlayerData,
                                        blackSmithData.BackSprite, Speaker.BlackSmith, Dir.Left);
        buffer[1][1].Set(npcData.GetNameID(), dialogueVisitNpcData,
                                    npcController.SetEmotion(Emotion.Normal), Speaker.Npc, Dir.Right);
        buffer[1][2].Set(npcData.GetNameID(), dialogueRequestNpcData,
                                    npcController.SetEmotion(Emotion.Normal), Speaker.Npc, Dir.Right);

        dialogueSet[1].SetDialogueLines(buffer[1]);

        dialogueSet[1].SetEndFunc(() =>
                            StartEnhanceInteraction(npcGenerator.WeaponController));

        return dialogueSet;
    }

    // 강화가 끝났을 때 대장장이와 NPC의 대사 세팅
    public void SetPostEnhancementDialogue(EnhanceResult result)
    {
        // 성공 했을 때
        if (EnhanceResult.Success == result)
        {
            dialoguePlayerSuccessData = scriptReader.ReadPlayer(blackSmithData.EnhanceSuccessID);
            dialogueBlackSmithDeliverData = scriptReader.ReadPlayer(blackSmithData.CompleteSuccessID);
            dialogueNPCSuccessExitData = scriptReader.ReadNPC(npcData.GetAdventurerType().ToString(),
                                                    npcData.NpcTendency.ToString(),
                                                    NpcState.ExitSuccess, npcData.GetGender());
        }
        // 대 성공 했을 때
        else if (EnhanceResult.GreatSuccess == result)
        {
            dialoguePlayerGreateSuccessData = scriptReader.ReadPlayer(blackSmithData.EnhanceGreatSuccessID);
            dialogueBlackSmithDeliverData = scriptReader.ReadPlayer(blackSmithData.CompleteSuccessID);
            dialogueNPCSuccessExitData = scriptReader.ReadNPC(npcData.GetAdventurerType().ToString(),
                                                    npcData.NpcTendency.ToString(),
                                                    NpcState.ExitSuccess, npcData.GetGender());
        }
        // 실패 했을 때
        else
        {
            dialoguePlayerFailData = scriptReader.ReadPlayer(blackSmithData.EnhanceFailID);
            dialogueBlackSmithDeliverData = scriptReader.ReadPlayer(blackSmithData.CompleteFailID);
            dialogueNPCFailExitData = scriptReader.ReadNPC(npcData.GetAdventurerType().ToString(),
                                                    npcData.NpcTendency.ToString(),
                                                    NpcState.ExitFail, npcData.GetGender());
        }
    }

   // 강화 결과에 따라서 마지막 대화 객체에 대사를 세팅
    public void OnEnhanceResult(EnhanceResult result)
    {
        Debug.Log($"state : {result}");

        if (result == EnhanceResult.Success)
        {
            buffer[2][0].Set(blackSmithData.NameID, dialoguePlayerSuccessData,
                                    blackSmithData.HappySprite, Speaker.BlackSmith, Dir.Right);
            buffer[3][0].Set(blackSmithData.NameID, dialogueBlackSmithDeliverData,
                                    blackSmithData.BackSprite, Speaker.BlackSmith, Dir.Left);
            buffer[3][1].Set(npcData.GetNameID(), dialogueNPCSuccessExitData,
                                    npcController.SetEmotion(Emotion.Happy), Speaker.Npc, Dir.Right);
        }
        else if(result == EnhanceResult.GreatSuccess)
        {
            buffer[2][0].Set(blackSmithData.NameID, dialoguePlayerGreateSuccessData,
                                    blackSmithData.HappySprite, Speaker.BlackSmith, Dir.Right);
            buffer[3][0].Set(blackSmithData.NameID, dialogueBlackSmithDeliverData,
                                    blackSmithData.BackSprite, Speaker.BlackSmith, Dir.Left);
            buffer[3][1].Set(npcData.GetNameID(), dialogueNPCSuccessExitData,
                                    npcController.SetEmotion(Emotion.Happy), Speaker.Npc, Dir.Right);
        }
        else
        {
            buffer[2][0].Set(blackSmithData.NameID, dialoguePlayerFailData,
                                    blackSmithData.SadSprite, Speaker.BlackSmith, Dir.Right);
            buffer[3][0].Set(blackSmithData.NameID, dialogueBlackSmithDeliverData,
                                    blackSmithData.BackSprite, Speaker.BlackSmith, Dir.Left);
            buffer[3][1].Set(npcData.GetNameID(), dialogueNPCFailExitData,
                                    npcController.SetEmotion(Emotion.Sad), Speaker.Npc, Dir.Right);
        }
        dialogueSet[2].SetDialogueLines(buffer[2]);
        dialogueSet[2].SetEndFunc(() => SetBackGroundOpenCounter());

        dialogueSet[3].SetDialogueLines(buffer[3]);

        dialogueSet[3].SetEndFunc(() => ExitAnimation());
    }
    #endregion

    // 게임이 일시정지가 되는 함수
    //public void TryEnhance(bool active)
    //{
    //    if (active)
    //    {
    //        Debug.Log("강화 중단");
    //        enhanceManager.RequestEnhance(false); // 강화 중단 요청
    //    }
    //    else
    //    {
    //        Debug.Log("강화 시작");
    //        enhanceManager.RequestEnhance(true); // 강화 시작 요청
    //    }
    //}

    //public void TryAnimation(bool active)
    //{
    //    if (active)
    //    {
    //        uiManager.StopAnimator();
    //        Debug.Log("애니메이션 중단");
    //    }
    //    else
    //    {
    //        uiManager.StartAnimator();
    //        Debug.Log("애니메이션 재개");
    //    }
    //}
}
