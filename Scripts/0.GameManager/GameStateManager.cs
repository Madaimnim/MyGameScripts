using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    #region Enum �w�q
    public enum GameState
    {
        GameStart,     // �T�ΩҦ�����]�C���}�l�ʵe�ιL���^
        InGame,        // �C�������`�ާ@
        Pause          // �Ȱ����q
    }
    #endregion

    public GameState currentState = GameState.GameStart;

    #region ��ҼҦ�
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �T�O������s��
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region ���}��k SetState(GameState newState)
    public void SetState(GameState newState) {
        currentState = newState;
        Debug.Log("��e�C�����A: " + newState);
    }
    #endregion
}
