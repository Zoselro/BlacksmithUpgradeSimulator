# 인트로 연출 구현 계획 (Intro 1~7 + CSV 대사)

> 작성일: 2026-10-04 · 기준: `claudeCode` 브랜치, `research.md` 분석 결과
> 참고 자료: `Assets/Resources/Intro/1~7.png`, `Assets/Resources/Intro/예시.png`(UI 기획서 "게임 인트로" 페이지)

---

## 0. 요구사항 정리

| # | 요구사항 | 출처 |
|---|---|---|
| R1 | 타이틀의 **게임 시작** 버튼을 누르면 인트로가 시작된다 | 사용자 |
| R2 | `1.png`부터 시작하고, **스페이스바**를 누를 때마다 다음 이미지로 넘어가 `7.png`까지 보여준다 | 사용자 |
| R3 | 7번이 끝난 뒤 스페이스바를 누르면 본게임(`GameScene`)으로 넘어간다 | 사용자 |
| R4 | 이미지별 대사 (아래 표) | 사용자 |
| R5 | 대사는 **CSV 파일로 관리**한다 | 사용자 |
| R6 | 배경이 바뀌면 말풍선을 숨겼다가 **1초 뒤에 말풍선을 표시**한다 | 예시.png 2번 항목 |
| R7 | 대사 텍스트는 **한 번에 표시**한다(타자기 효과 없음) | 예시.png |
| R8 | 말풍선 위에 화자 이름 **"플레이어"** 를 표시한다 | 예시.png |
| R9 | 말풍선이 떠 있는 동안 **▼(역삼각형) 아이콘이 위아래로 반복해서 움직인다** | 예시.png |
| R10 | 마지막 대사가 끝나고 스페이스바를 누르면 말풍선을 끈다 | 예시.png |
| R11 | (선택) 오른쪽 위 메뉴 버튼, 열면 일시정지 | 예시.png 3번 항목 → **2단계로 미룸** (9장) |

### 이미지별 대사

| 이미지 | 장면 | 대사 |
|---|---|---|
| 1.png | 강화 실패 화면 앞에서 키보드를 내리치는 플레이어 | . . . |
| 2.png | 모니터 클로즈업 "강화 실패" | 아 강화에 또 실패했네.. |
| 3.png | 화면이 붉게 깨지고 흔들림 | 도데체 왜 안되는거야! |
| 4.png | 모니터에서 빛이 쏟아져 빨려 들어감 | 어어? 뭐야!!! |
| 5.png | (이세계로 이동) | 으으.. 여기가 어디지.. |
| 6.png | | 어 저기는..? |
| 7.png | 대장간 바닥에 주저앉은 대장장이 | 어라? 이게 어떻게 된거지? |

> 대사는 요청하신 문장을 **그대로**(맞춤법 포함, 예: "도데체") CSV에 넣습니다. 고치려면 CSV만 수정하면 됩니다.

---

## 1. 현재 코드베이스에서 영향을 받는 부분

