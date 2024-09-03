
namespace Watermelon.BubbleMerge
{
    public class InGameSettingsExitButton : SettingsButtonBase
    {
        public override bool IsActive()
        {
            return true;
        }

        public override void OnClick()
        {
            UIController.GamePage.ShowExitPopUp();

            // Play button sound
            AudioController.PlaySound(AudioController.Sounds.buttonSound);
        }

    }
}