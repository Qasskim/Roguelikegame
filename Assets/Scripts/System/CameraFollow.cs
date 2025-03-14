using UnityEngine;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (플레이어 또는 적)
    public float smoothSpeed = 5f; // 부드러운 이동 속도
    private List<Transform> targets = new List<Transform>(); // 플레이어 + 적들 목록
    private int currentTargetIndex = 0; // 현재 보고 있는 대상 인덱스

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            targetPosition.z = transform.position.z; // 카메라는 Z축 고정
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.C)) // 🔄 키 입력으로 전환
        {
            SwitchTarget();
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    public void InitializeTargets(Transform player, List<Transform> enemies)
    {
        targets.Clear();
        targets.Add(player); // 플레이어 추가
        targets.AddRange(enemies); // 적들 추가
        currentTargetIndex = 0;
        SetTarget(targets[currentTargetIndex]); // 초기 타겟 설정
    }

    private void SwitchTarget()
    {
        if (targets.Count == 0) return;

        currentTargetIndex = (currentTargetIndex + 1) % targets.Count; // 순환 구조
        SetTarget(targets[currentTargetIndex]);
        Debug.Log($"📷 카메라 전환: {targets[currentTargetIndex].name}");
    }
}
