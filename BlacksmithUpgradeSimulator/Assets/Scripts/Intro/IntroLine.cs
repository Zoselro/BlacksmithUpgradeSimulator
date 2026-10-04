// 인트로 대사 한 줄 (IntroScript.csv의 한 행)
public class IntroLine
{
    public int Order;      // 진행 순서
    public string Image;   // Resources/Intro/ 아래 이미지 파일명 (확장자 제외)
    public string Speaker; // 말풍선 이름 칸
    public string Text;    // 대사 (비어 있으면 말풍선 없이 이미지만 표시)

    public bool HasText => !string.IsNullOrEmpty(Text);
}
