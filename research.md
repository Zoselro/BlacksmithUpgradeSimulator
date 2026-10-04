# BlacksmithUpgradeSimulator (프로젝트 포지) — 코드베이스 분석 보고서

> 작성일: 2026-10-04 · 기준 브랜치: `claudeCode` (HEAD `1431f0d GameStatus 변수명 변경`)
> 분석 범위: `Assets/Scripts` 전체(33개 파일, 약 3,000줄), 씬 2개, 애니메이터 컨트롤러/클립 7개, 프리팹·ScriptableObject, 대사 JSON, 프로젝트 설정, 루트의 기획 메모

---

## 1. 한눈에 보기

- **장르**: 2D 대장간 경영·강화 시뮬레이션. 하루에 손님(NPC) **8명**이 찾아오고, 각 손님이 맡긴 무기를 강화합니다. 결과(성공/대성공/실패)에 따라 골드를 받고, 하루가 끝나면 정산 화면이 나온 뒤 다음 날로 넘어갑니다.
- **엔진**: Unity **6000.0.58f2** (Unity 6), 2D, UGUI + TextMeshPro, **새 Input System**(`activeInputHandler: 2`)
- **빌드 버전**: `bundleVersion 1.1.2`, 1920×1080
- **씬**: `GameStart`(타이틀) → `GameScene`(본게임). `EnforceScene`은 빌드 목록에서 비활성 상태이고 파일도 없습니다.
- **진행 구동 방식**: 게임 흐름의 대부분이 **애니메이터 상태 머신 + 애니메이션 이벤트**와 **대화 세트의 종료 콜백(EndFunc)** 이 맞물려 돌아갑니다. 코드만 읽으면 흐름이 보이지 않으므로 이 보고서의 5장이 핵심입니다.
- **저장**: `Application.persistentDataPath`에 CSV 파일 4개를 둡니다. 모든 Get/Set이 **매번 파일을 직접 읽고 씁니다**(메모리 캐시 없음).

---

## 2. 저장소 구조

```
F:\BlacksmithUpgradeSimulator\
├─ BlacksmithUpgradeSimulator\          ← Unity 프로젝트
│  ├─ Assets\
│  │  ├─ Scripts\                       ← 게임 코드 (아래 3장)
│  │  ├─ Scenes\ GameStart.unity, GameScene.unity
│  │  ├─ Animation\GameScene\           ← GameScene.controller + 클립 7개
│  │  ├─ PreFabs\                       ← BlackSmithData.asset, Success/GreatSuccess/Fail.prefab, NpcPrefab.prefab
│  │  ├─ Resources\Dialogue\GameScript.Json   ← 대사 525줄
│  │  ├─ Resources\WeaponDatas\         ← WeaponData SO 15개 (5종 × 3등급)
│  │  ├─ Resources\UIResources\         ← UI 스프라이트
│  │  ├─ Miniature Army 2D V.1\Prefab\Basic\{성향}\{등급}\  ← NPC 프리셋 프리팹 160개 (NpcData 보유)
│  │  ├─ forge_sound\                   ← BGM 2, SFX 10 (wav)
│  │  ├─ Fonts\ (CookieRun SDF, 136MB)
│  │  ├─ BlacksmithUpgradeSimulator.inputactions  ← 메인 입력(Player/UI 맵)
│  │  └─ New Actions.inputactions       ← 미니게임용(현재 비활성)
│  ├─ Packages\manifest.json            ← 작업 트리에서 수정됨: unity-mcp 패키지 추가 (미커밋)
│  └─ ProjectSettings\
├─ 구현 현황\                           ← 기획서 PDF/PPTX, 데이터 테이블 xlsx, 브리핑 자료, 메모 txt
├─ 이어하기 기능 추가.txt               ← 저장/이어하기 설계 설명 (7장과 대조)
├─ 애니메이션 구현 현황.txt
└─ b15d72d3…, b163e4bc…, b1760…, b19dc…, b1b84…   ← Unity Library의 TextureImporter 캐시로 보이는 바이너리. git에 추적되고 있으며 프로젝트와 무관합니다.
```

**인코딩 주의**: `GameManager.cs`만 UTF-8(BOM)이고, 나머지 한글 주석이 있는 `.cs` 파일은 **CP949(EUC-KR)** 입니다. UTF-8 도구로 열면 주석과 한글 문자열 리터럴(`"강화 단계 : + "` 등)이 깨집니다. Unity 컴파일러가 이 리터럴을 어떻게 해석하느냐에 따라 게임 화면의 한글이 깨질 수 있으므로, UTF-8로 통일할 것을 권합니다.

---

## 3. 스크립트 구성 (클래스 맵)