| 대상 | 현재 동작 | 인트로와의 관계 |
|---|---|---|
| `Scripts/StartScene/StartSceneBtn.cs` | `GameStart()` → `SoundManager.Inst.PlaySFX(Start_Button)` → `SceneManager.LoadScene("GameScene")` | **수정 대상**: 인트로 씬으로 보내도록 바꿉니다 |
| `ProjectSettings/EditorBuildSettings.asset` | `GameStart`(0), `GameScene`(1), `EnforceScene`(비활성, 파일 없음) | 인트로 씬을 **빌드 목록에 추가**해야 `LoadScene`이 동작합니다 |
| `SoundManager` | 타이틀 씬에서 만들어지는 `DontDestroyOnLoad` 싱글턴 | 인트로 씬에서도 `SoundManager.Inst`를 그대로 쓸 수 있습니다(새 씬에 SoundManager를 따로 둘 필요 없음) |
| `Dialogue/ScriptReader.cs` | `Resources.Load<TextAsset>("Dialogue/GameScript")` + `JsonUtility` | 리소스를 불러오는 **같은 관례**를 따릅니다(Resources + TextAsset). 파서는 CSV용으로 새로 만듭니다 |
| `Dialogue/DialogueController.OnSpaceBar` | `PlayerInput`(Invoke Unity Events)의 `Player/SpaceBar` 이벤트를 받고, `context.performed`일 때만 처리 | 인트로 입력도 **같은 패턴**으로 받습니다 |
| `BlacksmithUpgradeSimulator.inputactions` | `Player/SpaceBar` = `<Keyboard>/space` | 인트로 씬의 `PlayerInput`에 그대로 연결합니다 |
| `GameDataManager` | `GameScene`에만 있고, `Awake`에서 `persistentDataPath/GameData.csv` 생성 | 인트로를 "새 게임일 때만" 보여주려면 저장 상태를 알아야 합니다 → 정적 헬퍼 추가(6장) |
| 캔버스 | GameStart: Scale With Screen Size, 1920×1080, Match 1 | 인트로 씬도 **같은 설정**을 씁니다 |
| 인트로 이미지 임포트 설정 | `textureType: 8(Sprite)`, **`spriteMode: 2`(Multiple)**, 하위 스프라이트 `1_0` 하나, `maxTextureSize 2048`, 압축 켜짐 | ⚠ Multiple 모드에서는 `Resources.Load<Sprite>("Intro/1")`이 기대대로 동작하지 않을 수 있습니다 → 2.3절 |

---

## 2. 설계

### 2.1 구조 결정: **별도 씬 `IntroScene`** (권장)

```
GameStart ──[게임 시작]──► IntroScene ──[7번 이후 스페이스]──► GameScene
                └─(이어하기 저장이 있으면, 6장)──────────────────────► GameScene
```

**별도 씬으로 하는 이유**
- `GameScene`은 `GameManager.Awake/Start`에서 바로 애니메이터를 재생하고 저장 파일을 읽고 씁니다. 그 위에 인트로를 겹치면 애니메이터·`DialogueController`의 스페이스바 입력과 충돌합니다(`DialogueController.OnSpaceBar`는 `ContentBox` 활성 여부만 확인함).
- `GameStart` 위에 오버레이로 띄우는 방식도 가능하지만, 타이틀 UI와 섞이고 나중에 "인트로 다시 보기"를 넣기 어렵습니다.
- 새 씬으로 만들면 기존 코드는 `StartSceneBtn` 한 줄만 바뀝니다.

### 2.2 새로 만들 파일

```
Assets/
├─ Resources/Intro/
│   ├─ 1.png … 7.png                 (기존)
│   └─ IntroScript.csv               ★ 신규: 대사 데이터
├─ Scenes/
│   └─ IntroScene.unity              ★ 신규
└─ Scripts/Intro/                     ★ 신규 폴더 (UTF-8 with BOM으로 저장 — research.md 2장 인코딩 문제 재발 방지)
    ├─ IntroLine.cs                  데이터 클래스
    ├─ IntroScriptReader.cs          CSV → List<IntroLine>
    ├─ IntroController.cs            진행 상태 머신, 입력, 씬 전환
    └─ BobbingIcon.cs                ▼ 아이콘 위아래 반복 이동 (애니메이터 대신 코드로 구현해도 됨)
```

수정할 파일: `StartSceneBtn.cs`, `EditorBuildSettings.asset`(에디터의 Build Profiles에서 추가), 인트로 png 7개의 `.meta`(Sprite Mode).

### 2.3 이미지 로드 방식

**권장**: `1~7.png` 임포트 설정을 **Sprite Mode: Single**로 바꿉니다(인스펙터에서 7개를 한꺼번에 선택해 변경).
- 그 뒤 `Resources.Load<Sprite>("Intro/" + imageName)`으로 불러옵니다.
- 원본이 1672×941이므로 `maxTextureSize 2048`이면 충분합니다. 일러스트 품질을 위해 Compression을 `None` 또는 `High Quality`로 올리는 것을 권장합니다.

Multiple 모드를 유지해야 한다면 `Resources.LoadAll<Sprite>("Intro/" + imageName)[0]`으로 불러옵니다(하위 스프라이트 이름이 `1_0`이기 때문).
→ 구현에서는 **두 방식 모두 처리**합니다(`Load`가 null이면 `LoadAll` 결과의 첫 번째 사용).

