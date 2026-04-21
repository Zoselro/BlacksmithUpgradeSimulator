using UnityEngine;

public class EnhancePopupUI : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private EnhanceUIManager enhanceUIManager;
    private WeaponController weapon;
    private NpcData npcData;

    // 무기 정보 보는 확인창
    public void StartEnhanceInteraction(WeaponController weapon)
    {
        uiManager.ShowDialogueBox(false);
        enhanceUIManager.OnClickActiveEnhanceActivePanel(true);
        enhanceUIManager.Initialized(weapon.PrevEnhancementLevel, weapon.Sprite, weapon.GetWeaponRankName(), weapon.GetWeaponTypeName());
    }
}
