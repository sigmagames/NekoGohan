#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class ExitButton : SettingsButtonBase
    {
        [SerializeField] Image imageRef;
        [SerializeField] GameObject ExitPanel;  // クレジットパネルの参照を追加

        [Space]
        [SerializeField] Sprite activeSprite;
        [SerializeField] Sprite disableSprite;

        private bool isActive = true;

        private void Start()
        {
            isActive = AudioController.GetVolume() != 0;

            if (isActive)
                imageRef.sprite = activeSprite;
            else
                imageRef.sprite = disableSprite;

            ExitPanel.SetActive(false);  // パネルを非表示にしておく
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            imageRef.sprite = activeSprite;
            ExitPanel.SetActive(true);  // クレジットパネルを表示
        }

        // ゲームを終了するメソッド
        public void ExitGame()
        {
            // エディタ内では終了しないようにする
#if UNITY_EDITOR
            Debug.Log("ゲーム終了");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
