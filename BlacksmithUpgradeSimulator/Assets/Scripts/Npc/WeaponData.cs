using UnityEngine;

public enum WeaponType
{
    LongSword,
    ShortSword,
    Bow,
    Rapier,
    Crossbow
}
public enum WeaponRank
{
    Common,
    Rare,
    Epic
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Project Forge/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private Sprite sprite;
    [SerializeField] private WeaponRank weaponRank;

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public Sprite GetWeaponSprite()
    {
        return sprite;
    }

    public WeaponRank GetWeaponRank()
    {
        return weaponRank;
    }
}
