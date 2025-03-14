using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Vector2Int gridPosition;
    private Transform player; // 🎯 플레이어를 추적하기 위한 변수

    void Start()
    {
        gridPosition = Vector2Int.RoundToInt(transform.position);

        // ✅ 적을 `TurnManager`에 등록 (적 턴 실행을 위해 필요)
        TurnManager.Instance.RegisterEnemy(this);
    }

    void Update()
    {
        // ✅ 플레이어가 존재하지 않으면 지속적으로 찾기
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Debug.Log("✅ 플레이어를 감지함: " + player.position);
            }
        }
    }

    public void PerformMove()
    {
        if (player == null) return; // 플레이어가 없으면 이동 안 함

        Vector2Int playerPosition = Vector2Int.RoundToInt(player.position);
        Vector2Int direction = GetBestMoveDirection(playerPosition);

        Debug.Log($"👹 [EnemyAI] {gameObject.name} 이동 방향: {direction}");

        if (direction == Vector2Int.zero)
        {
            Debug.LogWarning($"⚠️ [EnemyAI] {gameObject.name} 이동 방향 없음!");
            return;
        }

        Vector2Int newPosition = gridPosition + direction;

        if (GridManager.Instance.IsWalkable(newPosition))
        {
            gridPosition = newPosition;
            transform.position = (Vector2)gridPosition;
            Debug.Log($"👹 [EnemyAI] {gameObject.name} 이동 완료: {gridPosition}");
        }
        else
        {
            Debug.LogWarning($"⚠️ [EnemyAI] {gameObject.name} 이동 불가능 위치: {newPosition}");
        }
    }

    private Vector2Int GetBestMoveDirection(Vector2Int targetPosition)
    {
        int dx = targetPosition.x - gridPosition.x;
        int dy = targetPosition.y - gridPosition.y;

        return new Vector2Int(Mathf.Clamp(dx, -1, 1), Mathf.Clamp(dy, -1, 1));
    }
}
