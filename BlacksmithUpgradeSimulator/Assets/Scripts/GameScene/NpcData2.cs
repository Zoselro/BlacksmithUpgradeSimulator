using UnityEngine;
using UnityEngine.UI;

public class NpcData2 : MonoBehaviour
{
    [SerializeField] AdventurerType adventuerType;
    [SerializeField] string nameID;

    public string GetNameID()
    {
        if(nameID == null || nameID == "")
        {
            Debug.LogError("Name ID is not assigned.");
            return "Unknown";
        }
        return nameID;
    }

    public AdventurerType GetAdventurerType()
    {
        return adventuerType;
    }

    [Header("Face Sprites")]
    [SerializeField] private Sprite normalSprite;
    public Sprite NormalSprite => normalSprite;
    [SerializeField] private Sprite happySprite;
    public Sprite HappySprite => happySprite;
    [SerializeField] private Sprite sadSprite;
    public Sprite SadSprite => sadSprite;

    [Header("NPC Parts")]
    [SerializeField] private Image leftLeg;
    public Image LeftLeg => leftLeg;
    [SerializeField] private Image rightLeg;
    public Image RightLeg => rightLeg;
    [SerializeField] private Image leftArm;
    public Image LeftArm => leftArm;
    [SerializeField] private Image rightArm;
    public Image RightArm => rightArm;
    [SerializeField] private Image body;
    public Image Body => body;
    [SerializeField] private Image face;
    public Image Face => face;
    [SerializeField] private Image hair;
    public Image Hair => hair;
    [SerializeField] private Image hair2;
    public Image Hair2 => hair2;
    [SerializeField] private Image expression;


    [SerializeField] private NpcTendency npcTendency;
    public NpcTendency NpcTendency => npcTendency;

    public Sprite ChangeEmotion(Emotion emotion)
    {
        if (expression == null)
        {
            Debug.LogError("Expression sprite is not assigned.");
            return null;
        }

        switch (emotion)
        {
            case Emotion.Normal:
                expression.sprite = normalSprite;
                break;
            case Emotion.Sad:
                expression.sprite = sadSprite;
                break;
            case Emotion.Happy:
                expression.sprite = happySprite;
                break;
            default:
                expression.sprite = normalSprite;
                break;
        }

        return expression.sprite;
    }
}