| 영역 | 파일 | 역할 |
|---|---|---|
| **중앙 제어** | `GameManager.cs` (600줄) | 하루/손님 흐름 전체, 대사 버퍼 구성, NPC 등급 가중치, 애니메이션 이벤트 수신자. `Emotion`·`NpcState` enum도 여기에 정의 |
| **저장** | `GameDataManager.cs` | CSV 4종 읽기/쓰기, `GameStatus` enum, 백업/롤백 |
| **강화** | `Enhance/EnhanceManager.cs` | 진행 바, 결과 판정(난수), 골드·카운트 기록, 결과 팝업 생성. `EnhanceResult` enum |
| | `Enhance/EnhanceChanceCalculator.cs` | 강화 단계(0–14) → 성공 확률 테이블 |
| | `Enhance/EnhanceButton.cs` | "강화하기" / 결과 "확인" 버튼 핸들러 |
| | `Enhance/EnhanceUIManager.cs` | 무기 정보 패널, 진행 바, 확률 표시, 확인 버튼 |
| | `Enhance/EnhancementImage.cs` | 결과 팝업 프리팹(이전/이후 강화 단계 표시) |
| | `Enhance/StarCatchManager.cs` | 타이밍 미니게임 — **씬에서 비활성, 실질적으로 미사용** |
| **NPC/무기** | `Npc/NpcGenerator.cs` | 등급별 NPC 후보 맵, 무작위 선택, 무기 레벨 결정 위임 |
| | `Npc/NpcController.cs` | 씬의 NPC 1개(파츠 Image 9개)에 프리셋 스프라이트·RectTransform 복사, 표정 변경 |
| | `Npc/NpcData.cs` | NPC 프리셋 컴포넌트(등급, 이름, 성별, 성향, 파츠, 표정 스프라이트) + `AdventurerType`·`Gender`·`NpcTendency` enum |
| | `Npc/WeaponData.cs` | 무기 SO(종류, 등급, 스프라이트) + `WeaponType`·`WeaponRank` enum |
| | `Weapon/WeaponController.cs` | 등급에 따라 시작 강화 단계와 무기를 무작위로 결정, 강화 후 단계 계산 |
| **대화** | `Dialogue/DialogueController.cs` | 대화 세트/줄 인덱스 진행, 스페이스바 입력 수신 |
| | `Dialogue/DialogueSet.cs`, `DialogueLine.cs` | 대화 묶음(줄 배열 + 종료 콜백), 한 줄(이름·내용·스프라이트·화자·방향) |
| | `Dialogue/DialogueUI.cs`, `DialogueBoxUI.cs` | 말풍선·좌우 초상 표시, **애니메이터 트리거 발신자** |
| | `Dialogue/ScriptReader.cs`, `ScriptReaderDialogueLine.cs` | `Resources/Dialogue/GameScript` JSON 로드·조건 검색·무작위 선택 |
| | `Dialogue/Typing/TypingManager.cs`, `TextSender.cs` | 타자기 효과 — **연결은 되어 있지만 실제로 쓰이지 않음** |
| **UI** | `UIManager/UIManager.cs` | 배경 전환(`BgType`), NPC 표시(CanvasGroup alpha), 카운터 이미지, 방문 팝업 문구 |
| | `UIManager/TopUIManager.cs` | 상단 바(방문 수, 일차, 요일, 골드) |
| | `UIManager/MenuManager.cs` | 메뉴/설정(볼륨)/종료 팝업 |
| | `Adjustment/SettlementWindow.cs`, `StartNextDayButton.cs` | 정산 창, 다음 날 시작 |
| **공통** | `SoundManager.cs` | `DontDestroyOnLoad` 싱글턴 `SoundManager.Inst`, enum 인덱스 = 클립 배열 인덱스 |
| **타이틀** | `StartScene/*.cs`, `StartSceneUIManager.cs`, `GameExitButton.cs` | BGM, 버전 표시, 게임 시작/종료 |
| BlackSmith | `BlackSmith/BlackSmithData.cs` | 대장장이 SO: 이름, 스프라이트 4종, 대사 eventID 8종 |

### 의존 관계 요약

```
               ┌──────────── Animator (GameManager 오브젝트에 부착) ────────────┐
               │  AnimationEvent → GameManager.WelcomNpc / ResetGame / ...       │
               ▼                                                                │
 GameManager ──► NpcGenerator ──► NpcController, WeaponController             │
     │  │  └──► ScriptReader (대사 텍스트)                                       │
     │  └────► DialogueController ──► DialogueUI ──► DialogueBoxUI             │
     │                                     └──── animator.SetTrigger ──────────┘
     ├──► EnhanceManager ──► GameDataManager (CSV)
     ├──► UIManager / TopUIManager / EnhanceUIManager / SettlementWindow
     └──► GameDataManager (CSV)
 EnhanceButton(UI 버튼) ──► EnhanceManager.RequestEnhance, DialogueController.OnClickNextBtn
 SoundManager.Inst ◄── 거의 모든 클래스 (타이틀 씬에서 생성되어 유지)
```

