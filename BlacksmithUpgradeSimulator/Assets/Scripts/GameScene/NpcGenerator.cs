
using System.Collections.Generic;
using UnityEngine;

public class NpcGenerator : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private NpcController npcController;
    [SerializeField] private NpcData2[] npcDatas;

    [SerializeField] private AdventurerType adventurerType;
    public AdventurerType AdventurerType => adventurerType;

    public WeaponController WeaponController => weaponController;

    public NpcData2 Setting(AdventurerType adventurerType)
    {
        this.adventurerType = adventurerType;

        NpcData2 npcData = PickNpcDataByType(adventurerType); // adventurerType의 대한 랜덤한 npc를 뽑아냄
        
        if(npcData == null)
        {
            Debug.LogError($"[NpcGenerator] {adventurerType} 타입의 NpcData를 찾을 수 없습니다.");
            return null;
        }
        npcController.Initialize(npcData); // 뽑아낸 npc의 데이터를 npcController에 초기화한다.

        npcController.ApplyNpcTemplate(npcData); // 뽑아낸 npc의 데이터를 npcController에 적용한다.

        weaponController.GetEnhancementLevelByAdventurerType(adventurerType); // 고객의 등급에 따라 강화 등급을 결정한다.

        return npcData;
    }

    private NpcData2 PickNpcDataByType(AdventurerType type)
    {
        List<NpcData2> candidates = new List<NpcData2>();
        for (int i = 0; i < npcDatas.Length; i++)
        {
            if (npcDatas[i].GetAdventurerType() == type)
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
