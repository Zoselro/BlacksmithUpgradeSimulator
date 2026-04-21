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
    public float GetRandomEnhanceChance(AdventurerType type)
    {
        // 초급,중급,고급 모험가 일 경우 확률은 다르게 표기.
        switch (type)
        {
            case AdventurerType.Beginner: 
                return Random.Range(beginnerpLowerProbability, beginnerpUpperProbability);
            case AdventurerType.Intermediate: 
                return Random.Range(intermediateLowerProbability, intermediateUpperProbability);                      
            case AdventurerType.Advanced:
                return Random.Range(advancedLowerProbability, advancedUpperProbability);
            default: return 0.50f;
        }
    }
}