모든 참조는 `[SerializeField]`를 인스펙터에서 연결하는 방식입니다(싱글턴은 `SoundManager`, `TypingManager`뿐). DI나 이벤트 버스는 없습니다.

---

## 4. 씬 구성

### GameStart (타이틀)
- `SoundManager`(BGM 2개, SFX 8개 연결), `StartSceneGameManager`(타이틀 BGM 재생, `Application.version` 표시), `StartButton` → `StartSceneBtn.GameStart()` → `GameScene` 로드, `ExitButton` → 앱 종료.
- **"새 게임 / 이어하기" 구분이 없습니다.** 시작하면 항상 저장 파일을 이어서 씁니다.

### GameScene
주요 오브젝트와 직렬화된 값입니다.

| 오브젝트 | 컴포넌트 | 주요 값 |
|---|---|---|
| `GameManager` | GameManager + **Animator(GameScene.controller)** | `enhanceTime 3`, `eventPopUpTime 1`(미사용) |
| `EnhanceManager` | EnhanceManager | **`greatSuccessRatio 0.05`**, `bonusProbablity 0`, 결과 프리팹 [Fail, GreatSuccess, Success] |
| `EnhanceChanceCalculator` | | lower/upper 확률 필드 값이 있지만 **이를 쓰는 메서드는 주석 처리됨** |
| `WeaponGenerator` | WeaponController | WeaponData 15개 |
| `NpcGenerator` | NpcGenerator | NpcData 프리셋 **160개** |
| `NpcPrefab` | NpcController | 씬에 있는 NPC 1개(파츠 9개) |
| `ContentBox` | **DialogueController** | 애니메이션이 `ContentBox`를 껐다 켭니다 |
| `InputManager` | PlayerInput(Invoke Unity Events) | `Player/SpaceBar` → `DialogueController.OnSpaceBar` |
| `MiniGame` (비활성) | StarCatchManager + PlayerInput(New Actions) | `enhanceManager` 참조 **null** |
| `TypingManager` | TypingManager | `timeForCharacter 0`, `ContentText`의 EventTrigger에서 `GetInputDown/Up` 호출 |
| `EnhanceButton` / 결과창 `ConfirmButton` | EnhanceButton ×2 | 두 번째 인스턴스는 `gm` 참조가 null(사용하지 않으므로 문제 없음) |
| 정산 `ConfirmButton` | StartNextDayButton | |
| `MenuManager` 등 | | 메뉴 버튼 → `ShowMenu(true)`, 설정 완료 → `SettingComplete(false)`, `SafeDataResetButton` → `ResetSetting()`(효과음만 재생) |

**버튼 연결 (UnityEvent)**
- `EnhanceButton` → `EnhanceButtonOnClick()`
- `WeaponInfoButton` → `OnClickActiveEnhanceActivePanel(false)`(무기 정보 보기), `CheckButton` → `(true)`(돌아가기)
- 결과창 `ConfirmButton` → `EnhanceButton.ConfirmButtonOnClick()`
- 정산 `ConfirmButton` → `StartNextDayButton.OnStartNextDayButton()`

---

## 5. 게임 루프 — 애니메이터 × 대화 세트 × 이벤트

### 5.1 애니메이터 상태 머신 (`GameScene.controller`)

파라미터는 Trigger 3개입니다: `NextTrigger`, `Adjustment`, `StartText`

```
[BlackSmithEnter] (기본 상태, ★모션 없음) ──exitTime 0.75──► [LeftEntry] (대장장이 왼쪽에서 등장, ContentBox ON)
        ▲                                                         │ NextTrigger
        │                                                         ▼
        │                                               [WelcomNpc] (2s) ◄──────────────┐
        │                                     t=0: GameManager.WelcomNpc(OpenCounter)  │
        │                                     "누군가 방문" 팝업, NPC 슬라이드 인        │
        │                                                         │ NextTrigger          │
        │                                                         ▼                      │
        │                                               [ContentFalse] (대화창 OFF = 강화 UI 단계)
        │                                                         │ NextTrigger          │
        │                                                         ▼                      │
        │                                               [ContentTrue] (대화창 ON)        │
        │                                                         │ NextTrigger          │
        │                                                         ▼                      │
        │                                               [NpcExit] (2s, NPC 슬라이드 아웃) ┘ exitTime 1.0 → 다음 손님
        │                                                         │ Adjustment (exitTime 0.875, 먼저 판정됨)
        │                                                         ▼
        │                                               [Adjustment] (7s)
        │                                     t=0: SetBackGroundImage(CloseCounter)
        │                                     t=2: SetupSettlementUI(Blacksmith)
        │                                     t=6: PlaySfx(Settle_sound)
        │                                                         │ StartText
        │                                                         ▼
        └────────exitTime 1.0──────────────────────── [ShowStartDayText] (1.5s, "다시 하루가 밝았다.")
                                                      t=1.5: GameManager.ResetGame()
```