### 2.4 CSV 형식 — `Assets/Resources/Intro/IntroScript.csv`

```csv
Order,Image,Speaker,Text
1,1,플레이어,. . .
2,2,플레이어,아 강화에 또 실패했네..
3,3,플레이어,도데체 왜 안되는거야!
4,4,플레이어,어어? 뭐야!!!
5,5,플레이어,으으.. 여기가 어디지..
6,6,플레이어,어 저기는..?
7,7,플레이어,어라? 이게 어떻게 된거지?
```

| 열 | 의미 | 규칙 |
|---|---|---|
| `Order` | 진행 순서 | 정수. 파일의 줄 순서와 상관없이 **이 값으로 정렬**합니다 |
| `Image` | `Resources/Intro/` 아래의 이미지 이름(확장자 제외) | 이미지 이름이 바뀌어도 CSV만 고치면 됩니다 |
| `Speaker` | 말풍선의 이름 칸 | 비어 있으면 이름 칸을 숨깁니다 |
| `Text` | 대사 | 비어 있으면 말풍선 없이 이미지만 보여줍니다(현재 데이터에는 빈 줄 없음. 나중에 대사 없는 컷을 넣을 때 사용) |

**확장성**: 같은 `Image`로 줄을 여러 개 넣으면 **이미지 하나에 대사 여러 줄**이 됩니다(이미지가 바뀌지 않으면 1초 지연 없이 다음 대사만 교체 — 예시.png의 "스페이스바 인풋 시, 다음 텍스트 출력"). 이번 요구사항은 이미지마다 1줄이지만 파서와 컨트롤러는 여러 줄을 지원하도록 만듭니다.

**인코딩 규칙(중요)**
- CSV는 **UTF-8**로 저장해야 합니다. Excel에서 그냥 "CSV"로 저장하면 **CP949**가 되어 한글이 깨집니다 → Excel에서는 반드시 **"CSV UTF-8(쉼표로 분리)"** 를 선택합니다.
- 파서에서 BOM(`﻿`)과 `\r`을 제거합니다.
- 대사에 쉼표를 넣을 수 있도록 큰따옴표로 감싼 필드를 지원합니다(`"음, 그렇군"`, 따옴표 자체는 `""`로 이스케이프).

### 2.5 진행 상태 머신 (`IntroController`)

```
           Start()
             │  CSV 로드 → Order 기준 정렬 → index = 0
             ▼
     ┌──► [ShowImage] 배경 = lines[index].Image, 말풍선 OFF
     │         │  WaitForSeconds(1.0)   ← 이미지가 바뀐 경우에만 (같은 이미지의 다음 줄이면 0초)
     │         ▼
     │    [ShowBalloon] Text가 있으면: 말풍선 ON, 이름/대사 한 번에 표시, ▼ 아이콘 ON
     │                  Text가 비었으면: 말풍선 OFF (이미지만)
     │         │  state = WaitingInput
     │         ▼
     │    [WaitingInput] ── Space ──► index++
     │         │                        │
     │         │      index < Count ────┘ (다시 ShowImage)
     │         ▼
     └──  index == Count → 말풍선 OFF → [Finished] → SceneManager.LoadScene("GameScene")
```

**입력 규칙**
- `state == WaitingInput`일 때만 스페이스바를 받습니다. 1초 지연 중의 입력은 **무시**합니다(연타로 대사를 건너뛰는 것 방지, 예시.png의 "1초 뒤 말풍선 출력" 의도 유지).
- 대사가 없는 이미지도 1초가 지난 뒤부터 입력을 받습니다(바로 넘기지 않도록).
- 입력할 때마다 `SoundManager.Inst?.PlaySFX(ESfx.Button_Click)` 재생 — `DialogueController.OnClickNextBtn`과 같은 피드백. null 검사를 넣어서 **에디터에서 IntroScene을 직접 Play해도** 예외가 나지 않게 합니다(research.md 9-E: GameScene 직접 실행 시 NRE 문제를 반복하지 않음).
- 중복 전환 방지: `Finished` 상태로 바뀐 뒤에는 입력을 무시합니다(`LoadScene` 중복 호출 방지).

