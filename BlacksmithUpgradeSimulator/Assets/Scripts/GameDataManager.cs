using System.IO;
using UnityEngine;

public enum GameStatus
{
    NoEnhance, // 강화 전
    Enhancing, // 강화 진행중
    Enhanced // 강화 완료
}

public class GameDataManager : MonoBehaviour
{
    private string gameDataFilePath; // 게임 데이터 파일 경로
    private string statisticsDataPath; // 통계 데이터 파일 경로
    private string settlementDataPath; // 정산 데이터 파일 경로
    private string backUpGameDataPath; // 롤백 게임 데이터 파일 경로

    private const string DEFAULT_GAME_DATA_CSV =
    "Day,Gold,Visitors\n1,0,0";

    private const string DEFAULT_RESULT_DATA_CSV =
        "SuccessCnt,GreatSuccessCnt,FailCnt\n0,0,0";

    private const string DEFAULT_SETTLEMENT_DATA_CSV =
        "CurrentGold,CurrentSuccessCnt,CurrentGreatSuccessCnt,CurrentFailCnt,GameStauts\n0,0,0,0,0";

    private const string DEFAULT_BACKUP_GAMEDATA_CSV =
        "Day,Gold,Visitors,SuccessCnt,GreatSuccessCnt,FailCnt,CurrentGold,CurrentSuccessCnt,CurrentGreatSuccessCnt,CurrentFailCnt\n1,0,0,0,0,0,0,0,0,0";

    private void Awake()
    {
        CreateFile("GameData.csv", ref gameDataFilePath, DEFAULT_GAME_DATA_CSV); // 게임 데이터 파일 생성
        CreateFile("StatisticsData.csv", ref statisticsDataPath, DEFAULT_RESULT_DATA_CSV); // 통계 데이터 파일 생성
        CreateFile("SettlementData.csv", ref settlementDataPath, DEFAULT_SETTLEMENT_DATA_CSV); // 정산 데이터 파일 생성
        CreateFile("BackUpData.csv", ref backUpGameDataPath, DEFAULT_BACKUP_GAMEDATA_CSV); // 롤백 게임 데이터 파일 생성
    }

