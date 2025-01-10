#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class CreditPanel : MonoBehaviour
    {
        [SerializeField] GameObject creditPanel1;  // 1オブジェクト
        [SerializeField] GameObject creditPanel2;  // 2オブジェクト
        [SerializeField] GameObject creditPanel3;  // 3オブジェクト
        [SerializeField] Button rightButton;       // 右ボタンの参照

        private int currentPanelIndex = 0;  // 現在表示しているパネルのインデックス

        private void Start()
        {
            currentPanelIndex = 0;  // 初期状態では1オブジェクトを表示
            ShowCurrentPanel();

            // 右ボタンの初期状態を0度に設定
            rightButton.transform.rotation = Quaternion.Euler(0, 0, 0);

            // 右ボタンにクリックイベントを追加
            rightButton.onClick.AddListener(OnRightButtonClick);
        }

        // 右ボタンがクリックされた時の処理
        private void OnRightButtonClick()
        {
            TogglePanels();  // オブジェクトの表示を切り替える
            RotateButton();  // 右ボタンの回転を切り替える
        }

        // 現在のパネルのみを表示し、他のパネルを非表示にする
        private void ShowCurrentPanel()
        {
            creditPanel1.SetActive(currentPanelIndex == 0);
            creditPanel2.SetActive(currentPanelIndex == 1);
            creditPanel3.SetActive(currentPanelIndex == 2);
        }

        // オブジェクトの表示/非表示を切り替える
        private void TogglePanels()
        {
            currentPanelIndex = (currentPanelIndex + 1) % 3;  // 次のパネルに移動
            ShowCurrentPanel();  // 現在のパネルを表示
        }

        // 右ボタンの回転を切り替える
        private void RotateButton()
        {
            // 3番目のパネルを表示している場合のみ180度回転、それ以外は0度
            float rotationAngle = currentPanelIndex == 2 ? -180 : 0;
            rightButton.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }
    }
}
