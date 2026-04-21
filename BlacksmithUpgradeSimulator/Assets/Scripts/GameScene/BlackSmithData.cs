using UnityEngine;

[CreateAssetMenu(fileName = "BlackSmithData", menuName = "Project Forge/BlackSmithData")]
public class BlackSmithData : ScriptableObject
{
    [SerializeField] string nameID;
    public string NameID => nameID; // 대장장이 닉네임


    [SerializeField] private Sprite backSprite;
    public Sprite BackSprite => backSprite;
    [SerializeField] private Sprite normalSprite; // 대장장이 슬픔,웃음,Normal 이미지
    public Sprite NormalSprite => normalSprite;
    [SerializeField] private Sprite happySprite;
    public Sprite HappySprite => happySprite;
    [SerializeField] private Sprite sadSprite;
    public Sprite SadSprite => sadSprite;
    [SerializeField] private string openID; // 대장간 오픈 eventID
    public string OpenID => openID;
    [SerializeField] private string closeID; // 대장간 닫을 때 eventID
    public string CloseID => closeID;
    [SerializeField] string welcomeID; // 손님 방문 시 출력되는 eventID
    public string WelcomeID => welcomeID;
    [SerializeField] private string completeFailID; // 강화에 실패했을 때 출력되는 eventID
    public string CompleteFailID => completeFailID;
    [SerializeField] private string completeSuccessID; // 강화에 성공했을 때 출력되는 eventID
    public string CompleteSuccessID => completeSuccessID;
    [SerializeField] string enhanceSuccessID; // 강화에 성공했을 때 출력되는 eventID
    public string EnhanceSuccessID => enhanceSuccessID;
    [SerializeField] private string enhaceGreatSuccess; // 강화에 대성공 했을 때 출력되는 eventID
    public string EnhanceGreatSuccessID => enhaceGreatSuccess; 
    [SerializeField] string enhanceFailID; // 강화에 실패했을 때 출력되는 eventID
    public string EnhanceFailID => enhanceFailID;
}
