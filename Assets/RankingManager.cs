using UnityEngine;

public static class RankingManager
{
    public static void SalvarPontuacao(int pontos, float tempo)
    {
        for (int i = 0; i < 10; i++)
        {
            string chave = "Top" + i;

            if (!PlayerPrefs.HasKey(chave))
            {
                PlayerPrefs.SetString(chave, pontos + "|" + tempo);
                return;
            }

            string[] partes = PlayerPrefs.GetString(chave).Split('|');
            int pontosSalvos = int.Parse(partes[0]);
            float tempoSalvo = float.Parse(partes[1]);

            if (pontos > pontosSalvos || (pontos == pontosSalvos && tempo < tempoSalvo))
            {
                // Move entradas para baixo
                for (int j = 9; j > i; j--)
                {
                    string anterior = PlayerPrefs.GetString("Top" + (j - 1), "");
                    PlayerPrefs.SetString("Top" + j, anterior);
                }

                PlayerPrefs.SetString(chave, pontos + "|" + tempo);
                return;
            }
        }
    }

    public static void ZerarRanking()
    {
        for (int i = 0; i < 10; i++)
        {
            PlayerPrefs.DeleteKey("Top" + i);
        }
    }

    public static string[] ObterRankingFormatado()
    {
        string[] ranking = new string[10];

        for (int i = 0; i < 10; i++)
        {
            string entry = PlayerPrefs.GetString("Top" + i, "");
            if (!string.IsNullOrEmpty(entry))
            {
                string[] partes = entry.Split('|');
                int pontos = int.Parse(partes[0]);
                float tempo = float.Parse(partes[1]);
                int min = Mathf.FloorToInt(tempo / 60f);
                int seg = Mathf.FloorToInt(tempo % 60f);
                ranking[i] = $"#{i + 1} - Pontos: {pontos:D3} - Tempo: {min:D2}:{seg:D2}";
            }
            else
            {
                ranking[i] = $"#{i + 1} - (vazio)";
            }
        }

        return ranking;
    }
}
