using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void OnEnable() {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.stageSpawnPosition = gameObject.transform.position;
            Debug.Log($"生成點座標為{ gameObject.transform.position}");
        }
        PlayerStateManager.Instance.ActivateAllPlayer();
    }
}
