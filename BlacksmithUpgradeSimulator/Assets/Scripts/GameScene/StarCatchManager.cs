using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StarCatchManager : MonoBehaviour
{
    [SerializeField] private Image successArea;
    [SerializeField] private Slider starSlider;
    [SerializeField] private RectTransform successAreaRectTransform;
    [SerializeField] private RectTransform starHandleRectTransform;
    [SerializeField] private RectTransform enhancementProgressBar;
    public RectTransform EnhancementProgressBar => enhancementProgressBar;

    [SerializeField] private EnhanceManager enhanceManager;

    [SerializeField] private float plusProbability;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float waitTime = 0.5f; // 해당 시간초 후에 다시 미니게임 시작하기 위한 플래그 변수
    [SerializeField] private int maxEnhancementSteps = 3;
    public int MaxEnhacementSteps => maxEnhancementSteps;

    [SerializeField] private GameObject startCatchObject;
    [SerializeField] private GameObject enhancementProgressBarObj;

    private float currentWaitTime;
    private bool isRight; // Slider가 왼쪽으로 갔는지에 대한 플래그 변수
    private bool isSpaceBarPress; // 스페이스바를 눌렀을 때 플래그 변수
    private bool isSpacePressedCount;
    public bool IsSpacePressedCount => isSpacePressedCount;
    private float currentEnhancementStep = 0f; // enhancementProgressBar의 게이지가 채워지는 정도

    int spaceBarPressCount = 0;


    private void Update()
    {
       StarCatchStart();
    }

    public void StarCatchStart()
    {
        if (!isSpaceBarPress) // 스페이스바를 누르지 않았을 경우
        {
            if (!isRight) // Slider가 오른쪽으로 아직 안갔으면?
            {
                starSlider.value += Time.deltaTime * speed;

                if (starSlider.value >= 1f)
                {
                    starSlider.value = 1f;
                    isRight = true;
                }
            }
            else // Slider가 오른쪽으로 갔다면?
            {
                starSlider.value -= Time.deltaTime * speed;

                if (starSlider.value <= 0f)
                {
                    starSlider.value = 0f;
                    isRight = false;
                }
            }
        }
        else if (isSpaceBarPress)// 스페이스바를 눌렀을 경우
        {

            if (currentWaitTime <= waitTime) // 0.5초뒤에 게임 재 시작
            {
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime >= waitTime)
                {
                    isSpaceBarPress = false; // 스페이스바를 눌렀을 때 플래그 변수 초기화
                    currentWaitTime = 0f; // 시간 초기화
                    starSlider.value = 0f; // Slider 위치 초기화
                }
            }
        }
    }

    public void SpaceBarEvent(InputAction.CallbackContext context)
    {
        if (context.performed && !isSpaceBarPress)
        {
            spaceBarPressCount += 1;

            bool inside = IsHandleCenterInsideArea(successAreaRectTransform, starHandleRectTransform);

            if (inside)
            {
                //Debug.Log("Success");
                enhanceManager.BonusProbablity(plusProbability);
                //topBarUIManager.ProbabilityText.text = "강화 확률 : " + (enhanceManager.GetProbability() * 100).ToString("F1") + "%";
                enhancementProgressBar.localScale += new Vector3(currentEnhancementStep, 0f, 0f);
            }
            else
            {
                //Debug.Log("Fail");
            }
            if (spaceBarPressCount >= maxEnhancementSteps) // 만약 스페이스바를 해당 수 만큼 눌렀다면?
            {
                // 일어날 일 수행
                startCatchObject.gameObject.SetActive(false);
                enhancementProgressBarObj.gameObject.SetActive(false);
                isSpacePressedCount = true;
                return;
            }
            isSpaceBarPress = true;
        }
    }

    private bool IsHandleCenterInsideArea(RectTransform area, RectTransform handle)
    {
        // 1) 성공영역의 월드 코너(좌하/좌상/우상/우하)
        Vector3[] corners = new Vector3[4];
        area.GetWorldCorners(corners);

        // area 월드 Rect 만들기
        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[2].y;

        // 2) 핸들의 중심 월드 좌표
        Vector3 handleCenter = handle.TransformPoint(handle.rect.center);

        // 3) 포함 여부
        return (handleCenter.x >= minX && handleCenter.x <= maxX &&
                handleCenter.y >= minY && handleCenter.y <= maxY);
    }

    public void SetCurrentEnhancementStep()
    {
        currentEnhancementStep = 1f / maxEnhancementSteps;
    }
}