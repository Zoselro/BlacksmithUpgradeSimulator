using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    private NpcData2 currentNpcData;

    [Header("NPC Parts")]
    [SerializeField] private Image leftLeg;
    [SerializeField] private RectTransform leftLegRectTransform;

    [SerializeField] private Image rightLeg;
    [SerializeField] private RectTransform rightLegRectTransform;

    [SerializeField] private Image leftArm;
    [SerializeField] private RectTransform leftArmRectTransform;

    [SerializeField] private Image rightArm;
    [SerializeField] private RectTransform rightArmRectTransform;

    [SerializeField] private Image body;
    [SerializeField] private RectTransform bodyRectTransform;

    [SerializeField] private Image face;
    [SerializeField] private RectTransform faceRectTransform;

    [SerializeField] private Image hair;
    [SerializeField] private RectTransform hairRectTransform;

    [SerializeField] private Image hair2;
    [SerializeField] private RectTransform hair2RectTransform;

    [SerializeField] private Image expression;
    [SerializeField] private RectTransform expressionRectTransform;

    [Header("AdventureType")]
    [SerializeField] private AdventurerType adventurerType;

    NpcData2 currentDataTemplate;

    public void ApplyNpcTemplate(NpcData2 template)
    {
        currentDataTemplate = template;

        if(template == null)
        {
            Debug.LogError("NPC Template is null. Cannot apply NPC data.");
            return;
        }

        if (template != null)
        {
            leftLeg.sprite = template.LeftLeg.sprite;
            rightLeg.sprite = template.RightLeg.sprite;
            leftArm.sprite = template.LeftArm.sprite;
            rightArm.sprite = template.RightArm.sprite;
            body.sprite = template.Body.sprite;
            face.sprite = template.Face.sprite;
            hair.sprite = template.Hair.sprite;
            hair2.sprite = template.Hair2.sprite; // hair2도 hair와 동일한 스프라이트를 사용

            adventurerType = template.GetAdventurerType();


        }
        else
        {
            Debug.LogError("NPC Template is null. Cannot apply NPC data.");
        }
    }

    public void Initialize(NpcData2 npcData)
    {
        currentNpcData = npcData;
        // 초기 표정을 Normal로 설정
        SetEmotion(Emotion.Normal);
    }

    // 상황에 따라 표정만 바꾸는 기능
    public Sprite SetEmotion(Emotion emotion)
    {
        if (currentDataTemplate == null || expression == null) return null;

        switch (emotion)
        {
            case Emotion.Normal:
                expression.sprite = currentDataTemplate.NormalSprite;
                break;
            case Emotion.Happy:
                expression.sprite = currentDataTemplate.HappySprite;
                break;
            case Emotion.Sad:
                expression.sprite = currentDataTemplate.SadSprite;
                break;
        }

        return expression.sprite;
    }

    public AdventurerType GetAdventurerType()
    {
        return currentNpcData != null ? currentNpcData.GetAdventurerType() : AdventurerType.Beginner;
    }

    public void InitializeRectTransform(NpcData2 npcData)
    {
        CopyRect(npcData.LeftArmRectTransform, leftArmRectTransform);
        CopyRect(npcData.RightArmRectTransform, rightArmRectTransform);
        CopyRect(npcData.LeftLegRectTransform, leftLegRectTransform);
        CopyRect(npcData.RightLegRectTransform, rightLegRectTransform);
        CopyRect(npcData.BodyRectTransform, bodyRectTransform);
        CopyRect(npcData.FaceRectTransform, faceRectTransform);
        CopyRect(npcData.HairRectTransform, hairRectTransform);
        CopyRect(npcData.Hair2RectTransform, hair2RectTransform);
        CopyRect(npcData.ExpressionRectTransform, expressionRectTransform);
    }

    private void CopyRect(RectTransform source, RectTransform target)
    {
        target.anchorMin = source.anchorMin;
        target.anchorMax = source.anchorMax;
        target.pivot = source.pivot;

        target.anchoredPosition = source.anchoredPosition;
        target.sizeDelta = source.sizeDelta;

        target.localScale = source.localScale;
        target.localRotation = source.localRotation;
    }
}