### 2.6 씬 계층 (`IntroScene.unity`)

```
IntroScene
├─ Main Camera                         (GameStart와 동일한 2D 카메라)
├─ EventSystem                         (InputSystemUIInputModule)
├─ InputManager                        PlayerInput
│     Actions = BlacksmithUpgradeSimulator.inputactions, Default Map = Player
│     Behavior = Invoke Unity Events, Player/SpaceBar → IntroController.OnSpaceBar
├─ IntroController                     IntroController.cs
└─ Canvas                              Screen Space-Overlay, CanvasScaler 1920×1080, Match 1 (GameStart와 동일)
    ├─ BackgroundImage                 Image, 화면 전체 stretch, Preserve Aspect (이미지에 레터박스 포함)
    └─ Balloon                         빈 RectTransform (기본 비활성). 하단 가로 전체, 높이 약 25%
        ├─ BalloonBg                   Image — 반투명 사각형 (아래 "말풍선 스타일" 참고)
        ├─ SpeakerText                 TMP, CookieRun Bold SDF, 주황/노랑, 가운데 정렬, 불투명
        ├─ ContentText                 TMP, CookieRun Regular SDF, 흰색, 가운데 정렬, 불투명
        └─ NextIcon                    Image ▼ (주황), 불투명 + BobbingIcon.cs
```
- 폰트는 기존 `Assets/Fonts/CookieRun * SDF`를 재사용합니다.

#### 말풍선 스타일 (확정: 예시.png와 같이 아래쪽 반투명 사각형, 글씨는 불투명)

예시.png 2번 영역처럼 **화면 아래쪽 가로 전체에 걸친 사각형**만 반투명으로 하고, 그 위의 이름·대사·▼ 아이콘은 **완전히 불투명**하게 표시합니다.

| 요소 | 앵커 / 크기 | 색상 |
|---|---|---|
| `Balloon` | 앵커 bottom-stretch(min (0,0), max (1,0)), 높이 약 270px (1080 기준 25%), 아래쪽 레터박스 바로 위에 붙임 | — (Image 없음) |
| `BalloonBg` | 부모 전체 stretch | 단색 회색/검정 **RGBA (40, 40, 40, 150)** ≈ 알파 0.6. 스프라이트 없이 Image의 기본 흰색 사각형에 Color만 지정 |
| `SpeakerText` | 상단 중앙 | **RGBA (255, 190, 60, 255)** — 예시의 주황/노랑 "플레이어" |
| `ContentText` | 중앙 | **RGBA (255, 255, 255, 255)** 흰색 |
| `NextIcon` | 하단 중앙 | **RGBA (255, 140, 30, 255)** 주황 ▼ |

**글씨가 반투명이 되지 않게 하는 구현 규칙**
- 투명도는 **`BalloonBg` Image의 Color 알파로만** 줍니다. Image의 Color 알파는 그 Image 자신에게만 적용되고 자식/형제 텍스트에는 영향이 없습니다.
- `Balloon`(부모)에 **`CanvasGroup`을 달아 alpha를 낮추면 안 됩니다.** CanvasGroup alpha는 자식 전체(텍스트 포함)를 반투명하게 만듭니다. 켜고 끄는 것은 `SetActive`로만 합니다.
- 텍스트를 배경 Image의 자식이 아닌 **형제**로 두어(위 계층), 나중에 배경에 페이드 연출을 넣더라도 텍스트 투명도와 섞이지 않게 합니다.
- TMP 텍스트의 Vertex Color 알파는 255로 둡니다.
- 정식 말풍선 리소스("인트로 말풍선 이미지_독백")가 나오면 `BalloonBg`의 Sprite만 바꾸고, 알파 규칙은 그대로 유지합니다.

---

## 3. 코드 초안

> 실제 구현 때 기존 스타일(`[SerializeField] private`, 한국어 주석, `SoundManager.Inst` 사용)을 그대로 따릅니다.

