using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    #region 公開定義
    public Button GameSceneButton;
    public Button GameStartSceneButton;
    #endregion

    #region 私有定義
    [SerializeField] private string gameStartScene = "GameStart";
    [SerializeField] private string gameScene = "Game";
    #endregion

    #region 公開方法 LoadSceneGameStart()
    public void LoadSceneGameStart() {
        StartCoroutine(LoadScene(gameStartScene));
    }
    #endregion

    #region 公開方法 LoadSceneGame()
    public void LoadSceneGame() {
        StartCoroutine(LoadScene(gameScene));
    }
    #endregion

    #region 私有協程：順序加載場景
    private IEnumerator LoadScene(string sceneName) {
        if (GameSceneButton != null)
        {
            GameSceneButton.interactable = false;
        }
        else
        {
            Debug.LogWarning("⚠️ GameSceneButton 為 null，按鈕無法禁用");
        }

        #region Data完成確認
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager.Instance 為 null！");
            yield break;
        }

        if (!GameManager.Instance.IsAllDataLoaded)
        {
            Debug.LogError("❌ 資料未完成加載，等待中...");
            yield return new WaitUntil(() => GameManager.Instance.IsAllDataLoaded);
        }
        #endregion

        #region 單例存在確認
        if (GamePauseManager.Instance == null)
        {
            Debug.LogError("❌ GamePauseManager.Instance 為 null");
            yield break;
        }
        if (FadeManager.Instance == null)
        {
            Debug.LogError("❌ FadeManager.Instance 為 null");
            yield break;
        }
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ GameManager.Instance 為 null");
            yield break;
        }
        #endregion

        // 暫停遊戲
        GamePauseManager.Instance.PauseGame();

        // 執行淡出效果
        yield return FadeManager.Instance.FadeOut();

        // 等待黑屏穩定
        yield return new WaitForSecondsRealtime(FadeManager.Instance.fadeDuration);

        // 加載場景
        AsyncOperation newScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return new WaitUntil(() => newScene.isDone);

        // 停用按鈕
        if (GameSceneButton != null)
        {
            GameSceneButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ GameSceneButton 為 null，無法隱藏");
        }

        // 執行淡入效果
        yield return FadeManager.Instance.FadeIn();

        // 恢復遊戲
        GamePauseManager.Instance.ResumeGame();
    }
    #endregion
}
