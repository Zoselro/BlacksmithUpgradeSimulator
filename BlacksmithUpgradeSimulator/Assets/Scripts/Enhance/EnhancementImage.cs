using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhancementImage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextEnhancementLevel;
    [SerializeField] private TextMeshProUGUI prevEnhancementLevel;
    [SerializeField] private EnhanceResult result;
    [SerializeField] private Image Img;
    public void UpdateEnhancementWeaponUI(WeaponController weapon)
    {
        Img.sprite = weapon.Sprite;
        nextEnhancementLevel.text = weapon.NextEnhancementLevel.ToString() + " 강";
        prevEnhancementLevel.text = weapon.PrevEnhancementLevel.ToString() + " 강";
    }

    public EnhanceResult GetResult()
    {
        return result;
    }
}
