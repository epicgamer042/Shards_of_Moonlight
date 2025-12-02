using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerScoreData", menuName = "Scriptable Objects/PlayerScoreData")]

public class PlayerScoreData : ScriptableObject
{
    public List<float> levelTimes = new List<float>();

    private void OnEnable()
    {
        // Ensure the list always has 6 slots
        if (levelTimes.Count < 6)
        {
            levelTimes = new List<float>(new float[6]);
        }
    }
}
