using UnityEngine;


public class DialogueLine
{
    private string nickName;
    public string NicName => nickName;
    private string content;
    public string Content => content;
    private Sprite sprite;
    public Sprite Sprite => sprite;
    private Direction dir;
    public Direction Dir => dir;
    private bool active;
    public bool Active => active;
    public bool IsValid => !string.IsNullOrEmpty(content);

    public void Set(string nickName, string content, Sprite sprite, Direction dir)
    {
        this.nickName = nickName;
        this.content = content;
        this.sprite = sprite;
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
