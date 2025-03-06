using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    #region Enum 定義
    public enum GameState
    {
        GameStart,     // 禁用所有控制（遊戲開始動畫或過場）
        InGame,        // 遊戲內正常操作
        Pause          // 暫停階段
    }
    #endregion

    public GameState currentState = GameState.GameStart;

    #region 單例模式
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 確保跨場景存活
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region 公開方法 SetState(GameState newState)
    public void SetState(GameState newState) {
        currentState = newState;
        Debug.Log("當前遊戲狀態: " + newState);
    }
    #endregion
}
