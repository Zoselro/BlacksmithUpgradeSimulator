using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private WeaponData[] weapons;
    private int prevEnhancementLevel = 0; // 강화 전 레벨
    private int nextEnhancementLevel = 0; // 강화 후 레벨
    public int PrevEnhancementLevel => prevEnhancementLevel;
    public int NextEnhancementLevel => nextEnhancementLevel;

    private Sprite sprite = null;
    public Sprite Sprite => sprite;
    private WeaponRank weaponRank;

    private WeaponData weaponData;

    public void GetEnhancementLevelByAdventurerType(AdventurerType type)
    {
        //sprite = sprites[Random.Range(0, sprites.Length)]; // 무기 랜덤 이미지
        if (type == AdventurerType.Beginner)
        {
            prevEnhancementLevel = Random.Range(0, 4); // +0 ~ +3
            weaponRank = WeaponRank.Common;
        }
        else if (type == AdventurerType.Intermediate)
        {
            prevEnhancementLevel = Random.Range(3, 7); // +3 ~ +6
            weaponRank = WeaponRank.Rare;
        }
        else if (type == AdventurerType.Advanced)
        {
            prevEnhancementLevel = Random.Range(6, 10); // +6 ~ +9
            weaponRank = WeaponRank.Epic;
        }
        weaponData = GetWeapon(weaponRank);
        sprite = weaponData.GetWeaponSprite();
    }

    public void SetEnhancementStages(int cnt)
    {
        nextEnhancementLevel = prevEnhancementLevel;
        nextEnhancementLevel += cnt;
    }

    // WeaponRank에 따라서 Weapons에서 각각 일반,레어,에픽에 맞는 무기 1종을 가져와야 한다.

    private WeaponData GetWeapon(WeaponRank rank)
    {
        List<WeaponData> filtered = new List<WeaponData>();

        foreach (WeaponData weapon in weapons)
        {
            if (weapon.GetWeaponRank() == rank)
            {
                filtered.Add(weapon);
            }
        }

        if (filtered.Count == 0)
        {
            Debug.LogError("해당 등급 무기 없음");
            return null;
        }
        WeaponData wd = filtered[Random.Range(0, filtered.Count)];
        Debug.Log($"선택된 무기: {wd.name}, 등급: {wd.GetWeaponRank()}, 타입: {wd.GetWeaponType()}");
        return wd;
    }

    public WeaponType GetWeaponType()
    {
        if (weaponData == null)
        {
            Debug.LogError("무기 데이터가 설정되지 않았습니다.");
            return default;
        }
        return weaponData.GetWeaponType();
    }

    public string GetWeaponTypeName()
    {
        if (weaponData == null)
        {
            Debug.LogError("무기 데이터가 설정되지 않았습니다.");
            return "Unknown";
        }
        switch (weaponData.GetWeaponType())
        {
            case WeaponType.LongSword: return "장검";
            case WeaponType.ShortSword: return "단검";
            case WeaponType.Bow: return "활";
            case WeaponType.Rapier: return "레이피어";
            case WeaponType.Crossbow: return "석궁";
        }
        return "";
    }

    public WeaponRank GetWeaponRank()
    {
        if (weaponData == null)
        {
            Debug.LogError("무기 데이터가 설정되지 않았습니다.");
            return default;
        }
        return weaponData.GetWeaponRank();
    }

    public string GetWeaponRankName()
    {
        if (weaponData == null)
        {
            Debug.LogError("무기 데이터가 설정되지 않았습니다.");
            return "Unknown";
        }
        switch (weaponData.GetWeaponRank())
        {
            case WeaponRank.Common: return "일반";
            case WeaponRank.Rare: return "레어";
            case WeaponRank.Epic: return "에픽";
        }
        return "";
    }
}
