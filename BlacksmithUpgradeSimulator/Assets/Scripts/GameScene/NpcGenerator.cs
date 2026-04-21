
using System.Collections.Generic;
using UnityEngine;

public enum EmotionType
{
    Normal,
    Happy,
    Angry
}

public class NpcGenerator : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private NpcController npcController;
    [SerializeField] private NpcData[] npcDatas;

    public NpcController NpcController => npcController;

    [SerializeField] private AdventurerType adventurerType;


    public WeaponController WeaponController => weaponController;
    public AdventurerType AdventurerType => adventurerType;

    public NpcData Setting(AdventurerType adventurerType)
    {
        this.adventurerType = adventurerType;

        NpcData npcData = PickNpcDataByType(adventurerType); // adventurerType의 대한 랜덤한 npc를 뽑아냄
        weaponController.GetEnhancementLevelByAdventurerType(adventurerType); // 고객의 등급에 따라 강화 등급을 결정한다.
        return npcData;
    }

    private NpcData PickNpcDataByType(AdventurerType type)
    {
        List<NpcData> candidates = new List<NpcData>();
        for (int i = 0; i < npcDatas.Length; i++)
        {
            if (npcDatas[i].AdventurerType == type)
            {
                candidates.Add(npcDatas[i]);
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogError($"[NpcGenerator] {type} 타입의 NpcData가 없습니다.");
            return null;
        }

        int index = Random.Range(0, candidates.Count);
        return candidates[index];
    }
}
