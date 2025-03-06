using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    #region �ܼƩw�q
    public GameObject inventoryPanel; // �ݭn������ Panel
    #endregion

    #region ��s�޿�
    private void Update() {
        // �u���b InGame �� Pause ���A�ɡA�~��ϥ� I ��P ESC ��
        if (GameStateManager.Instance.currentState == GameStateManager.GameState.InGame ||
            GameStateManager.Instance.currentState == GameStateManager.GameState.Pause)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ToggleInventory();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleInventory();
            }
        }
    }
    #endregion

    #region UI ������k
    private void ToggleInventory() {
        bool isActive = inventoryPanel.activeSelf;
        inventoryPanel.SetActive(!isActive);

        // �����C�����A
        if (isActive)
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.InGame);
        }
        else
        {
            GameStateManager.Instance.SetState(GameStateManager.GameState.Pause);
        }
    }
    #endregion
}