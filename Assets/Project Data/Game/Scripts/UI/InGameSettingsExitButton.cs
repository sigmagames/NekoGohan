using UnityEngine;

namespace Watermelon.BubbleMerge
{
    public class InGameSettingsExitButton : SettingsButtonBase
    {
        [SerializeField] GameObject nekoRetryImage;

        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            UIController.GamePage.ShowExitPopUp();
            nekoRetryImage.SetActive(true); // インスタンスが不要な場合
            // ボタンサウンドを再生
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }
    }
}
