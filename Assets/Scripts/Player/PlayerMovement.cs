using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnCost = 1.0f;
    private Vector2Int gridPosition;

    void Start()
    {
        gridPosition = Vector2Int.RoundToInt(transform.position);
        transform.position = (Vector2)gridPosition;
    }

    void Update()
    {
        Vector2Int moveDirection = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) moveDirection = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) moveDirection = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) moveDirection = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) moveDirection = Vector2Int.right;

        if (Input.GetKeyDown(KeyCode.Q)) moveDirection = new Vector2Int(-1, 1);
        if (Input.GetKeyDown(KeyCode.E)) moveDirection = new Vector2Int(1, 1);
        if (Input.GetKeyDown(KeyCode.Z)) moveDirection = new Vector2Int(-1, -1);
        if (Input.GetKeyDown(KeyCode.C)) moveDirection = new Vector2Int(1, -1);

        if (moveDirection != Vector2Int.zero)
        {
            TryMove(gridPosition + moveDirection);
        }
    }

    void TryMove(Vector2Int targetPosition)
    {
        if (GridManager.Instance.IsWalkable(targetPosition))
        {
            gridPosition = targetPosition;
            transform.position = (Vector2)gridPosition;
            TurnManager.Instance.ProcessTurn(turnCost);
        }
    }
}