### 3.1 `IntroLine.cs`
```csharp
public class IntroLine
{
    public int Order;
    public string Image;   // Resources/Intro/ 아래 파일명(확장자 제외)
    public string Speaker;
    public string Text;    // 비어 있으면 말풍선 없이 이미지만 표시

    public bool HasText => !string.IsNullOrEmpty(Text);
}
```

### 3.2 `IntroScriptReader.cs`
```csharp
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class IntroScriptReader
{
    private const string PATH = "Intro/IntroScript"; // Resources 기준, 확장자 제외

    public static List<IntroLine> Load()
    {
        List<IntroLine> result = new List<IntroLine>();
        TextAsset csv = Resources.Load<TextAsset>(PATH);
        if (csv == null)
        {
            Debug.LogError($"[Intro] CSV를 찾을 수 없습니다: Resources/{PATH}.csv");
            return result;
        }

        string[] rows = csv.text.TrimStart('﻿').Split('\n');
        for (int i = 1; i < rows.Length; i++) // 0행은 헤더
        {
            string row = rows[i].TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(row)) continue;

            List<string> cols = SplitCsvRow(row);
            if (cols.Count < 2 || !int.TryParse(cols[0], out int order))
            {
                Debug.LogWarning($"[Intro] {i + 1}행 형식 오류 → 건너뜀: {row}");
                continue;
            }

            result.Add(new IntroLine
            {
                Order = order,
                Image = cols[1].Trim(),
                Speaker = cols.Count > 2 ? cols[2].Trim() : "",
                Text = cols.Count > 3 ? cols[3] : "",
            });
        }

        result.Sort((a, b) => a.Order.CompareTo(b.Order));
        return result;
    }

    // 큰따옴표로 감싼 필드("a, b")와 이스케이프("")를 지원하는 한 줄 파서
    private static List<string> SplitCsvRow(string row)
    {
        List<string> cols = new List<string>();
        StringBuilder sb = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < row.Length; i++)
        {
            char c = row[i];
            if (inQuotes)
            {
                if (c == '"' && i + 1 < row.Length && row[i + 1] == '"') { sb.Append('"'); i++; }
                else if (c == '"') inQuotes = false;
                else sb.Append(c);
            }
            else if (c == '"') inQuotes = true;
            else if (c == ',') { cols.Add(sb.ToString()); sb.Clear(); }
            else sb.Append(c);
        }
        cols.Add(sb.ToString());
        return cols;
    }
}
```

### 3.3 `IntroController.cs`
```csharp
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    private enum IntroState { Loading, Delay, WaitingInput, Finished }

    [Header("UI")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject balloon;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI contentText;

    [Header("Options")]
    [SerializeField] private float balloonDelay = 1f;       // 배경 전환 후 말풍선이 나오기까지의 시간
    [SerializeField] private string nextSceneName = "GameScene";

    private List<IntroLine> lines;
    private int index;
    private string currentImage;
    private IntroState state = IntroState.Loading;

    private void Start()
    {
        lines = IntroScriptReader.Load();
        if (lines.Count == 0) { GoNextScene(); return; } // 데이터가 없으면 인트로를 건너뜀
        index = 0;
        StartCoroutine(ShowLine());
    }

    // PlayerInput(Invoke Unity Events) → Player/SpaceBar
    public void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (!context.performed || state != IntroState.WaitingInput) return;

        SoundManager.Inst?.PlaySFX(ESfx.Button_Click);
        index++;

        if (index >= lines.Count) GoNextScene();
        else StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        IntroLine line = lines[index];

        // 이미지가 바뀌는 경우에만: 말풍선 OFF → 배경 교체 → 1초 대기
        if (line.Image != currentImage)
        {
            state = IntroState.Delay;
            balloon.SetActive(false);
            backgroundImage.sprite = LoadSprite(line.Image);
            currentImage = line.Image;
            yield return new WaitForSeconds(balloonDelay);
        }

        // 대사가 있으면 말풍선과 텍스트를 한 번에 표시
        balloon.SetActive(line.HasText);
        if (line.HasText)
        {
            speakerText.gameObject.SetActive(!string.IsNullOrEmpty(line.Speaker));
            speakerText.text = line.Speaker;
            contentText.text = line.Text;
        }

        state = IntroState.WaitingInput;
    }

    private void GoNextScene()
    {
        state = IntroState.Finished;
        balloon.SetActive(false);
        SceneManager.LoadScene(nextSceneName);
    }

    // Sprite Mode가 Single이든 Multiple이든 모두 처리
    private Sprite LoadSprite(string imageName)
    {
        string path = "Intro/" + imageName;
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
        {
            Sprite[] all = Resources.LoadAll<Sprite>(path);
            if (all.Length > 0) sprite = all[0];
        }
        if (sprite == null) Debug.LogError($"[Intro] 이미지를 찾을 수 없습니다: Resources/{path}");
        return sprite;
    }
}
```

