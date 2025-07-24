using UnityEngine;

public class GameClock : MonoBehaviour
{
    public static GameClock Instance;

    public int hour = 6;
    public int minute = 0;
    public float secondsPerMinute = 1f;  // Duração em segundos para 1 minuto do jogo

    private float timer = 0f;

    // Propriedade pública para indicar se é "noite" (00:00 às 03:59)
    public bool IsNightTime => hour >= 0 && hour < 4;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= secondsPerMinute)
        {
            timer -= secondsPerMinute;
            IncrementMinute();
        }
    }

    void IncrementMinute()
    {
        minute++;
        if (minute >= 60)
        {
            minute = 0;
            hour++;
            if (hour >= 24)
                hour = 0;
        }
        // Debug para acompanhar o tempo
        Debug.Log($"Hora atual do jogo: {hour:D2}:{minute:D2}");
    }
}
