using UnityEngine;
using System;

public class EndGameZone : MonoBehaviour
{

    public static event Action OnLevelCompleted;

    private bool allShroudsFound = false;

    private void OnEnable()
    {
        InGame_UI.AllShardsCollected += HandleShardCompletion;
    }

    private void OnDisable()
    {
        InGame_UI.AllShardsCollected += HandleShardCompletion;
    }

    private void HandleShardCompletion()
    {
        allShroudsFound = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && allShroudsFound)
        {
            OnLevelCompleted?.Invoke(); // this fires the end game event
        }
    }

}
