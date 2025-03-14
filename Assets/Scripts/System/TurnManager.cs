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
        Debug.Log($"🔄 [TurnManager] 플레이어 턴 진행: {playerTurnCost}");

        currentTurnProgress += playerTurnCost;
        if (currentTurnProgress >= baseTurnCost)
        {
            Debug.Log("🔄 [TurnManager] 적 턴 실행");
            currentTurnProgress = 0f;
            ExecuteEnemyTurn();
        }
    }

    private void ExecuteEnemyTurn()
    {
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy != null)
            {
                Debug.Log($"👹 [TurnManager] 적 이동 실행: {enemy.gameObject.name}");
                enemy.PerformMove();
            }
            else
            {
                Debug.LogWarning("⚠️ [TurnManager] Null 적 객체가 감지됨");
            }
        }
    }
}
