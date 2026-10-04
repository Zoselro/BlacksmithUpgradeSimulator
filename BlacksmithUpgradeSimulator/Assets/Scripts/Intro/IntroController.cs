using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 인트로 진행: 이미지 표시 -> 1초 뒤 말풍선 -> 스페이스바로 다음 -> 마지막 이후 GameScene
public class IntroController : MonoBehaviour
{
    private enum IntroState { Loading, Delay, WaitingInput, Finished }

    [Header("UI")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private GameObject balloon;
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI contentText;

    [Header("Options")]
    [SerializeField] private float balloonDelay = 1f; // 배경 전환 후 말풍선이 나오기까지의 시간
    [SerializeField] private string nextSceneName = "GameScene";

    private List<IntroLine> lines;
    private int index;
    private string currentImage;
    private IntroState state = IntroState.Loading;

    private void Start()
    {
        balloon.SetActive(false);
        lines = IntroScriptReader.Load();
        if (lines.Count == 0) // 데이터가 없으면 인트로를 건너뜀
        {
            GoNextScene();
            return;
        }

        index = 0;
        StartCoroutine(ShowLine());
    }

    // PlayerInput(Invoke Unity Events) -> Player/SpaceBar
    public void OnSpaceBar(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        Next();
    }

    // 다음 대사로 진행 (입력 대기 상태일 때만)
    public void Next()
    {
        if (state != IntroState.WaitingInput)
            return;

        if (SoundManager.Inst != null)
            SoundManager.Inst.PlaySFX(ESfx.Button_Click);

        index++;
        if (index >= lines.Count)
            GoNextScene();
        else
            StartCoroutine(ShowLine());
    }

    private IEnumerator ShowLine()
    {
        IntroLine line = lines[index];

        // 이미지가 바뀌는 경우에만: 말풍선 OFF -> 배경 교체 -> 대기
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
            if (all.Length > 0)
                sprite = all[0];
        }

        if (sprite == null)
            Debug.LogError($"[Intro] 이미지를 찾을 수 없습니다: Resources/{path}");
        return sprite;
    }
}
