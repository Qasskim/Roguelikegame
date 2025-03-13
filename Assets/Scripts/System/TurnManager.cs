using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;
    public float baseTurnCost = 1.0f;
    private float currentTurnProgress = 0f;
    private List<EnemyAI> enemies = new List<EnemyAI>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RegisterEnemy(EnemyAI enemy)
    {
        if (!enemies.Contains(enemy)) enemies.Add(enemy);
    }

    public void ProcessTurn(float playerTurnCost)
    {
        currentTurnProgress += playerTurnCost;
        if (currentTurnProgress >= baseTurnCost)
        {
            currentTurnProgress = 0f;
            ExecuteEnemyTurn();
        }
    }

    private void ExecuteEnemyTurn()
    {
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null) enemy.PerformMove();
        }
    }
}
