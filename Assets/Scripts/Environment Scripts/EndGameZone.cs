using UnityEngine;
using System;

public class EndGameZone : MonoBehaviour
{

    public static event Action OnLevelCompleted;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnLevelCompleted?.Invoke(); // this fires the event
        }
    }
}
