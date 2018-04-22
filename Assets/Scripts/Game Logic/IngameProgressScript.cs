using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameProgressScript : MonoBehaviour {

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

    //Last spawn save manipulators:
    public void setSaveSpawn(GameObject spawn)
    {
        saveSpawn = spawn;
    }
    public GameObject getSaveSpawn()
    {
        return saveSpawn;
    }

    //Puzzle completion manipulators:
    public void setPuzzleCompleted(bool to, int puzzleIndex)
    {
        puzzleCompletions[puzzleIndex - 1] = to;
    }
    public bool getPuzzleCompleted(int puzzleIndex)
    {
        return puzzleCompletions[puzzleIndex - 1];
    }

    //Obtained habilities manipulators:
    public void setHabilityObtained(bool to, int habilityIndex)
    {
        habilities[habilityIndex - 1] = to;
    }
    public bool getHabilityObtained(int habilityIndex)
    {
        return habilities[habilityIndex - 1];
    }

    //Visited areas manipulators:
    public void setAchievementObtained(bool to, int achievementIndex) //Puzzle indexes start from 1 (Auto substracted for array)
    {
        puzzleCompletions[achievementIndex - 1] = to;
    }
    public bool getAchievementObtained(int achievementIndex)
    {
        return puzzleCompletions[achievementIndex - 1];
    }

    //Obtained achievements manipulators:
    public void setAreaVisited(bool to, int areaIndex) //Puzzle indexes start from 1 (Auto substracted for array)
    {
        puzzleCompletions[areaIndex - 1] = to;
    }
    public bool getAreaVisited(int puzzleIndex)
    {
        return puzzleCompletions[puzzleIndex - 1];
    }

    //Game saver and loader:
    public void saveGameState() //Saves current important game variables
    {

    }

    public void loadGameState(int file) //Loads game state according to file
    {

    }
}
