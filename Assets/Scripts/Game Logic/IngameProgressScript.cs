using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameProgressScript : MonoBehaviour {

    public struct PlayerData
    {
        public bool[] puzzleCompletions;
        public bool[] bossCompletions;
        public bool[] habilities;
        public bool[] achievements;
        public bool[] areas;
        public Vector3 saveSpawn;
        public Vector3 playerPosition;
        public int lastVisitedArea;
        public int day, month, year, hour, minute;
        public bool isEmpty;
    }

    public int puzzleCount = 14;
    public int bossCount = 2;
    public int habilityCount = 1;
    public int achievementCount = 1;
    public int areaCount = 4;

    public Transform BaseWorldSpawn;

    public PlayerData[] playerData = new PlayerData[4];
    private int currentPlayer = -1;

    [Header("Area names:")]
    public string[] areaNames;

    /*
    #region LocalPlayerData
    //ACHIEVEMENT TRACKING (DISPLAYED ON ACHIEVEMENTS MENU):
    // --> All indexes are set and read from getter and setter methods below with +or-1 degree of correction since arrays start from 0

    //Keeps track of what puzzles have been completed in current file.
    //Puzzles 1 to 11 are the mandatory puzzles, puzzles 12 to 14 are the optional ones
    private bool[] puzzleCompletions = new bool[14];

    //Keeps track of what bosses have been defeated in current file.
    private bool[] bossCompletions = new bool[2];

    //Keeps track of what habilities the player has obtained.
    //1 = LightRay 
    private bool[] habilities = new bool[1];

    //Keeps track of the player's achievements.
    private bool[] achievements = new bool[1];

    //Keeps track of the discovered areas.
    private bool[] areas = new bool[4];

    //OTHER NON DISPLAYED PROGRESSES:

    private GameObject saveSpawn;
    private int lastVisitedArea;
    private int day, month, year, hour, minute;
    #endregion
    */

    public static IngameProgressScript instance;

    void Awake()
    {
        if(instance == null) { instance = this; }
    }

    void Start()
    {
        //for(int i = 0; i < 4; i++)
        //{
        //    MakeEmptyGameState(i);
        //    playerData[i] = LoadGameState(i);
        //}
    }

    #region DylanFunctions XD

    //Last spawn save manipulators:
    public void setSaveSpawn(Vector3 spawn)
    {
        playerData[currentPlayer].saveSpawn = spawn;
    }
    public Vector3 getSaveSpawn()
    {
        return playerData[currentPlayer].saveSpawn;
    }

    //Puzzle completion manipulators:
    public void setPuzzleCompleted(bool to, int puzzleIndex)
    {
        playerData[currentPlayer].puzzleCompletions[puzzleIndex - 1] = to;
    }
    public bool getPuzzleCompleted(int puzzleIndex)
    {
        return playerData[currentPlayer].puzzleCompletions[puzzleIndex - 1];
    }

    //Obtained habilities manipulators:
    public void setHabilityObtained(bool to, int habilityIndex)
    {
        playerData[currentPlayer].habilities[habilityIndex - 1] = to;
    }
    public bool getHabilityObtained(int habilityIndex)
    {
        return playerData[currentPlayer].habilities[habilityIndex - 1];
    }

    //Visited areas manipulators:
    public void setAchievementObtained(bool to, int achievementIndex) //Puzzle indexes start from 1 (Auto substracted for array)
    {
        playerData[currentPlayer].puzzleCompletions[achievementIndex - 1] = to;
    }
    public bool getAchievementObtained(int achievementIndex)
    {
        return playerData[currentPlayer].puzzleCompletions[achievementIndex - 1];
    }

    //Obtained achievements manipulators:
    public void setAreaVisited(bool to, int areaIndex) //Puzzle indexes start from 1 (Auto substracted for array)
    {
        playerData[currentPlayer].puzzleCompletions[areaIndex - 1] = to;
        playerData[currentPlayer].lastVisitedArea = areaIndex-1;
    }
    public bool getAreaVisited(int puzzleIndex)
    {
        return playerData[currentPlayer].puzzleCompletions[puzzleIndex - 1];
    }

    #endregion

    //Game saver and loader:
    void SaveGameState(int playerIndex) //Saves current important game variables
    {
        PlayerData pd = playerData[playerIndex];

        SaveSystem.SetVector3("p" + playerIndex + "-saveSpawn",         pd.saveSpawn);
        SaveSystem.SetVector3("p" + playerIndex + "-playerPosition",    Player.instance.transform.position);
        SaveSystem.SetString("p" + playerIndex + "-puzzleCompletions",  BoolToString(pd.puzzleCompletions));
        SaveSystem.SetString("p" + playerIndex + "-bossCompletions",    BoolToString(pd.bossCompletions));
        SaveSystem.SetString("p" + playerIndex + "-habilities",         BoolToString(pd.habilities));
        SaveSystem.SetString("p" + playerIndex + "-achievements",       BoolToString(pd.achievements));
        SaveSystem.SetString("p" + playerIndex + "-areas",              BoolToString(pd.areas));
        SaveSystem.SetInt("p" + playerIndex + "-lastVisitedArea",       pd.lastVisitedArea);

        System.DateTime dt = System.DateTime.Now;
        pd.day = dt.Day;
        pd.month = dt.Month;
        pd.year = dt.Year;
        pd.hour = dt.Hour;
        pd.minute = dt.Minute;

        SaveSystem.SetInt("p" + playerIndex + "-save-day",      pd.day);
        SaveSystem.SetInt("p" + playerIndex + "-save-month",    pd.month);
        SaveSystem.SetInt("p" + playerIndex + "-save-year",     pd.year);
        SaveSystem.SetInt("p" + playerIndex + "-save-hour",     pd.hour);
        SaveSystem.SetInt("p" + playerIndex + "-save-minute",   pd.minute);

        SaveSystem.SetBool("p" + playerIndex + "-isEmpty", false);
    }

    public void SaveGame()
    {
        SaveGameState(currentPlayer);
    }

    PlayerData LoadGameState(int playerIndex) //Loads game state according to file
    {
        PlayerData pd = new PlayerData();

        pd.saveSpawn = SaveSystem.GetVector3("p" + playerIndex + "-saveSpawn");
        pd.playerPosition = SaveSystem.GetVector3("p" + playerIndex + "-playerPosition");
        pd.puzzleCompletions = StringToBool(SaveSystem.GetString("p" + playerIndex + "-puzzleCompletions"));
        pd.bossCompletions = StringToBool(SaveSystem.GetString("p" + playerIndex + "-bossCompletions"));
        pd.habilities = StringToBool(SaveSystem.GetString("p" + playerIndex + "-habilities"));
        pd.achievements = StringToBool(SaveSystem.GetString("p" + playerIndex + "-achievements"));
        pd.areas = StringToBool(SaveSystem.GetString("p" + playerIndex + "-areas"));
        pd.lastVisitedArea = SaveSystem.GetInt("p" + playerIndex + "-lastVisitedArea");

        pd.day = SaveSystem.GetInt("p" + playerIndex + "-save-day");
        pd.month = SaveSystem.GetInt("p" + playerIndex + "-save-month");
        pd.year = SaveSystem.GetInt("p" + playerIndex + "-save-year");
        pd.hour = SaveSystem.GetInt("p" + playerIndex + "-save-hour");
        pd.minute = SaveSystem.GetInt("p" + playerIndex + "-save-minute");

        pd.isEmpty = SaveSystem.GetBool("p" + playerIndex + "-isEmpty");

        return pd;
    }

    void MakeEmptyGameState(int playerIndex) //Saves current important game variables
    {
        PlayerData pd = playerData[playerIndex];

        pd.habilities = new bool[habilityCount];
        pd.achievements = new bool[achievementCount];
        pd.areas = new bool[areaCount];
        pd.bossCompletions = new bool[bossCount];
        pd.day = 0;
        pd.hour = 0;
        pd.isEmpty = true;
        pd.lastVisitedArea = 0;
        pd.minute = 0;
        pd.month = 0;
        pd.playerPosition = BaseWorldSpawn.position;
        pd.puzzleCompletions = new bool[puzzleCount];
        pd.saveSpawn = BaseWorldSpawn.position;
        pd.year = 0;


        playerData[playerIndex] = pd;
        SaveGameState(playerIndex);
    }

    public void DeletePlayer(int playerIndex) { MakeEmptyGameState(playerIndex); }

    public void SetPlayer(int playerIndex)
    {
        currentPlayer = playerIndex;
        Player.instance.transform.position = playerData[playerIndex].playerPosition;
        if(playerData[currentPlayer].isEmpty) { TutorialController.instance.StartTutorial(); }
    }

    public void PlayerNotEmptyAnymore() { playerData[currentPlayer].isEmpty = false; }

    public void LoadAllData()
    {
        for(int i = 0; i < 4; i++)
        {
            playerData[i] = LoadGameState(i);
        }
    }

    public BoatController.BoatData GetBoatData(int playerIndex)
    {
        BoatController.BoatData bd;
        bd.areaName = areaNames[playerData[playerIndex].lastVisitedArea];
        bd.completedPuzzles = CountTrueBools(playerData[playerIndex].puzzleCompletions);
        bd.day = playerData[playerIndex].day;
        bd.month = playerData[playerIndex].month;
        bd.year = playerData[playerIndex].year;
        bd.hour = playerData[playerIndex].hour;
        bd.minutes = playerData[playerIndex].minute;
        bd.isEmpty = playerData[playerIndex].isEmpty;

        return bd;
    }

    int CountTrueBools(bool[] b)
    {
        if (b == null) return 0;
        int count = 0;
        for(int i = 0; i < b.Length; i++)
        {
            if (b[i]) count++;
        }
        return count;
    }

    string BoolToString(bool[] puz)
    {
        string st = "";
        for(int i = 0; i < puz.Length; i++)
        {
            if(puz[i]) { st += "1"; }
            else { st += "0"; }
        }
        return st;
    }

    bool[] StringToBool(string st)
    {
        bool[] b = new bool[st.Length];
        char[] c = st.ToCharArray();
        for(int i = 0; i < c.Length; i++)
        {
            if(c[i] == '1') { b[i] = true; }
            else if (c[i] == '0') { b[i] = false; }
        }
        return b;
    }
}
