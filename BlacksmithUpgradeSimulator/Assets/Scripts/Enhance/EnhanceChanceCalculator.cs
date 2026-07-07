using UnityEngine;


public class EnhanceChanceCalculator : MonoBehaviour
{
    // 장비강화 진행되기 전 성공 확률이 5~40%, 50%, 50%~90% 확률의 세 가지 옵션 중 하나를 랜덤으로 돌리는 기능
    [SerializeField] private float beginnerpLowerProbability;
    [SerializeField] private float beginnerpUpperProbability;

    [SerializeField] private float intermediateLowerProbability;
    [SerializeField] private float intermediateUpperProbability;

    [SerializeField] private float advancedLowerProbability;
    [SerializeField] private float advancedUpperProbability;

    //public float GetRandomEnhanceChance(AdventurerType type)
    //{
    //    // 초급,중급,고급 모험가 일 경우 확률은 다르게 표기.
    //    switch (type)
    //    {
    //        case AdventurerType.Beginner:
    //            return Random.Range(beginnerpLowerProbability, beginnerpUpperProbability);
    //        case AdventurerType.Intermediate:
    //            return Random.Range(intermediateLowerProbability, intermediateUpperProbability);
    //        case AdventurerType.Advanced:
    //            return Random.Range(advancedLowerProbability, advancedUpperProbability);
    //        default: return 0.50f;
    //    }
    //}

    public float GetEnhanceProbablity(int level)
    {
        switch(level)
        {
            case 0: return 0.95f;
            case 1: return 0.9f;
            case 2: return 0.85f;
            case 3: return 0.80f;
            case 4: return 0.75f;
            case 5: return 0.70f;
            case 6: return 0.60f;
            case 7: return 0.55f;
            case 8: return 0.50f;
            case 9: return 0.45f;
            case 10: return 0.40f;
            case 11: return 0.35f;
            case 12: return 0.30f;
            case 13: return 0.20f;
            case 14: return 0.10f;
            default: return 0.10f;
        }
    }
}
