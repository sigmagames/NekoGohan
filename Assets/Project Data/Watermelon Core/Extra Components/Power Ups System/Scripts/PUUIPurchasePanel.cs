using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Watermelon
{
    public class PUUIPurchasePanel : MonoBehaviour, IPopupWindow
    {
        [SerializeField] GameObject powerUpPurchasePanel;
        [SerializeField] RectTransform safeAreaTransform;

        [Space(5)]
        [SerializeField] Image powerUpPurchasePreview;
        [SerializeField] TMP_Text powerUpPurchaseAmountText;
        [SerializeField] Text powerUpPurchaseDescriptionText;
        [SerializeField] TMP_Text powerUpPurchasePriceText;
        [SerializeField] Image powerUpPurchaseIcon;

        [Space(5)]
        [SerializeField] Button smallCloseButton;
        [SerializeField] Button bigCloseButton;
        [SerializeField] Button purchaseButton;

        [Space(5)]
        [SerializeField] CurrencyUIPanelSimple currencyPanel;

        private PUSettings settings;

        private bool isOpened;
        public bool IsOpened => isOpened;

        private void Awake()
        {
            smallCloseButton.onClick.AddListener(ClosePurchasePUPanel);
            bigCloseButton.onClick.AddListener(ClosePurchasePUPanel);
            purchaseButton.onClick.AddListener(PurchasePUButton);
        }

        public void Initialise()
        {
            NotchSaveArea.RegisterRectTransform(safeAreaTransform);
        }

    public void Show(PUSettings settings)
    {
        this.settings = settings;

        currencyPanel.Initialise();

        powerUpPurchasePanel.SetActive(true);

        powerUpPurchasePreview.sprite = settings.Icon;
        powerUpPurchaseDescriptionText.text = settings.Description;
        powerUpPurchasePriceText.text = settings.Price.ToString();
        powerUpPurchaseAmountText.text = string.Format("x{0}", settings.PurchaseAmount);

        Currency currency = CurrenciesController.GetCurrency(settings.CurrencyType);
        powerUpPurchaseIcon.sprite = currency.Icon;

        // プレイヤーが購入に必要な通貨を持っているか確認し、ボタンの状態を設定
        if (settings.HasEnoughCurrency())
        {
            purchaseButton.interactable = true;  // 通貨が足りている場合はボタンをアクティブ
        }
        else
        {
            purchaseButton.interactable = false;  // 通貨が足りない場合はボタンを非アクティブ
        }

        UIController.OnPopupWindowOpened(this);
    }


    public void PurchasePUButton()
    {
        AudioController.PlaySound(AudioController.Sounds.buttonSound);

        bool purchaseSuccessful = PUController.PurchasePowerUp(settings.Type);

        // 購入が成功した場合のみパネルを閉じる
        if (purchaseSuccessful)
        {
            ClosePurchasePUPanel();
        }
        else
        {
            Debug.Log("Not enough currency to purchase power-up.");
            // UIIAPStoreには遷移しないように変更
        }
    }


        public void ClosePurchasePUPanel()
        {
            powerUpPurchasePanel.SetActive(false);

            UIController.OnPopupWindowClosed(this);
        }
    }
}