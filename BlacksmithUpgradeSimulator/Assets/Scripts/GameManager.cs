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
    private const int FIRST_DIALOGUE_NUM = 1;
    private const int SECOND_DIALOGUE_NUM = 3;
    private const int THIRD_DIALOGUE_NUM = 1;
    private const int FOURTH_DIALOGUE_NUM = 2;

    //[SerializeField] private int gold;
    //public int Gold => gold;

    //[SerializeField] private int visitors;
    //public int Visitors => visitors;
    //public void SetVisitor(int visit)
    //{
    //    visitors += visit;
    //}

    string[] weekdays = { "월", "화", "수", "목", "금", "토", "일" };
    string weekday = "";
    int dayindex = 0;
    public string Weekday => weekday;

    [Header("Dialogue Management")]
    [SerializeField] private DialogueController dialogueController;

    [Header("Dependencies")]
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
    [SerializeField] private GameDataManager gameDataManager;

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

    private float[] weight = new float[] { 0f, 0f, 0f }; // 가중치에 따라서 리스트중 Npc 타입 결정

    private bool isPaused; // 게임이 일시정지 상태인지 여부를 나타내는 변수
    public bool IsPaused => isPaused;

    private bool isLoadedFromSave; // 이어하기 여부를 저장하는 변수

    // --------------- 대사 변수 -----------------
    [Header("ScriptReader")]
    [SerializeField] private ScriptReader scriptReader;

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
    public string DialogueClosePlayerData => dialogueClosePlayerData; // 정산 할 때 대사
    // --------------- 대사 변수 -----------------

    private void Awake()
    {
        CreateBuffer(); // 대화 객체 저장공간 초기화
        scriptReader.LoadDialogueData(); // 대사 데이터 로드
        isLoadedFromSave = gameDataManager.GetVisitors() > 0; // 방문자 수가 1명 이상이면 이어하기로 판단
        
        // 강화를 끝내고 난 후, 게임을 종료했다면, 방문자 수를 증가시키는 로직을 추가
        if (gameDataManager.GetGameStatus() == GameStatus.Enhanced && gameDataManager.GetVisitors() < 8)
        {
            gameDataManager.SetVisitors(gameDataManager.GetVisitors() + 1); // 방문자 수 증가
            gameDataManager.SetGameStatus(GameStatus.NoEnhance);
        }
        else if (gameDataManager.GetGameStatus() == GameStatus.Enhancing) // 강화진행도중 게임을 껏다면 다시 데이터를 롤백
        {
            Debug.Log("데이터 롤백!");
            gameDataManager.RollbackGameData(); // 게임 데이터 롤백
        }
    }
    private void Start()
    {
        npcGenerator.InitializeNpcDataMap(); // npcGenerator의 npcDataMap 초기화
        SoundManager.Inst.PlayBGM(EBgm.Counter_music); // BGM 재생
        InitDialogueSet(); // 대화 객체 저장공간 초기화
        initializeBuffer(); // 대사 버퍼 저장공간 초기화

        int dayindex = 0;
        dayindex = (gameDataManager.GetDay() - 1) % weekdays.Length; // 저장되어 있는 일 수를 불러온 후 요일 인덱스 계산
        weekday = weekdays[dayindex]; // 계산된 요일 인덱스를 기반으로 요일 문자열 세팅

        topUIManager.TopBarDisPlay(); // 일 수 갱신

        topUIManager.SetGoldText(gameDataManager.GetGold()); // 골드 갱신

        enhanceManager.BuildEnhanceResultPrefabMap(); // 강화 결과 prefab 세팅

        uiManager.SetBackGround(); // 배경 세팅

        int visitors = gameDataManager.GetVisitors();
        GameStatus status = gameDataManager.GetGameStatus();

        if (visitors <= 0)
        {
            HandlePreEnhancementFlow(0); // 방문자 수가 0명이면, 첫 방문 대사 세팅
            gameDataManager.SetGameStatus(GameStatus.NoEnhance); // 첫 방문이므로 강화 전 상태로 세팅
            dialogueUI.PlayAniamtion("BlackSmithEnter");
        }
        else if (visitors <= 7) // 방문자 수가 9명 미만이면, 첫 방문 대사 제외 나머지 세팅
        {
            HandlePreEnhancementFlow(1); // 방문자 수가 0명이 아니면, 첫 방문 대사 제외 나머지 세팅
            gameDataManager.SetGameStatus(GameStatus.NoEnhance); // Npc가 방문했으므로 강화 전 상태로 세팅
            dialogueUI.PlayAniamtion("WelcomNpc");
        }
        else if (visitors >= 8)
        {
            if (status == GameStatus.Enhanced)
            {
                dialogueUI.PlayAniamtion("Adjustment");
            }
            else
            {
                HandlePreEnhancementFlow(1);
                gameDataManager.SetGameStatus(GameStatus.NoEnhance); // Npc가 8명이고, 강화가 끝나지 않은 상태라면, 강화 전 상태로 세팅
                dialogueUI.PlayAniamtion("WelcomNpc");
            }
        }
    }

    private void Update()
    {
        Debug.Log($"status : {gameDataManager.GetGameStatus()}");
        Debug.Log($"isLoadedFromSave : {isLoadedFromSave}");
        if (!enhanceManager.IsEnhancing) return;
        enhanceManager.PlayEnhance(probability, adventurerType,
                                                npcGenerator.WeaponController, gameDataManager.GetGold(),
                                                gameDataManager.GetFailCount(),gameDataManager.GetSuccessCount(),
                                                gameDataManager.GetGreatSuccessCount(),
                                                gameDataManager.GetCurrentGold(),
                                                gameDataManager.GetCurrentSuccessCount(),
                                                gameDataManager.GetCurrentGreatSuccessCount(),
                                                gameDataManager.GetCurrentFailCount());

    }

    public void HandlePreEnhancementFlow(int startIndex)
    {
        npcData = SetupNpc(); // Npc 세팅

        SetPreEnhancementDialogue(); // 대사 불러오기

        // GameManager의 대사 세팅 함수에서 대사 세팅이 끝나면,
        // 대화 컨트롤러에게 대화 객체를 전달해준다.

        dialogueController.SetDialogue(SetDialogue(), startIndex);
    }

    public void NextDay()
    {
        //days += 1;
        gameDataManager.SetDay(gameDataManager.GetDay() + 1);
        dayindex = (gameDataManager.GetDay() - 1) % weekdays.Length;
        weekday = weekdays[dayindex];
        topUIManager.TopBarDisPlay(); // 일 수 갱신
    }
    private NpcData SetupNpc()
    {
        adventurerType = GetWeightedCustomer(); // 방문 고객수에 따라 방문 고객의 등급이 정해진다.
        npcData = npcGenerator.Setting(adventurerType); // 고객의 타입을 Generator에 알려준 후 , 타입을 갖고 그 고객의 전반적인 세팅을 한다.

        probability = enhanceChanceCalculator.GetEnhanceProbablity(npcGenerator.EnhancemmentLevel); // 무기 강화에 따라 강화 확률이 결정이 된다.

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
        switch (gameDataManager.GetVisitors())
        {
            case 0:
                weight[0] = 1f; weight[1] = 0f; weight[2] = 0f;
                break;
            case 1:
                weight[0] = 0.9f; weight[1] = 0.08f; weight[2] = 0.02f;
                break;
            case 2:
                weight[0] = 0.7f; weight[1] = 0.25f; weight[2] = 0.05f;
                break;
            case 3:
                weight[0] = 0.45f; weight[1] = 0.45f; weight[2] = 0.1f;
                break;
            case 4:
                weight[0] = 0.2f; weight[1] = 0.6f; weight[2] = 0.2f;
                break;
            case 5:
                weight[0] = 0.1f; weight[1] = 0.45f; weight[2] = 0.45f;
                break;
            case 6:
                weight[0] = 0f; weight[1] = 0.45f; weight[2] = 0.55f;
                break;
            case 7:
                weight[0] = 0f; weight[1] = 0.3f; weight[2] = 0.7f;
                break;
            default:
                weight[0] = 0f; weight[1] = 0.1f; weight[2] = 0.9f;
                break;
        }

        return GetWeightedRandom(adventurerTypeArr, weight);
    }

    #region Event Aniamtion

    // 하루가 끝났을 때, 각종 리셋 하는 함수
    public void ResetGame()
    {
        gameDataManager.SetVisitors(0);
        gameDataManager.ResetSettlementData(); // 정산 화면에서 보여진 후 현재 골드 양, 현재 성공 횟수, 현재 대 성공 횟수, 현재 실패 횟수 초기화
        isLoadedFromSave = false; // 이어하기 여부 초기화

        uiManager.SetBackGround(BgType.CloseCounter);
        
        NextDay(); // 일 수 갱신

        // 첫 번째 방문이지만, 첫 날이 아니라면,
        // 오프닝 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해준다.
        initializeBuffer();
        HandlePreEnhancementFlow(0); // 강화 하기전 흐름 처리와 첫 방문 대사 세팅
    }

    public void SetupSettlementUI(BgType bgType)
    {
        dialogueUI.ChangeSprite(Dir.Right, blackSmithData.NormalSprite); // 오른쪽 대장장이 이미지를 기본 이미지로 세팅
        settlementWindow.SetupSettlementUI(); // 정산 창에 현재 일차, 성공 횟수, 대성공 횟수, 실패 횟수, 골드 수치 갱신
        uiManager.SetBackGround(bgType); // 정산 할 때 배경 이미지 변경
        dialogueUI.ShowImage(Dir.Left, false); // 왼쪽 스프라이트 끄기
        dialogueUI.ShowImage(Dir.Right, true); // 오른쪽 스프라이트 켜기
    }

    public void WelcomNpc(BgType bgType)
    {
        HandlePreEnhancementFlow(1);

        if (!isLoadedFromSave)
        {
            Debug.Log("방문자 수 증가");
            gameDataManager.SetVisitors(gameDataManager.GetVisitors() + 1); // 방문자 수 증가
        }

        isLoadedFromSave = false; // 첫 호출 이후부터는 정상 동작

        gameDataManager.BackupGameData(); // 게임 데이터 백업

        topUIManager.TopBarDisPlay(); // 방문자 수 갱신
        uiManager.SetBackGround(bgType);
        SoundManager.Inst.PlaySFX(ESfx.Bell);
    }

    public void SetBackGroundImage(BgType bgType)
    {
        uiManager.SetBackGround(bgType);
    }

    public void PlaySfx(ESfx sfxType)
    {
        SoundManager.Inst.PlaySFX(sfxType);
    }

    #endregion

    #region NPC 퇴장 연출
    // 나가는 연출
    public void ExitAnimation()
    {
        //dialogueUI.NPCExitTrigger(); // NPC 나가는 연출 트리거
        dialogueUI.NextTrigger();
        if (gameDataManager.GetVisitors() > 7)
        {
            Debug.Log("손님 끝");
            dialogueUI.AdjustmentTrigger();
        }
        topUIManager.SetGoldText(gameDataManager.GetGold());

        // 골드 텍스트가 올라가면서 애니메이션 연출
        Debug.Log($"강화 결과 : {enhanceManager.Result}");
        
    }
    #endregion

    #region 무기 정보를 확인하는 메서드
    public void StartEnhanceInteraction(WeaponController weapon)
    {
        //dialogueUI.ContentBoxFalseTrigger(); // 대화 내용창 끄는 트리거
        dialogueUI.NextTrigger();
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

    private void CreateBuffer()
    {
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

    private void initializeBuffer()
    {
        // 만약, 첫 번째 방문이라면, 대사 버퍼에 대사 객체를 새로 만들어준다.
        if (gameDataManager.GetVisitors() <= 0 && gameDataManager.GetDay() == 0)
        {
            CreateBuffer();
        }
        // 첫 번째 방문이지만, 첫 날이 아니라면,
        // 오프닝 대사 버퍼에 있는 대사 객체들의 내용을 초기화 해준다.
        else if (gameDataManager.GetVisitors() <= 0 && gameDataManager.GetDay() > 0)
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
        if(gameDataManager.GetVisitors() <= 0) // 첫 번째 방문이라면, 오프닝 대사와 정산 대사 세팅
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
        if (gameDataManager.GetVisitors() == 0)
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