- 애니메이터가 `GameManager` 오브젝트에 붙어 있으므로 **애니메이션 이벤트는 모두 `GameManager`의 public 메서드를 호출**합니다. `intParameter`는 enum 값입니다(1 = `BgType.OpenCounter`, 0 = `CloseCounter`, 3 = `Blacksmith`, 7 = `ESfx.Settle_sound`).
- `NextTrigger`를 보내는 곳: `UIManager.WelcomNextNpc`, `GameManager.StartEnhanceInteraction`, `EnhanceButton.ConfirmButtonOnClick`, `GameManager.ExitAnimation`. 상태 4개가 모두 같은 트리거로 넘어가므로, **트리거를 한 번 더 보내거나 덜 보내면 연출 전체가 어긋납니다.**
- ★ `BlackSmithEnter.anim`은 커밋 `1b381d9`에서 삭제됐지만 상태는 남아 있습니다(Motion 누락). 빈 상태로 잠깐 머문 뒤 `LeftEntry`로 넘어갑니다.

### 5.2 대화 세트 4개 (`GameManager.dialogueSet[0..3]`)

| 세트 | 줄 수 | 내용 | 종료 콜백(EndFunc) |
|---|---|---|---|
| 0 | 1 | 대장장이 오프닝 (`shopOpen00`) — 하루의 첫 손님 전에만 | `uiManager.WelcomNextNpc("누군가가 방문 했습니다.")` → NextTrigger |
| 1 | 3 | 대장장이 인사(`greetSmith`) → NPC 등장(`Enter`) → NPC 의뢰(`Request`) | `StartEnhanceInteraction(weapon)` → NextTrigger + 강화 패널 열기 |
| 2 | 1 | 대장장이 결과 반응(성공/대성공/실패), 오른쪽 초상 | `SetBackGroundOpenCounter()` |
| 3 | 2 | 대장장이가 무기를 건넴(`CompleteSuccess/Fail`) → NPC 퇴장(`ExitSuccess/ExitFail`) | `ExitAnimation()` → NextTrigger (+ 8명째면 Adjustment) |

- 줄 객체는 `buffer[4][]`에 미리 만들어 두고 재사용합니다(`Set`/`Reset`).
- `DialogueController`: `OnClickNextBtn` → 남은 줄이 있으면 `OnDialogueNext`, 없으면 `OnDialogueEnd`(콜백 실행 → 다음 세트의 첫 줄 표시).
- 내용이 비어 있는 줄(`IsValid == false`)은 표시하지 않고 **인덱스도 올리지 않은 채 return** 합니다. 그래서 세트 2는 강화 결과가 나오기 전까지 "대기" 상태로 멈춰 있습니다(이 동작에 기대고 있음).
- `dialogueSetIndex == 3 && lineIndex == 0`일 때 `ShowPrefab(true)`로 NPC를 다시 보이게 합니다(강화 화면에서 숨겼던 NPC).
- 대장장이 줄에서만 `ShowImage(dir, sprite)`로 초상을 바꿉니다. NPC 줄에 저장된 스프라이트는 사용되지 않습니다.

### 5.3 손님 1명 처리 순서 (정상 플레이)

1. **WelcomNpc 상태 진입 (t=0 이벤트)** → `GameManager.WelcomNpc(OpenCounter)`
   - `HandlePreEnhancementFlow(1)`: `SetupNpc()`(등급 가중 추첨 → NPC·무기·강화 단계·확률 결정) → 대사 읽기 → `dialogueController.SetDialogue(sets, 1)`(세트 1의 첫 줄 즉시 표시)
   - 이어하기 직후가 아니면 `visitors + 1`, `BackupGameData()`, 상단 바 갱신, 배경 변경, 종소리
2. 스페이스바 ×3 → 세트 1 진행 → 4번째 입력에서 `StartEnhanceInteraction` → `ContentFalse`(대화창 OFF) + 강화 패널. `dialogueSetIndex = 2`인데 줄이 비어 있으므로 대기합니다.
3. (선택) 무기 정보 버튼으로 종류·등급·현재 단계 확인
4. **강화하기** → `RequestEnhance(true)`(로딩 SFX), 패널을 닫고 진행 바와 "성공 확률 xx%" 표시, 배경을 작업실로, 상단 바·대장장이·NPC·카운터 숨김
5. `GameManager.Update` → 매 프레임 `EnhanceManager.PlayEnhance(...)`: 3초 동안 진행 바를 채우고 `GameStatus.Enhancing` 기록
6. 진행 100% → `Enhance()` 난수 판정 → 카운트 기록 → `GameStatus.Enhanced` → 결과 대사 세팅(`SetPostEnhancementDialogue` + `OnEnhanceResult`) → `EnhanceEquipment()`로 골드 기록, 강화 후 단계 계산, 결과 팝업 생성, SFX → 확인 버튼 표시
7. **확인** → 배경을 대장간으로, 상단 바 복원, `OnClickNextBtn()`(세트 2 결과 대사), `NextTrigger`(→ ContentTrue), 팝업 Destroy
8. 스페이스 → 세트 2 종료 → 카운터 배경 → 세트 3(NPC 다시 표시, 무기 전달 대사) → 스페이스 → NPC 퇴장 대사 → 스페이스 → `ExitAnimation()`: NextTrigger(→ NpcExit), `visitors > 7`이면 `AdjustmentTrigger()`(정산 대사 세팅), 골드 텍스트 갱신
9. NpcExit 종료 → (8명 미만) WelcomNpc로 돌아가 1번부터 반복 / (8명) Adjustment

