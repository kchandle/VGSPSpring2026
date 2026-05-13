using System.Collections.Generic;
using UnityEngine;

public class PlayerMusicManager : MonoBehaviour
{
    public enum MusicIndexes
    {
        Menu,
        RoamOne,
        RoamTwo,
        BattleOne,
        BattleTwo,
        BattleWin,
        BattleLose,
        SecretFound,
        CardShop,
        Spooky,
        ReginaldNonBattle,
        ReginaldBattle,
        Evelmart,
        LAST,
    }

    [System.Serializable]
    public class MusicEntry
    {
        public MusicIndexes index;
        public AudioClip clip;
    }

    public List<MusicEntry> musicEntries = new();

    private Dictionary<MusicIndexes, AudioClip> indexes = new();

    private MusicPlayer musicPlayer;

    void Awake()
    {
        musicPlayer = GetComponent<MusicPlayer>();

        if (musicPlayer == null)
        {
            Debug.LogError("MusicPlayer component is missing!");
        }

        // Convert list → dictionary at runtime
        foreach (var entry in musicEntries)
        {
            if (!indexes.ContainsKey(entry.index))
            {
                indexes.Add(entry.index, entry.clip);
            }
        }

        foreach (MusicIndexes index in System.Enum.GetValues(typeof(MusicIndexes)))
        {
            Debug.Log(index);
        }

    }
}