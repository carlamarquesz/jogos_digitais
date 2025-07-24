using UnityEngine;
using TMPro;  // ou UnityEngine.UI

public class GameClockUI : MonoBehaviour
{
    public TextMeshProUGUI clockText;  // ou public Text clockText;

    void Update()
    {
        if (GameClock.Instance != null && clockText != null)
        {
            int hour = GameClock.Instance.hour;
            int minute = GameClock.Instance.minute;
            clockText.text = $"{hour:D2}:{minute:D2}";
        }
    }
}
