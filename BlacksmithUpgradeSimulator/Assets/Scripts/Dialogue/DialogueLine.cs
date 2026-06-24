using UnityEngine;

public enum Speaker
{
    BlackSmith, // 대장장이
    Npc // NPC
}

public enum Dir // 말하는 사람의 위치
{
    Left,
    Right
}

public class DialogueLine
{
    private string nickName;
    public string NicName => nickName;
    private string content;
    public string Content => content;
    private Sprite sprite;
    public Sprite Sprite => sprite;
    private Speaker speak;
    public Speaker Speak => speak;
    private Dir dir;
    public Dir Dir => dir;

    private bool active;
    public bool Active => active;
    public bool IsValid => !string.IsNullOrEmpty(content);

    public void Set(string nickName, string content, Sprite sprite, Speaker speak, Dir dir)
    {
        this.nickName = nickName;
        this.content = content.Replace(".", ".\n").Replace("!","!\n");
        Debug.Log(this.content);
        this.sprite = sprite;
        this.speak = speak;
        this.dir = dir;
    }
    
    public Sprite GetImage()
    {
        return sprite;
    }

    public void Reset()
    {
        nickName = null;
        content = null;
        sprite = null;
    }
}
