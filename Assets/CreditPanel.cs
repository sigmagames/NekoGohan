#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class CreditPanel : MonoBehaviour
    {
        [SerializeField] GameObject creditPanel1;  // 1オブジェクト
        [SerializeField] GameObject creditPanel2;  // 2オブジェクト
        [SerializeField] Button rightButton;       // 右ボタンの参照

        private bool isPanel1Active = true;  // 1オブジェクトが表示中かを管理するフラグ

        private void Start()
        {
            isPanel1Active = true;  // 初期状態では1オブジェクトを表示
            creditPanel1.SetActive(true);
            creditPanel2.SetActive(false);

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

        // オブジェクトの表示/非表示を切り替える
        private void TogglePanels()
        {
            if (isPanel1Active)
            {
                creditPanel1.SetActive(false);  // 1オブジェクトを非表示
                creditPanel2.SetActive(true);   // 2オブジェクトを表示
            }
            else
            {
                creditPanel1.SetActive(true);   // 1オブジェクトを表示
                creditPanel2.SetActive(false);  // 2オブジェクトを非表示
            }

            isPanel1Active = !isPanel1Active;  // 表示フラグを反転
        }

        // 右ボタンの回転を切り替える
        private void RotateButton()
        {
            if (isPanel1Active)
            {
                // 0度にリセット
                rightButton.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                // -180度に回転
                rightButton.transform.rotation = Quaternion.Euler(0, 0, -180);
            }
        }
    }
}
