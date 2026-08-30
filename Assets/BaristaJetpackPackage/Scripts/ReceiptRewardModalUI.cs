using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReceiptRewardModalUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject receiptModalRoot;
    public Image receiptBackgroundImage;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI loyaltyPointsText;

    [Header("Buttons")]
    public Button collectPointsButton;
    public Button watchAdButton;

    public void ShowRewardModal(int finalScore, int loyaltyPointsEarned)
    {
        if (receiptModalRoot != null) receiptModalRoot.SetActive(true);

        if (scoreText != null)
            scoreText.text = $"Score: {finalScore}";

        if (loyaltyPointsText != null)
            loyaltyPointsText.text = $"+{loyaltyPointsEarned} Loyalty Points";
    }

    public void OnClickCollectPoints()
    {
        Debug.Log("Gửi điểm Loyalty về POS Server...");
        // Gọi GameRewardService để gửi S2S sang POS Backend
        CloseModal();
    }

    public void OnClickWatchAd()
    {
        Debug.Log("Chạy Rewarded Ad để x2 điểm...");
        // Kích hoạt Unity Ads / AdMob SDK
    }

    public void CloseModal()
    {
        if (receiptModalRoot != null) receiptModalRoot.SetActive(false);
    }
}
