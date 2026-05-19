using UnityEngine;
using UnityEngine.UI;

public class NpcController : MonoBehaviour
{
    private NpcData2 currentNpcData;

    [Header("NPC Parts")]
    [SerializeField] private Image leftLeg;
    [SerializeField] private Image rightLeg;
    [SerializeField] private Image leftArm;
    [SerializeField] private Image rightArm;
    [SerializeField] private Image body;
    [SerializeField] private Image face;
    [SerializeField] private Image hair;
    [SerializeField] private Image hair2;
    [SerializeField] private Image expression;

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
}