### 5.4 하루의 끝 / 다음 날
- **Adjustment** 애니메이션: 대장장이 퇴장, 페이드, `EndContentBox`(마감 대사), 정산 창(당일 성공/대성공/실패/획득 골드), 정산 SFX
- 정산 **확인** → `StartText` 트리거 + "다시 하루가 밝았다." → 1.5초 뒤 `ResetGame()`:
  `visitors = 0`, 당일 정산 데이터와 상태 초기화, 배경 닫힌 카운터, `NextDay()`(day + 1, 요일 갱신), 버퍼 초기화, `HandlePreEnhancementFlow(0)`(오프닝 세트부터) → BlackSmithEnter → LeftEntry …
- 요일은 `weekdays[(day-1) % 7]`로 계산합니다(1일차 = "월").
- **엔딩·목표·명성 시스템은 없습니다**(`작업 메모장.txt`의 명성 구상과 `명성.png`만 존재). 날짜는 끝없이 이어집니다.

---

## 6. 수치 시스템

### 6.1 손님 등급 가중치 (`GameManager.GetWeightedCustomer`) — 기준은 **추첨 시점의 `visitors` 값**

| visitors | 초급 | 중급 | 숙련 |
|---|---|---|---|
| 0 (첫 손님) | 100% | 0 | 0 |
| 1 | 90% | 8% | 2% |
| 2 | 70% | 25% | 5% |
| 3 | 45% | 45% | 10% |
| 4 | 20% | 60% | 20% |
| 5 | 10% | 45% | 45% |
| 6 | 0 | 45% | 55% |
| 7 (8번째) | 0 | 30% | 70% |
| 8+ (기본값) | 0 | 10% | 90% |

정상 플레이에서는 `WelcomNpc`가 추첨을 **먼저** 하고 visitors를 **그다음에** 올리므로, n번째 손님은 `visitors = n-1` 행을 씁니다. 이어하기 직후에는 이미 올라간 값으로 추첨하므로 한 단계 높은 행을 씁니다(9장 B-4).

### 6.2 시작 강화 단계와 무기 (`WeaponController`)
| 등급 | 시작 단계 `Random.Range` | 무기 등급 |
|---|---|---|
| Beginner | 0–4 | Common |
| Intermediate | 5–9 | Rare |
| Advanced | 10–14 | Epic |

무기 종류는 같은 등급 5종(장검, 단검, 활, 레이피어, 석궁) 중에서 무작위로 고릅니다. 코드 주석의 "+0~+3" 같은 범위 표기는 실제 값과 다릅니다.

### 6.3 성공 확률 (`EnhanceChanceCalculator.GetEnhanceProbablity`)
| 단계 | 0 | 1 | 2 | 3 | 4 | 5 | 6 | 7 | 8 | 9 | 10 | 11 | 12 | 13 | 14 |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 확률 | 95 | 90 | 85 | 80 | 75 | 70 | 60 | 55 | 50 | 45 | 40 | 35 | 30 | 20 | 10 |

### 6.4 판정 (`EnhanceManager.Enhance`)
```
roll = Random.value
roll >  p                 → Fail
roll <= p × 0.05          → GreatSuccess   (대성공 = 성공 확률의 5%)
그 외                     → Success
```

### 6.5 보상 / 결과 (`EnhanceEquipment`)
| 결과 | 초급 | 중급 | 숙련 | 강화 단계 |
|---|---|---|---|---|
| 대성공 | +100 | +200 | +250 | +2 |
| 성공 | +50 | +100 | **+100** | +1 |
| 실패 | 0 | 0 | 0 | 변화 없음(하락·파괴 없음) |

- 골드는 누적(`Gold`)과 당일(`CurrentGold`) 두 곳에 함께 더합니다. **골드를 쓰는 곳(비용, 상점 등)은 없습니다.**
- 숙련 손님의 일반 성공 보상(100)이 중급과 같습니다. 의도한 값인지 확인이 필요합니다.
- 결과로 단계가 바뀌어도 다음 손님에게 이어지지 않습니다(손님마다 새 무기).

---

## 7. 저장 / 이어하기 시스템 (`GameDataManager`)

### 7.1 파일 (`%USERPROFILE%\AppData\LocalLow\DefaultCompany\BlacksmithUpgradeSimulator\`)

