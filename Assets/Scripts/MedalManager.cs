using UnityEngine;
using TMPro;

public class MedalManager : MonoBehaviour
{
    [SerializeField] int startMedals = 1000;  // 最初の所持メダル
    [SerializeField] int currentBet = 10;     // 初期BET数
    [SerializeField] TextMeshProUGUI medalText; // 所持メダル表示
    [SerializeField] TextMeshProUGUI betText;   // BET表示
    private int currentMedals;

    private void Start()
    {
        // ゲーム開始時のメダル初期化
        currentMedals = startMedals;
        UpdateUI();
    }
    public void AddMedals(int amount)
    {
        currentMedals += amount;
        UpdateUI();
    }

    public bool SpendMedals()
    {
        if (currentMedals >= currentBet)
        {
            currentMedals -= currentBet;
            UpdateUI();
            return true;
        }
        else
        {
            Debug.Log("メダルが足りません！");
            return false;
        }
    }

    // --- BET枚数をセット ---
    public void SetBet(int amount)
    {
        currentBet = amount;
        UpdateUI();
    }

    // --- 今の所持メダルを返す ---
    public int GetCurrentMedals()
    {
        return currentMedals;
    }

    // --- 今のBET数を返す ---
    public int GetCurrentBet()
    {
        return currentBet;
    }

    // --- UIを更新 ---
    private void UpdateUI()
    {
        if (medalText != null)
        {
            medalText.text = $"所持メダル: {currentMedals}";
        }

        if (betText != null)
        {
            betText.text = $"BET: {currentBet}";
        }
    }
}