    private void CreateFile(string fileName, ref string filePath, string defaultCSV)
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log($"filePath : {filePath}");
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, defaultCSV);
        }
    }

    #region Get

    public int GetDay()
    {
        string[] lines = File.ReadAllLines(gameDataFilePath);
        string[] data = lines[1].Split(',');

        return int.Parse(data[0]);
    }

    public int GetGold()
    {
        string[] lines = File.ReadAllLines(gameDataFilePath);
        string[] data = lines[1].Split(',');

        return int.Parse(data[1]);
    }

    public int GetVisitors()
    {
        string[] lines = File.ReadAllLines(gameDataFilePath);
        string[] data = lines[1].Split(',');

        return int.Parse(data[2]);
    }

    public int GetSuccessCount()
    {
        string[] lines = File.ReadAllLines(statisticsDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[0]);
    }

    public int GetGreatSuccessCount()
    {
        string[] lines = File.ReadAllLines(statisticsDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[1]);
    }

    public int GetFailCount()
    {
        string[] lines = File.ReadAllLines(statisticsDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[2]);
    }

    public int GetCurrentGold()
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[0]);
    }

    public int GetCurrentSuccessCount()
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[1]);
    }

    public int GetCurrentGreatSuccessCount()
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[2]);
    }

    public int GetCurrentFailCount()
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        return int.Parse(data[3]);
    }

    public GameStatus GetGameStatus()
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        return (GameStatus)int.Parse(data[4]);
    }

    #endregion

    #region Set

    public void SetDay(int day)
    {
        string[] lines = File.ReadAllLines(gameDataFilePath);
        string[] data = lines[1].Split(',');

        data[0] = day.ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(gameDataFilePath, lines);
    }

    public void SetGold(int gold)
    {
        string[] lines = File.ReadAllLines(gameDataFilePath);
        string[] data = lines[1].Split(',');

        data[1] = gold.ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(gameDataFilePath, lines);
    }

    public void SetVisitors(int visitors)
    {
        string[] lines = File.ReadAllLines(gameDataFilePath);
        string[] data = lines[1].Split(',');

        data[2] = visitors.ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(gameDataFilePath, lines);
    }

    public void SetSuccessCount(int successCount)
    {
        string[] lines = File.ReadAllLines(statisticsDataPath);
        string[] data = lines[1].Split(',');
        data[0] = successCount.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(statisticsDataPath, lines);
    }

    public void SetGreatSuccessCount(int greatSuccessCount)
    {
        string[] lines = File.ReadAllLines(statisticsDataPath);
        string[] data = lines[1].Split(',');
        data[1] = greatSuccessCount.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(statisticsDataPath, lines);
    }

    public void SetFailCount(int failCount)
    {
        string[] lines = File.ReadAllLines(statisticsDataPath);
        string[] data = lines[1].Split(',');
        data[2] = failCount.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(statisticsDataPath, lines);
    }

    public void SetCurrentGold(int currentGold)
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        data[0] = currentGold.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(settlementDataPath, lines);
    }

    public void SetCurrentSuccessCount(int currentSuccessCount)
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        data[1] = currentSuccessCount.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(settlementDataPath, lines);
    }

    public void SetCurrentGreatSuccessCount(int currentGreatSuccessCount)
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        data[2] = currentGreatSuccessCount.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(settlementDataPath, lines);
    }

    public void SetCurrentFailCount(int currentFailCount)
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');
        data[3] = currentFailCount.ToString();
        lines[1] = string.Join(",", data);
        File.WriteAllLines(settlementDataPath, lines);
    }

    public void SetGameStatus(GameStatus gameStatus)
    {
        string[] lines = File.ReadAllLines(settlementDataPath);
        string[] data = lines[1].Split(',');

        data[4] = ((int)gameStatus).ToString();

        lines[1] = string.Join(",", data);
        File.WriteAllLines(settlementDataPath, lines);
    }

    public void ResetSettlementData()
    {
        SetCurrentGold(0);
        SetCurrentSuccessCount(0);
        SetCurrentGreatSuccessCount(0);
        SetCurrentFailCount(0);
        SetGameStatus(GameStatus.NoEnhance);
    }

    #endregion

    #region Reset

    // 게임 데이터 백업 기능: 현재 게임 데이터를 백업 파일에 저장
    public void BackupGameData()
    {
        string[] lines =
        {
            "Day,Gold,Visitors,SuccessCnt,GreatSuccessCnt,FailCnt,CurrentGold,CurrentSuccessCnt,CurrentGreatSuccessCnt,CurrentFailCnt",
            $"{GetDay()},{GetGold()},{GetVisitors()},{GetSuccessCount()},{GetGreatSuccessCount()},{GetFailCount()},{GetCurrentGold()},{GetCurrentSuccessCount()},{GetCurrentGreatSuccessCount()},{GetCurrentFailCount()}"
        };

        File.WriteAllLines(backUpGameDataPath, lines);
    }

    // 롤백 기능: 백업된 데이터를 불러와서 게임 데이터를 복원
    public void RollbackGameData()
    {
        if (File.Exists(backUpGameDataPath))
        {
            string[] lines = File.ReadAllLines(backUpGameDataPath);
            if (lines.Length > 1)
            {
                string[] data = lines[1].Split(',');
                SetDay(int.Parse(data[0]));
                SetGold(int.Parse(data[1]));
                SetVisitors(int.Parse(data[2]));
                SetSuccessCount(int.Parse(data[3]));
                SetGreatSuccessCount(int.Parse(data[4]));
                SetFailCount(int.Parse(data[5]));
                SetCurrentGold(int.Parse(data[6]));
                SetCurrentSuccessCount(int.Parse(data[7]));
                SetCurrentGreatSuccessCount(int.Parse(data[8]));
                SetCurrentFailCount(int.Parse(data[9]));
            }
        }
    }
    #endregion
}
