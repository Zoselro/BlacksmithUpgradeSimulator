using System.Collections.Generic;
using UnityEngine;
public enum EnhanceResult
{
    Fail,
    Success,
    GreatSuccess
}

public class EnhanceManager : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private EnhanceUIManager enhanceUIManager;
    [SerializeField] private EnhanceButton enhanceButton;
    [SerializeField] private EnhancementImage[] enhanceResultPopups; // 성공, 실패, 대 성공 팝업창 프리팹이 들어있는 오브젝트 배열
    [SerializeField] private TopUIManager topUIManager;
    [SerializeField] private Transform canvasParent; // EnhancementImage 프리팹이 생성될 때 부모로 설정할 캔버스의 Transform

    [SerializeField] private float greatSuccessRatio; // 대성공 확률
    [SerializeField] private float bonusProbablity; // 보너스 강화 확률
    private Dictionary<EnhanceResult, EnhancementImage> prefabMap;

    private EnhancementImage enhancementImage;
    private EnhanceResult result; // 강화 결과
    public EnhanceResult Result => result;

    private float currentEnhanceTime;

    private bool isEnhancing;
    public bool IsEnhancing => isEnhancing;

    public void RequestEnhance(bool isEnhance)
    {
        if (isEnhance)
        {
            isEnhancing = true;
            Debug.Log("강화 시작");
            return;
        }
        else
        {
            isEnhancing = false;
            Debug.Log("강화 중단");
            return;
        }
    }

    public void PlayEnhance(float successProb, AdventurerType adventurerType, WeaponController weapon,
                            int gold, int failCnt,
                            int successCnt, int greatSuccessCnt,
                            int currentGold, int currentSuccessCnt,
                            int currentGreatSuccessCnt,
                            int currentFailCnt)
    {
        if (isEnhancing)
        {
            currentEnhanceTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentEnhanceTime / gm.EnhanceTime);
            enhanceUIManager.EnhanceProgressBar(progress);

            // 만약 강화 시간초가 지났다면?
            if (progress >= 1f)
            {
                isEnhancing = false;
                currentEnhanceTime = 0; // 강화 시간 초기화
                BuildEnhanceResultPrefabMap(); // 성공, 대성공, 실패 prefab 세팅
                result = Enhance(successProb, greatSuccessRatio, failCnt, successCnt, greatSuccessCnt,
                                            currentFailCnt, currentSuccessCnt, currentGreatSuccessCnt);
                Debug.Log($"강화 결과 : {result}");

                // GameManager에게 강화 결과를 전달.
                // GameManager는 전달 받은 강화결과로 대사 세팅
                gm.SetPostEnhancementDialogue(result);
                gm.OnEnhanceResult(result);

                EnhanceEquipment(adventurerType, result, gold, currentGold, weapon);
                enhancementImage.UpdateEnhancementWeaponUI(weapon); // 무기 이미지 및 강화 결과 세팅
                enhanceUIManager.ActiveConfirmButton(true);
                //topUIManager.SetGoldText(gm.Gold);
            }
        }
    }

    public void BuildEnhanceResultPrefabMap()
    {
        prefabMap = new Dictionary<EnhanceResult, EnhancementImage>();

        foreach (EnhancementImage prefab in enhanceResultPopups)
        {
            EnhancementImage enhancementImage = prefab;
            if (enhancementImage == null)
                continue;
            prefabMap[enhancementImage.GetResult()] = prefab;
        }
    }

    // 확률, 대성공 확률, 실패, 성공, 대성공 카운트
    // 에 대하여 EnhanceResult 값을 EnhanceEquipment() 에 넘겨줌
    public EnhanceResult Enhance(float successProb, float greatSuccessRatio,
                            int failCnt, int successCnt,
                            int greatSuccessCnt, 
                            int currentFailCnt, int currentSuccessCnt, 
                            int currentGreatSuccessCnt)
    {
        float roll = Random.value;

        if (roll > successProb)
        {
            gm.SetFailCnt(1);
            gm.SetCurrentFailCnt(1);
            return EnhanceResult.Fail;
        }

        float greatSuccessProb = successProb * greatSuccessRatio;

        if (roll <= greatSuccessProb)
        {
            gm.SetGreatSuccessCnt(1);
            gm.SetCurrentGreatSuccessCnt(1);
            return EnhanceResult.GreatSuccess;

        }
        gm.SetCurrentSuccessCnt(1);
        gm.SetSuccessCnt(1);
        return EnhanceResult.Success;
    }

    // 장비강화 기능 구현
    // NPC 타입, 성공확률, EnhanceEquipment, Add 할 골드, 무기를 넘겨줌.
    public void EnhanceEquipment(AdventurerType adventurerType, EnhanceResult result, 
                                int gold, int currentGold, WeaponController weapon)
    {
        ESfx EnhanceResultSFX = ESfx.Fail;

        // result : 그 확률에 대한 성공, 실패, 대성공 의 여부 결과값
        switch (result)
        {
            case EnhanceResult.GreatSuccess:
                if (adventurerType == AdventurerType.Beginner)
                {
                    gm.SetGold(100);
                    gm.SetCurrentGold(100);
                    weapon.SetEnhancementStages(2);
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                }
                else if (adventurerType == AdventurerType.Intermediate)
                {
                    gm.SetGold(200);
                    gm.SetCurrentGold(200);
                    weapon.SetEnhancementStages(2);
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                }
                else
                {
                    gm.SetGold(250);
                    gm.SetCurrentGold(250);
                    weapon.SetEnhancementStages(2);
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                }
                EnhanceResultSFX = ESfx.GreatSuccess;
                break;

            case EnhanceResult.Success:
                //성공하면 보상금 획득
                if (adventurerType == AdventurerType.Beginner)
                {
                    gm.SetGold(50);
                    gm.SetCurrentGold(50);
                    weapon.SetEnhancementStages(1);
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                }
                else if (adventurerType == AdventurerType.Intermediate)
                {
                    gm.SetGold(100);
                    gm.SetCurrentGold(100);
                    weapon.SetEnhancementStages(1);
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                }
                else
                {
                    gm.SetGold(100);
                    gm.SetCurrentGold(100);
                    weapon.SetEnhancementStages(1);
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                }
                EnhanceResultSFX = ESfx.Success;
                break;

            case EnhanceResult.Fail:
                if (adventurerType == AdventurerType.Beginner)
                {
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                    weapon.SetEnhancementStages(0);
                }
                else if (adventurerType == AdventurerType.Intermediate)
                {
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                    weapon.SetEnhancementStages(0);
                }
                else
                {
                    enhancementImage = Instantiate(prefabMap[result], canvasParent, false).GetComponent<EnhancementImage>();
                    weapon.SetEnhancementStages(0);
                }
                EnhanceResultSFX = ESfx.Fail;
                break;
        }
        SoundManager.Inst.PlaySFX(EnhanceResultSFX);
        int lastIndex = enhancementImage.transform.parent.childCount - 1;

        // 맨 아래에서 두 번째
        enhancementImage.transform.SetSiblingIndex(lastIndex - 1);
    }
    public void EnhancementImageActive(bool active)
    {
        if (enhancementImage != null)
        {
            Destroy(enhancementImage.gameObject);
            enhancementImage = null;
            //enhancementImage.gameObject.SetActive(active);
        }
    }

    public void BonusProbablity(float probablity)
    {
        probablity += bonusProbablity;
    }
}
