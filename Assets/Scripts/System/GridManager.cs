using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public int width = 10;
    public int height = 10;

    private Dictionary<Vector2Int, bool> grid = new Dictionary<Vector2Int, bool>();

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 3;

    private GameObject playerInstance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("❌ GridManager 인스턴스가 중복 생성됨!");
            Destroy(gameObject);
            return;
        }

        GenerateGrid();
        SpawnPlayer();
        Invoke("SpawnEnemies", 0.2f);
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[new Vector2Int(x, y)] = true;
            }
        }

        Debug.Log("✅ Grid 생성 완료");
    }

    void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("❌ Player Prefab이 할당되지 않았음!");
            return;
        }

        Vector2Int spawnPosition = GetRandomEmptyPosition();
        Debug.Log($"🎮 플레이어 배치 위치: {spawnPosition}");

        playerInstance = Instantiate(playerPrefab, (Vector2)spawnPosition, Quaternion.identity);
        playerInstance.name = "Player";

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("❌ Camera.main을 찾을 수 없음! 씬에 Main Camera가 있는지 확인하세요.");
            return;
        }

        CameraFollow cameraFollow = cam.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.SetTarget(playerInstance.transform);
            Debug.Log("✅ 카메라가 플레이어를 따라가도록 설정됨!");
        }
        else
        {
            Debug.LogError("❌ CameraFollow 스크립트가 Main Camera에 추가되지 않음!");
        }

        Debug.Log($"✅ 플레이어 배치 완료 - 위치: {playerInstance.transform.position}");
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("❌ Enemy Prefab이 할당되지 않았음!");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            Vector2Int spawnPosition = GetRandomEmptyPosition();
            Debug.Log($"👹 적 {i + 1} 배치 위치: {spawnPosition}");

            GameObject enemyInstance = Instantiate(enemyPrefab, (Vector2)spawnPosition, Quaternion.identity);
            enemyInstance.name = "Enemy_" + i;

            // ✅ 적을 `TurnManager`에 등록
            TurnManager.Instance.RegisterEnemy(enemyInstance.GetComponent<EnemyAI>());
        }

        Debug.Log("✅ 적 배치 완료");
    }

    Vector2Int GetRandomEmptyPosition()
    {
        List<Vector2Int> emptyPositions = new List<Vector2Int>();

        foreach (var cell in grid)
        {
            if (cell.Value)
                emptyPositions.Add(cell.Key);
        }

        if (emptyPositions.Count == 0)
        {
            Debug.LogError("❌ 빈 공간을 찾을 수 없음!");
            return Vector2Int.zero;
        }

        return emptyPositions[Random.Range(0, emptyPositions.Count)];
    }

    public bool IsWalkable(Vector2Int position)
    {
        return grid.ContainsKey(position) && grid[position];
    }
}