| 파일 | 헤더 | 기본값 |
|---|---|---|
| `GameData.csv` | `Day,Gold,Visitors` | `1,0,0` |
| `StatisticsData.csv` | `SuccessCnt,GreatSuccessCnt,FailCnt` (누적) | `0,0,0` |
| `SettlementData.csv` | `CurrentGold,CurrentSuccessCnt,CurrentGreatSuccessCnt,CurrentFailCnt,GameStauts` (당일 + 상태) | `0,0,0,0,0` |
| `BackUpData.csv` | 위 10개 필드(상태 제외) | `1,0,0,…` |

- `Awake`에서 파일이 없을 때만 기본값으로 만듭니다.
- 모든 Getter는 `File.ReadAllLines` → `lines[1].Split(',')` → `int.Parse`, 모든 Setter는 읽기 → 필드 하나 교체 → `WriteAllLines` 입니다.
- `GameStatus`: `NoEnhance(0)` 강화 전, `Enhancing(1)` 강화 중, `Enhanced(2)` 강화 완료
- `BackupGameData()`: `WelcomNpc`마다(visitors를 올린 **뒤**) 현재 값 10개를 스냅샷합니다.
- `RollbackGameData()`: 스냅샷 10개를 되돌립니다(**GameStatus는 되돌리지 않음**).

### 7.2 게임 시작 시 분기 (`GameManager.Awake` → `Start`)

```
Awake:
  isLoadedFromSave = visitors > 0
  if status == Enhanced && visitors < 8 : visitors += 1, status = NoEnhance   // 강화가 끝난 손님 → 다음 손님으로
  elif status == Enhancing              : RollbackGameData()                  // 강화 도중 종료 → 손님이 들어온 시점으로

Start:
  visitors == 0        → HandlePreEnhancementFlow(0), NoEnhance, Play("BlackSmithEnter")   // 하루 시작
  1 ≤ visitors ≤ 7     → HandlePreEnhancementFlow(1), NoEnhance, Play("WelcomNpc")
  visitors ≥ 8 :
      status == Enhanced → Play("Adjustment")                                        // 바로 정산
      그 외              → HandlePreEnhancementFlow(1), NoEnhance, Play("WelcomNpc")  // 8번째 손님 다시
```
`WelcomNpc` 이벤트에서는 `isLoadedFromSave == true`이면 visitors를 올리지 않고 플래그만 내립니다. 그래서 이어하기 직후 첫 손님이 중복으로 집계되지 않습니다(커밋 `df0ce9d`, `817b4fb`의 수정 내용).

`이어하기 기능 추가.txt`의 설계(강화 전·중 종료 → 롤백, 강화 완료 후 종료 → 결과 인정, 8번째 손님 완료 후 종료 → 바로 정산)는 위 분기로 구현되어 있습니다. 다만 아래 9장 A-1처럼 **"강화 전" 상태가 플레이 중에 기록되지 않는 문제**가 있습니다.

---

## 8. 대사 데이터 (`Resources/Dialogue/GameScript.Json`)

- 스키마: `{ lines: [ { text, eventID, NpcTendency, NpcState, NpcType, Gender } ] }`, `JsonUtility`로 읽습니다.
- **대장장이 대사 14줄**(eventID로 검색): `shopOpen00`, `greetSmith`, `enhanceSuccessSmith`, `enhanceGreatSuccessSmith`, `enhanceFailSmith`, `shopClose` 각 2줄, `CompleteSuccess`·`CompleteFail` 각 1줄
- **NPC 대사 511줄**: 등급 3 × 성향 4 × 상태 4 × 성별 2 = **96개 조합이 모두 정확히 5줄씩** 있습니다(빠진 조합 없음).
- ★ 추가로 `NpcTendency: "Insightful"`인 31줄이 있지만, enum 이름은 `Insight`이므로 **이 31줄은 절대 선택되지 않습니다.**
- 검색 방식: `ReadPlayer(eventID)` / `ReadNPC(type, tendency, state, gender)`가 매번 525줄 전체를 훑어 조건에 맞는 줄 중 하나를 무작위로 고릅니다.
- NPC 프리셋 160개: 성향 4 × (초급 12 + 중급 12 + 숙련 16). 남녀가 반반이고, 숙련 남성만 성향별 10개입니다.

---

## 9. 발견된 문제점 · 위험 요소

### A. 저장/이어하기 로직 (높음)

