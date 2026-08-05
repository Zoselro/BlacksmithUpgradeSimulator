using System.IO;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "GameData.csv");
        Debug.Log($"filePath : {filePath}");
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "Day,Gold,Visitors\n0,0,0");
        }
    }

    #region Get

    public int GetDay()
    {
        string[] lines = File.ReadAllLines(filePath);
        string[] data = lines[1].Split(',');

        return int.Parse(data[0]);
    }

    public int GetGold()
    {
        string[] lines = File.ReadAllLines(filePath);
        string[] data = lines[1].Split(',');

        return int.Parse(data[1]);
    }

    public int GetVisitors()
    {
        string[] lines = File.ReadAllLines(filePath);
        string[] data = lines[1].Split(',');

        return int.Parse(data[2]);
    }

    #endregion

    #region Set

    public void SetDay(int day)
    {
        string[] lines = File.ReadAllLines(filePath);
        string[] data = lines[1].Split(',');

        data[0] = day.ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(filePath, lines);
    }

    public void SetGold(int gold)
    {
        string[] lines = File.ReadAllLines(filePath);
        string[] data = lines[1].Split(',');

        data[1] = gold.ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(filePath, lines);
    }

    public void SetVisitors(int visitors)
    {
        string[] lines = File.ReadAllLines(filePath);
        string[] data = lines[1].Split(',');

        data[2] = visitors.ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(filePath, lines);
    }

    #endregion
}
