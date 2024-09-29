#pragma warning disable 649

using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class CreditButton : SettingsButtonBase
    {
        [SerializeField] Image imageRef;
        [SerializeField] GameObject creditPanel;  // クレジットパネルの参照を追加

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

            creditPanel.SetActive(false);  // パネルを非表示にしておく
        }

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {

                imageRef.sprite = activeSprite;
                creditPanel.SetActive(true);  // クレジットパネルを表示

        }
    }
}
