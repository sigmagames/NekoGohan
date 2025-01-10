using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class OtherButton : SettingsButtonBase
    {
        [SerializeField] Image imageRef;
        [SerializeField] GameObject otherPanel;  // クレジットパネルの参照を追加

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

            otherPanel.SetActive(false);  // パネルを非表示にしておく
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            imageRef.sprite = activeSprite;
            otherPanel.SetActive(true);  // クレジットパネルを表示
        }

        // プライバシーポリシーURLをブラウザで開くメソッド
        public void OpenPrivacyPolicy()
        {
            string url = "https://sigmagames.hatenablog.com/entry/2024/10/01/180421?_gl=1*eg1fbw*_gcl_au*ODU5NjAxOTY5LjE3Mjc3NzI5NjY.";
            Application.OpenURL(url);
        }
    }
}