### 3.4 `BobbingIcon.cs` (▼ 아이콘)
```csharp
using UnityEngine;

public class BobbingIcon : MonoBehaviour
{
    [SerializeField] private float amplitude = 8f; // 위아래 이동 거리(px)
    [SerializeField] private float speed = 4f;

    private RectTransform rect;
    private Vector2 basePos;

    private void Awake()
    {
        rect = (RectTransform)transform;
        basePos = rect.anchoredPosition;
    }

    private void OnEnable() { if (rect != null) rect.anchoredPosition = basePos; }

    private void Update()
    {
        rect.anchoredPosition = basePos + Vector2.up * Mathf.Sin(Time.time * speed) * amplitude;
    }
}
```
말풍선(`Balloon`)의 자식이므로 말풍선이 꺼지면 함께 멈춥니다. 예시.png의 "말풍선이 활성화된 동안 반복 동작" 조건을 그대로 만족합니다.

### 3.5 `StartSceneBtn.cs` 수정
```csharp
public void GameStart()
{
    SoundManager.Inst.PlaySFX(ESfx.Start_Button);
    // 새 게임이면 인트로, 이어하기면 바로 본게임 (6장)
    SceneManager.LoadScene(GameDataManager.IsNewGame() ? "IntroScene" : "GameScene");
}
```

---

## 4. 작업 순서 (체크리스트)

> **진행 상태: 전체 완료 ✅ (2026-10-04)**

1. **에셋 준비** ✅
   - [x] `Intro/1~7.png` → Sprite Mode **Single**, Compression **High Quality**, Mipmap 끔
   - [x] `Assets/Resources/Intro/IntroScript.csv` 작성(UTF-8), 2.4절 내용
   - [x] (추가) ▼ 아이콘 스프라이트 `Assets/Resources/Intro/UI/NextArrow.png` 생성 — 프로젝트에 삼각형 리소스가 없어서 64×40 흰색 삼각형을 만들고 Image color로 주황색 지정
2. **스크립트** ✅ (새 파일은 UTF-8)
   - [x] `Scripts/Intro/IntroLine.cs`, `IntroScriptReader.cs`, `IntroController.cs`, `BobbingIcon.cs`
   - [x] (추가) `IntroController.Next()`를 public으로 분리 — 나중에 클릭/버튼으로 넘기기를 붙일 수 있게
3. **씬** ✅
   - [x] `IntroScene.unity` 생성, 2.6절 계층 구성, 인스펙터 참조 연결
   - [x] `PlayerInput` → `Player/SpaceBar` 이벤트에 `IntroController.OnSpaceBar` 연결(Dynamic CallbackContext)
   - [x] Build Settings에 `IntroScene` 추가(순서: GameStart 0, IntroScene 1, GameScene 2). 활성 Build Profile이 없어 EditorBuildSettings가 사용됨을 확인
4. **진입 연결** ✅
   - [x] `GameDataManager.IsNewGame()` 정적 메서드 추가(6장) — 파일의 CP949 인코딩과 CRLF를 유지한 채 16줄만 삽입
   - [x] `StartSceneBtn.GameStart()` 수정
5. **검증** ✅ (5장 결과 참고)

---

## 5. 검증 시나리오

