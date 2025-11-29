using UnityEngine;

public static class SaveManager
{
    private const string MATCHES_KEY = "TotalMatches";
    private const string TURNS_KEY = "TotalTurns";

    public static int TotalMatches
    {
        get => PlayerPrefs.GetInt(MATCHES_KEY, 0);
        set => PlayerPrefs.SetInt(MATCHES_KEY, value);
    }

    public static int TotalTurns
    {
        get => PlayerPrefs.GetInt(TURNS_KEY, 0);
        set => PlayerPrefs.SetInt(TURNS_KEY, value);
    }

    public static void SaveNow()
    {
        PlayerPrefs.Save();
    }
}
