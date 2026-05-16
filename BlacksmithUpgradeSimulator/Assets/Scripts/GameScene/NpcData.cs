using UnityEngine;

[CreateAssetMenu(fileName = "NpcData", menuName = "Project Forge/NpcData")]
public class NpcData : ScriptableObject
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


    public Sprite GetNPCSprite(Emotion emotion)
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


// 구지 성별을 구별할 필요가 없을 수 도 있다. -> eventID에 따라 여성 대사 또는 남성 대사를 외칠지에 대해 구별이 가능.
// 대사 테이블 eventId를 통해 Message를 가져온다?