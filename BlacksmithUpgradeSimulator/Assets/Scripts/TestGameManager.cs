using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    // public 연출 관리 클래스 연출
    // public 대장장이 클래스 대장장이

    // public NPC추출기 NPC추출기
    // public DialogueTable 테이블
    // public DialogueUI dialogueUI;

    //private void Start()
    //{
    //    // 연출.Show() // 연출이 종료되면 대장장이 대화 표시.
    //}

    //public void 대장장이대화(대화Type type)
    //{
    //    string text = 대장장이.대화가져오기(type);
    //    dialogueUI.대화출력(text); -> ?
    //}
    
    // 단계
    // 대장장이
    // NPC

    // 대장장이 인사 - [NPC 등장 - NPC 맡기기] - 강화 단계로 넘어가기
    // NPC.등장()

    // List<대화정보> list = new List<string>();

    // 대장장이 인사 대사 저장.
    // 대화정보 dialogueData;

    // string id = 대장장이.GetId(인사)
    // string text = 테이블.GetText(id)
    // string name = 대장장이.GetName()
    // Sprite sprite = 대장장이.GetSprite()

    // dialogueData.text = text;
    // dialogueData.text = text;
    // dialogueData.text = text;

    // list.Add(dialogueData);

    // public struct NPCData
    // - 성향
    // - 타입

    // NPC 등장 대사 저장
    // result = 강화 매니저.GetResult()
    // NPC.GetData() // 성향, 타입
    // string text = 테이블.GetNPCText(성향, 타입, 강화 매니저.GetResult())
    // list.Add(dialogueData);

    // NPC 맡기기 대사 저장
    // NPC.GetData() // 성향, 타입
    // string text = 테이블.GetNPCText(성향, 타입, 맡기기)
    // list.Add(dialogueData);

    // 대화 관리자.StartDialogue(list);


    // class 대화 관리자

    // public DialogueUI dialogueUI;
    // private List<대화 정보> list;
    // private int index;

    // OnButtonClick()
    // {
    //      index++;
    //      if(index < list.Count - 1)
    //         index = 0;
    //      
    //      dialogueData = list[index];
    //      dialogueUI.대화 출력(dialogueData.이름, dialogueData.이미지, dialogueData.텍스트)
    // }

    // 강화씬에서도 마찬가지로 성향, 타입, 이벤트 는 다르지 않다.
    // 강화 씬에서의 이벤트 -> 강화 결과 값을 미리 가져와서 세팅 해본다.
}