**A-1. 플레이 중에는 `GameStatus`가 `NoEnhance`로 돌아가지 않습니다 → 이어하기 때 손님을 건너뜀**
`SetGameStatus` 호출 위치는 Awake/Start, `PlayEnhance`(Enhancing/Enhanced), `ResetSettlementData`(하루 리셋)뿐입니다. 그래서 첫 손님의 강화가 끝난 뒤로는 **그날이 끝날 때까지 상태가 계속 `Enhanced`** 로 남습니다.
- 재현: k번째 손님(k ≥ 2)이 들어와 대화 중이거나 강화 패널에서 아직 강화하지 않은 상태로 종료 → 다시 실행 → Awake가 이전 손님의 `Enhanced`를 보고 `visitors += 1` → **k번째 손님을 건너뜁니다.**
- k = 8에서 같은 상황이면 → Start의 `visitors ≥ 8 && Enhanced` 분기 → **8번째 손님을 처리하지 않고 바로 정산합니다.**
- 수정 방향: `GameManager.WelcomNpc`(또는 `HandlePreEnhancementFlow`)에서 `SetGameStatus(NoEnhance)`를 기록.

**A-2. 이어하기 후 정산 마감 대사가 비어 있음**
`dialogueClosePlayerData`(그리고 오프닝 대사)는 `SetPreEnhancementDialogue`에서 `visitors <= 0`일 때만 읽습니다. 하루 중간에 이어하기를 하면 이 값이 비어 있으므로, 그날 정산의 `EndContentBox`가 공백으로 나옵니다. Start에서 바로 `Play("Adjustment")`하는 경로는 `AdjustmentTrigger()`를 아예 거치지 않아 텍스트가 설정되지 않습니다.

**A-3. `Awake` 실행 순서 의존**
`GameManager.Awake`가 `gameDataManager.GetVisitors()`를 호출하지만, 파일 경로는 `GameDataManager.Awake`에서 설정됩니다. Script Execution Order가 지정되어 있지 않으므로 Unity가 다른 순서로 실행하면 `File.ReadAllLines(null)` 예외가 납니다. 지금 동작하는 것은 우연한 순서 덕분일 수 있습니다.

**A-4. 이어하기 직후 등급 가중치가 한 단계 밀림**
6.1에서 설명했듯이, 이어하기 직후에는 이미 올라간 `visitors`로 추첨합니다(예: 8번째 손님 다시 → `default` 행, 숙련 90%).

**A-5. 저장 초기화 수단 없음**
타이틀에 새 게임 버튼이 없고, `SafeDataResetButton`(`MenuManager.ResetSetting`)은 효과음만 냅니다. 처음부터 하려면 CSV를 직접 지워야 합니다.

**A-6. 롤백의 실효성**
게임 데이터(카운트, 골드)는 결과가 나온 **같은 프레임**에 기록되고, 그 사이에 `Enhanced`가 설정됩니다. 따라서 `Enhancing` 중 종료로 롤백할 때 실제로 되돌릴 변경은 거의 없습니다(무해한 안전망). 롤백 후에는 **다른 무작위 NPC/무기**가 나옵니다.

### B. 성능 (중간)
- `GameManager.Update`가 **매 프레임** `Debug.Log` 2번을 찍고, 그중 하나가 CSV를 읽습니다.
- 강화 중에는 매 프레임 `PlayEnhance` 인자로 CSV를 8번 읽고, `SetGameStatus(Enhancing)`로 1번 더 읽고 씁니다. 3초 × 60fps ≈ **1,600번 이상의 파일 I/O**에 로그 스팸까지 겹칩니다.
- 권장: 시작할 때 메모리로 읽어 들이고, 바뀔 때만 저장(dirty flag)하는 방식.

### C. 대화 / 흐름 (중간~낮음)
- **C-1.** NPC가 두 번 생성됩니다. Start(visitors 0~7)에서 `HandlePreEnhancementFlow`를 호출한 직후 재생되는 `WelcomNpc` 애니메이션 이벤트가 또 호출합니다. 첫 번째 결과는 버려집니다. visitors 0인 경우도 마찬가지로 Start에서 만든 NPC가 버려집니다(대사 텍스트도 다시 추첨됨).
- **C-2.** `DialogueController.OnClickNextBtn`에 `dialogueSetIndex` 범위 검사가 없습니다. 세트 3이 끝난 뒤(`index == 4`, NpcExit·Adjustment 중) 대화창이 활성인 상태에서 스페이스를 누르면 `IndexOutOfRangeException`이 납니다. 첫 손님의 강화 대기 중에는 `dialogues[2].Dialogues == null`이라 NRE가 날 수 있지만, 그때는 `ContentBox`가 비활성이어서 입력이 막힙니다. 이런 보호는 애니메이션의 활성/비활성 키에 의존하므로 깨지기 쉽습니다.
- **C-3.** 빈 줄이면 인덱스를 올리지 않고 return합니다. 데이터가 빠지면(예: `ReadNPC`가 null 반환) **그 줄에서 영구히 멈춥니다(소프트락)**. 현재 데이터는 96개 조합을 모두 채우고 있어서 발생하지 않습니다.
- **C-4.** `ScriptReader.ReadPlayer`는 결과가 0개일 때 검사하지 않습니다(`Random.Range(0,0)` → `list[0]` 예외).
- **C-5.** `charName.color = new Color(0f, 175f, 239f)` — `Color`는 0~1 범위입니다. 의도는 `Color32(0,175,239)`(하늘색)로 보이지만, 실제로는 (0,1,1) 시안으로 클램프됩니다. `SetImageUIAlpha(…, 255f)`도 같은 문제지만 결과는 1로 동작합니다.
- **C-6.** 메뉴를 열어도 강화가 멈추지 않습니다(`TryEnhance`/`TryAnimation` 주석 처리). 강화 중에 메뉴 → "종료" → 타이틀로 나가면 상태가 `Enhancing`으로 남아 다음 진입 때 롤백됩니다.

