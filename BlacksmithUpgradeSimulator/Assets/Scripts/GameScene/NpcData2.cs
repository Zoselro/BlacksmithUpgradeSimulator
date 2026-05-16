using UnityEngine;

public class NpcData2 : MonoBehaviour
{
    [SerializeField] AdventurerType adventuerType;
    public AdventurerType AdventurerType => adventuerType;

    [SerializeField] private Sprite expression;

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

    public void GetNPCEmotion(Emotion emotion)
    {

        switch (emotion)
        {
            case Emotion.Normal:
                expression = normalSprite;
                break;
            case Emotion.Sad:
                expression = sadSprite;
                break;
            case Emotion.Happy:
                expression = happySprite;
                break;
            default:
                expression = normalSprite;
                break;
        }
    }
}
