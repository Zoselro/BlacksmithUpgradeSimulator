using System.Collections.Generic;
using UnityEngine;

public class NpcGenerator : MonoBehaviour
{
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private NpcController npcController;
    [SerializeField] private NpcData[] npcDatas;

    [SerializeField] private AdventurerType adventurerType;
    public AdventurerType AdventurerType => adventurerType;

    public WeaponController WeaponController => weaponController;

    private Dictionary<AdventurerType, List<NpcData>> npcDataMap;

    //List<NpcData> candidates = new List<NpcData>();

    public NpcData Setting(AdventurerType adventurerType)
    {
        this.adventurerType = adventurerType;

        NpcData npcData = PickNpcDataByType(adventurerType); // adventurerType의 대한 랜덤한 npc를 뽑아냄
        
        if (npcData == null)
        {
            Debug.LogError($"[NpcGenerator] {adventurerType} 타입의 NpcData를 찾을 수 없습니다.");
            return null;
        }
        npcController.Initialize(npcData); // 뽑아낸 npc의 데이터를 npcController에 초기화한다.

        npcController.ApplyNpcTemplate(npcData); // 뽑아낸 npc의 데이터를 npcController에 적용한다.
        npcController.InitializeRectTransform(npcData); // 뽑아낸 npc 이미지들의 위치를 npcController에 적용한다.

        weaponController.GetEnhancementLevelByAdventurerType(adventurerType); // 고객의 등급에 따라 강화 등급을 결정한다.

        return npcData;
    }

    //private NpcData PickNpcDataByType(AdventurerType type)
    //{
    //    candidates.Clear(); // 후보 리스트를 초기화
    //    for (int i = 0; i < npcDatas.Length; i++)
    //    {
    //        if (npcDatas[i].GetAdventurerType() == type)
    //        {
    //            candidates.Add(npcDatas[i]);
    //        }
    //    }

    //    if (candidates.Count == 0)
    //    {
    //        Debug.LogError($"[NpcGenerator] {type} 타입의 NpcData가 없습니다.");
    //        return null;
    //    }

    //    int index = Random.Range(0, candidates.Count);
    //    return candidates[index];
    //}

    private NpcData PickNpcDataByType(AdventurerType type)
    {
        if (!npcDataMap.TryGetValue(type, out List<NpcData> candidates))
        {
            Debug.LogError($"{type} 타입의 NPC가 없습니다.");
            return null;
        }

        int index = Random.Range(0, candidates.Count);
        return candidates[index];
    }

    public void InitializeNpcDataMap()
    {
        npcDataMap = new Dictionary<AdventurerType, List<NpcData>>();

        foreach (NpcData npcData in npcDatas)
        {
            AdventurerType type = npcData.GetAdventurerType();

            if (!npcDataMap.ContainsKey(type))
            {
                npcDataMap[type] = new List<NpcData>();
            }

            npcDataMap[type].Add(npcData);
        }
    }
}