### D. 죽은 코드 / 잔재 (낮음)
- `StarCatchManager`(미니게임): 오브젝트는 비활성이고 `enhanceManager`가 null입니다. 활성화하면 성공 영역 판정에서 NRE가 납니다. `EnhanceManager.BonusProbablity`는 매개변수 값만 바꾸므로 **아무 효과가 없고**, `GameManager.SetProbability`는 호출되지 않습니다.
- `TypingManager`/`TextSender`: `timeForCharacter = 0`이고 `Typing()`을 호출하는 곳이 없습니다.
- `EnhanceChanceCalculator`의 lower/upper 필드, `GameManager.eventPopUpTime`·`isPaused`·`typingManager`, `UIManager.StopAnimator/StartAnimator`, `NpcController.GetAdventurerType`, `NpcGenerator.AdventurerType`, `DialogueLine.Active`, `SettlementWindow` 일부 필드는 쓰이지 않습니다.
- `initializeBuffer`의 `GetDay() == 0` 분기는 day가 1부터 시작하므로 도달할 수 없습니다.
- `BlackSmithEnter` 상태의 Motion 누락(5.1).
- `EnhanceUIManager.Initialized`: `weaponTypeText`에 "무기 등급", `weaponRankText`에 "무기 종류"를 넣습니다(필드 이름이 뒤바뀜, 화면 출력 자체는 맞음).
- `NpcController.Initialize`가 `ApplyNpcTemplate`보다 먼저 `SetEmotion`을 호출해서 이전 NPC 템플릿을 참조합니다(곧바로 다시 설정되므로 무해).
- CSV 헤더 오타 `GameStauts`, 필드 오타 `adventuerType`, `hair2RectTransfrom`, `enhaceGreatSuccess` 등. **필드 이름을 고치면 직렬화 값이 사라지므로** `[FormerlySerializedAs]`가 필요합니다.
- 루트의 `b1…` 바이너리 5개(Library 캐시)가 git에 추적되고 있습니다.

### E. 기타
- `SoundManager`는 enum 순서와 인스펙터 배열 순서가 같다는 가정에 기대고 있습니다(현재 SFX 8개, BGM 2개 순서 일치 확인). `GameScene`에는 `SoundManager`가 없으므로 **에디터에서 GameScene을 직접 Play하면 `SoundManager.Inst`가 null이어서 NRE**가 납니다(반드시 GameStart부터 실행).
- 강화 결과 팝업은 `Instantiate` 후 `SetSiblingIndex(last-1)`로 "아래에서 두 번째"에 놓습니다. 캔버스 자식 순서가 바뀌면 깨집니다.
- 작업 트리에 미커밋 변경이 있습니다: `Packages/manifest.json`, `packages-lock.json`(unity-mcp 추가).

---

## 10. 개선 우선순위 제안

1. **A-1 수정**: 손님 등장 시점에 `GameStatus.NoEnhance`를 기록 (한 줄 수정으로 손님 건너뛰기 버그 해결)
2. **A-2 수정**: 오프닝/마감 대사를 visitors와 관계없이 Start에서 읽고, 정산으로 바로 가는 경로에서도 `SetEndContentBox` 호출
3. **GameDataManager 메모리 캐싱** + `Update`의 `Debug.Log` 제거 (성능, B)
4. `GameDataManager`의 초기화를 지연 초기화나 `[DefaultExecutionOrder(-100)]`로 보장 (A-3)
5. `OnClickNextBtn`/`ReadPlayer` 경계 검사 추가 (C-2, C-4)
6. JSON의 `Insightful` → `Insight` 수정 (31줄 살리기)
7. 새 게임(저장 초기화) 버튼
8. 소스 인코딩 UTF-8 통일, 죽은 코드 정리

---

## 부록: 커밋 히스토리 요약 (최근)
- `1431f0d` GameStatus 변수명 변경
- `817b4fb` 마지막 NPC가 끝난 뒤 다음 방문자가 0명이 되는 오류 수정
- `3516654` 기능 추가 PPT
- `df0ce9d` 방문자 수 중복 증가 해결, 데이터 불러오기 정상 작동 확인
- `5d09fad` 롤백 메서드 추가 진행 중
- 총 89개 커밋. 초기에는 애니메이션 연출 → 강화/정산 → CSV 저장·이어하기 순서로 발전했습니다.
