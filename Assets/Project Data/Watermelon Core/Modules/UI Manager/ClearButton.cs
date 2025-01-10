using UnityEngine;
using UnityEngine.UI;
using Watermelon.BubbleMerge;

namespace Watermelon
{
    public class ClearButton : MonoBehaviour
    {
        [SerializeField] GameObject clearPanel; // Clearパネルの参照
        [SerializeField] Button clearDataButton; // データクリアボタンの参照
        [SerializeField] Text pathText; // 保存先パスを表示するTextオブジェクト

        private void Start()
        {
            // Clearパネルを非表示にしておく
            if (clearPanel != null)
            {
                clearPanel.SetActive(false);
            }

            // データクリアボタンにリスナーを追加
            if (clearDataButton != null)
            {
                clearDataButton.onClick.AddListener(OnClearDataButtonPressed);
            }

            // 保存先パスを取得してpathTextに表示
            if (pathText != null)
            {
                string savePath = Application.persistentDataPath;
                pathText.text = $"Save Path:\n{savePath}";
                Debug.Log($"[ClearButton]: Save path is {savePath}");
            }
        }

        /// <summary>
        /// Clearボタンが押されたときの動作
        /// </summary>
        public void OnClick()
        {
            if (clearPanel != null)
            {
                clearPanel.SetActive(true); // Clearパネルを表示
            }
        }

        private void OnClearDataButtonPressed()
        {
            // ゲーム進行をリセット
            SaveController.ResetGameProgress();

            // 最大到達レベルをリセット
            LevelController.MaxLevelReached = 0;

            // すべてのコインをリセット
            CurrenciesController.ResetAllCurrencies();

            // アプリを終了
            Debug.Log("[ClearButton]: ゲームデータをリセットしてアプリを終了します。");
            Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        }
    }
}