| # | 시나리오 | 기대 결과 |
|---|---|---|
| T1 | 저장 파일을 지우고 → 타이틀 → 게임 시작 | IntroScene, 1.png 표시 → 1초 뒤 말풍선 "플레이어 / . . ." |
| T1-b | 말풍선 표시 상태 확인 | 아래쪽 사각형만 반투명(배경 이미지가 비쳐 보임), 이름·대사·▼는 선명한 불투명 |
| T2 | T1에서 1초 이내 스페이스 | 무시됨 |
| T3 | 1초 뒤 스페이스 | 2.png로 전환 → 1초 뒤 말풍선 "플레이어 / 아 강화에 또 실패했네..", ▼ 위아래로 움직임 |
| T4 | 계속 진행 | 3~7번 대사가 표 그대로 나옴 |
| T5 | 7번에서 스페이스 | 말풍선 OFF → GameScene, 기존과 같이 1일차 오프닝(대장장이 등장) |
| T6 | 스페이스 연타 | 이미지/대사를 건너뛰지 않고, GameScene이 두 번 로드되지 않음 |
| T7 | 하루 진행 후 종료 → 재실행 → 게임 시작 | 인트로 없이 바로 GameScene(이어하기) |
| T8 | CSV의 3번 대사를 수정 | 빌드 없이 에디터 재생만으로 반영 |
| T9 | Excel에서 "CSV UTF-8"로 저장한 파일 | 한글 정상(BOM 처리 확인) |
| T10 | 에디터에서 IntroScene을 직접 Play | SoundManager 없이도 예외 없이 진행 |
| T11 | CSV에 쉼표가 들어간 대사(`"음, 그렇군"`) | 한 칸으로 정상 표시 |

### 검증 결과 (2026-10-04, Unity 6000.0.58f2 에디터 Play Mode)

입력은 Input System에 실제 Space 키 이벤트(`InputSystem.QueueStateEvent`)를 넣어 `PlayerInput → OnSpaceBar` 경로 그대로 테스트했습니다. 테스트 동안 사용자 저장 파일은 백업했다가 바이트 단위로 원복했습니다.

| # | 결과 | 확인 내용 |
|---|---|---|
| T1 | ✅ | 타이틀 `GameStart()` → `IsNewGame=True` → IntroScene, 1.png + "플레이어 / . . ." |
| T1-b | ✅ | 스크린샷: 하단 띠만 반투명(일러스트가 비침), 이름(주황)·대사(흰색)·▼(주황) 불투명 |
| T2 | ✅ | Space 직후 `state=Delay`, 말풍선 OFF. 지연 중 두 번째 Space → index 변화 없음 |
| T3 | ✅ | 2.png, "아 강화에 또 실패했네.." (스크린샷 확인) |
| T4 | ✅ | 3 "도데체 왜 안되는거야!", 4 "어어? 뭐야!!!", 5 "으으.. 여기가 어디지..", 6 "어 저기는..?", 7 "어라? 이게 어떻게 된거지?" (7번 스크린샷 확인) |
| T5 | ✅ | 7번에서 Space → 말풍선 OFF → GameScene 로드, 1일차 대장장이 오프닝 "오늘도 시작해 볼까!" 정상 |
| T6 | ✅ | 마지막에 Space 2회 연속 → 두 번째는 `Finished` 상태로 무시, GameScene 1개만 로드 |
| T7 | ✅ | 저장 파일을 `2,150,3`으로 바꾼 뒤 게임 시작 → `IsNewGame=False` → 인트로 없이 GameScene |
| T8 | ✅ | CSV 수정 후 재임포트 → 수정된 대사 반영 (테스트 후 원복) |
| T9 | ✅ | BOM + CRLF + 순서가 섞인 CSV → BOM 제거, Order로 정렬되어 정상 로드 |
| T10 | ✅ | IntroScene 단독 Play(`SoundManager.Inst == null`) → 예외 없이 진행 |
| T11 | ✅ | `"음, 그렇군 ""정말"""` → 한 칸, `음, 그렇군 "정말"` |
| 콘솔 | ✅ | 컴파일 오류 0. 새 코드에서 나온 Exception/Warning 0 (기존 `GameManager.eventPopUpTime` CS0414 경고와 MCP 브리지 자체의 WebSocket 경고만 있음) |

