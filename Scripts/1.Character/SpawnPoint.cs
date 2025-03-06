using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpawnPoint : MonoBehaviour
{
    public List<Transform> spawnPositionsList = new List<Transform>(); // 直接存儲座標，而不是 Transform

    private void Start() {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.spawnPositionsList.Clear(); // ✅ 清空舊的座標
            PlayerStateManager.Instance.spawnPositionsList.AddRange(
                spawnPositionsList.Select(t => (Vector2)t.position) // ✅ 轉換 `Transform` 到 `Vector2`
            );
        }
        else
            Debug.LogError("未找到PlayerStateManager");
    }
}
