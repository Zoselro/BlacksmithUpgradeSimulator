using System;

#region StartScene, EndingScene JSON 클래스 정의
//[Serializable]
//public class Scenes
//{
//    public SceneJson[] scenes;
//}

//[Serializable]
//public class SceneJson
//{
//    public string sceneName;
//    public Line[] lines;
//}

//[Serializable]
//public class Line
//{
//    public string text;
//}
//#endregion StartScene, EndingScene 클래스 정의

//#region GameScene 스크립트 JSON 클래스 정의
//[Serializable]
//public class DialogueScenes
//{
//    public DialoguesJson[] scenes;
//}
//[Serializable]
//public class DialoguesJson
//{
//    public string sceneName;
//    public Dialogue[] lines;
//}

//[Serializable]
//public class Dialogue
//{
//    public string text;
//    //public string img;
//    public string charName;
//    public string eventId;
//}
#endregion GameScene 스크립트 JSON 클래스 정의

[Serializable]
public class ScriptReaderDialogueLines
{
    public DialogueData[] lines;
}

[Serializable]
public class DialogueData
{
    public string text;
    public string eventID;
    public string NpcTendency;
    public string NpcState;
    public string NpcType;
    public string Gender;
}