---

## 6. 새 게임 판정 — `GameDataManager.IsNewGame()`

`GameDataManager`는 GameScene에만 있으므로 타이틀에서 쓸 수 있게 **정적 메서드**를 추가합니다(인스턴스 경로 필드와는 별개로 같은 파일 경로를 계산).

```csharp
// GameDataManager.cs 에 추가
public static bool IsNewGame()
{
    string path = Path.Combine(Application.persistentDataPath, "GameData.csv");
    if (!File.Exists(path)) return true;                 // 저장 파일이 없음 = 처음 실행

    string[] lines = File.ReadAllLines(path);
    if (lines.Length < 2) return true;
    string[] d = lines[1].Split(',');
    // 1일차, 골드 0, 방문자 0 = 아직 아무것도 진행하지 않은 상태
    return d.Length >= 3 && d[0] == "1" && d[1] == "0" && d[2] == "0";
}
```
- `GameDataManager.cs`는 **CP949 인코딩**입니다. 편집하기 전에 UTF-8로 변환하거나 인코딩을 유지한 채 편집해야 기존 한글 주석이 깨지지 않습니다.
- 1일차 첫 손님이 오기 전에 껐다 켜면 인트로가 다시 나옵니다. 아직 진행한 것이 없으므로 의도에 맞는 동작으로 봅니다.

---

## 7. 위험 요소와 대응

| 위험 | 대응 |
|---|---|
| Sprite Mode Multiple 때문에 `Resources.Load<Sprite>`가 null | 임포트를 Single로 변경 + 코드에서 `LoadAll` 대체 처리 |
| Excel 저장 시 CP949로 바뀌어 한글이 깨짐 | 2.4절 규칙을 문서화하고 BOM 처리. 필요하면 파서에서 깨진 문자(`�`)를 감지해 경고 로그 |
| Build Profiles에 씬을 추가하지 않아 `LoadScene` 실패 | 4장 체크리스트, T1에서 확인 |
| 이미지 7장(약 11MB 원본)을 Resources에 넣어 빌드 용량 증가 | 지금 규모에서는 문제없음. 커지면 씬에서 직접 참조하는 `Sprite[]` 배열 방식으로 전환 가능 |
| 스페이스 연타로 코루틴이 겹침 | `state` 가드(WaitingInput일 때만 입력 처리) |
| 새 스크립트가 다른 인코딩으로 저장됨 | 새 파일은 모두 UTF-8 with BOM (GameManager.cs와 동일) |

---

## 8. 범위 밖 (이번 작업에서 하지 않음)
- 예시.png의 **메뉴 버튼과 일시정지**: 기존 `MenuManager`는 `EnhanceManager`에 의존하고 GameScene 전용으로 만들어져 있어서 그대로 재사용할 수 없습니다. 필요하면 2단계에서 인트로용 간단한 메뉴(계속 / 인트로 건너뛰기 / 타이틀로)를 따로 만드는 것을 제안합니다.
- 페이드 인/아웃, 화면 흔들림(3.png), 섬광(4.png) 같은 추가 연출, BGM/효과음 지정 → CSV에 `Sfx`, `Effect` 열을 추가하는 방식으로 확장할 수 있습니다.
- 스페이스바를 길게 눌러 인트로 전체를 건너뛰는 기능

---

## 9. 결정 사항 (확정, 2026-10-04)

| ID | 항목 | 결정 |
|---|---|---|
| D1 | 1.png 대사 | **". . ."** (CSV 1행 Text) |
| D2 | 인트로 표시 조건 | **새 게임일 때만** (6장 `IsNewGame()`) |
| D3 | 인트로 대사 데이터 | 기존 `GameScript.Json`과 **분리**, `Resources/Intro/IntroScript.csv` |
| D4 | 말풍선 모양 | 예시.png처럼 **아래쪽 사각형 영역만 반투명**, 글씨(이름·대사)와 ▼는 **불투명** (2.6절 "말풍선 스타일") |
