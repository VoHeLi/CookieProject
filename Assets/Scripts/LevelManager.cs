using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private bool[] isLevelCompleted = new bool[6];

    public bool getIsLevelFinished(int i)
    {
        if (isLevelCompleted[i])
        {
            return true;
        }
        return false;
    }

    public void setLevelFinished(int i)
    {
        isLevelCompleted[i] = true;
    }
}
