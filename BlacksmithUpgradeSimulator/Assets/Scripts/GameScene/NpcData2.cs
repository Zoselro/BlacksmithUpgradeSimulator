using UnityEngine;

public class NpcData2 : MonoBehaviour
{
    [SerializeField] AdventurerType adventuerType;
    public AdventurerType AdventurerType => adventuerType;
    [SerializeField] string nameID;
    public string NameID => nameID;
    [SerializeField] private Sprite normalSprite; // 대장장이 슬픔,웃음,Normal 이미지
    public Sprite NormalSprite => normalSprite;
    [SerializeField] private Sprite happySprite;
    public Sprite HappySprite => happySprite;
    [SerializeField] private Sprite sadSprite;
    public Sprite SadSprite => sadSprite;
    [SerializeField] private NpcTendency npcTendency;
    public NpcTendency NpcTendency => npcTendency;

    public Sprite GetNPCEmotion(Emotion emotion)
    {
        Sprite sprite = null;

        switch (emotion)
        {
            case Emotion.Normal:
                sprite = normalSprite;
                break;
            case Emotion.Sad:
                sprite = sadSprite;
                break;
            case Emotion.Happy:
                sprite = happySprite;
                break;
            default:
                sprite = normalSprite;
                break;
        }
        return sprite;
    }
}
