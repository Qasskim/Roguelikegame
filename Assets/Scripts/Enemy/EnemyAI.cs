using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Transform player;
    private Vector2Int gridPosition;

    void Start()
    {
        Invoke("FindPlayer", 1f);
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogError("EnemyAI: Player를 찾을 수 없습니다!");
            return;
        }

        gridPosition = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPosition;
        TurnManager.Instance.RegisterEnemy(this);
    }

    public void PerformMove()
    {
        if (player != null)
        {
            Vector2Int playerGridPos = Vector2Int.RoundToInt(player.position);
            Vector2Int direction = playerGridPos - gridPosition;

            direction = new Vector2Int(Mathf.Clamp(direction.x, -1, 1), Mathf.Clamp(direction.y, -1, 1));

            Vector2Int targetPosition = gridPosition + direction;

            if (GridManager.Instance.IsWalkable(targetPosition))
            {
                gridPosition = targetPosition;
                transform.position = (Vector2)gridPosition;
            }
        }
    }
}
