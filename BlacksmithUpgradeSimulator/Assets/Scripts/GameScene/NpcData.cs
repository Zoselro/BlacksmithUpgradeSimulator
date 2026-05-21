using UnityEngine;
using UnityEngine.UI;

public class NpcData : MonoBehaviour
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
    [SerializeField] private RectTransform leftLegRectTransform;
    public RectTransform LeftLegRectTransform => leftLegRectTransform;

    [SerializeField] private Image rightLeg;
    public Image RightLeg => rightLeg;
    [SerializeField] private RectTransform rightLegRectTransform;
    public RectTransform RightLegRectTransform => rightLegRectTransform;

    [SerializeField] private Image leftArm;
    public Image LeftArm => leftArm;
    [SerializeField] private RectTransform leftArmRectTransform;
    public RectTransform LeftArmRectTransform => leftArmRectTransform;

    [SerializeField] private Image rightArm;
    public Image RightArm => rightArm;
    [SerializeField] private RectTransform rightArmRectTransform;
    public RectTransform RightArmRectTransform => rightArmRectTransform;

    [SerializeField] private Image body;
    public Image Body => body;
    [SerializeField] private RectTransform bodyRectTransform;
    public RectTransform BodyRectTransform => bodyRectTransform;

    [SerializeField] private Image face;
    public Image Face => face;
    [SerializeField] private RectTransform faceRectTransform;
    public RectTransform FaceRectTransform => faceRectTransform;

    [SerializeField] private Image hair;
    public Image Hair => hair;
    [SerializeField] private RectTransform hairRectTransform;
    public RectTransform HairRectTransform => hairRectTransform;

    [SerializeField] private Image hair2;
    public Image Hair2 => hair2;
    [SerializeField] private RectTransform hair2RectTransfrom;
    public RectTransform Hair2RectTransform => hair2RectTransfrom;

    [SerializeField] private Image expression;
    [SerializeField] private RectTransform expressionRectTransform;
    public RectTransform ExpressionRectTransform => expressionRectTransform;

    [SerializeField] private NpcTendency npcTendency;
    public NpcTendency NpcTendency => npcTendency;
}
