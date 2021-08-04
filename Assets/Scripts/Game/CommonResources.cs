using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonResources
{
    public static GameManager game_manager;

    private static PlayerController player;
    public static PlayerController Player
    {
        get { return player; }
    }

    private static Dictionary<string, AudioClip> audio_files;
    private static Dictionary<string, GameObject> object_prefabs;
    public static GameObject dialogue_box_prefab;

    public static float TextSpeedFast
    {
        get { return 0.04f; }
    }

    public static float TextSpeedSlow
    {
        get { return 0.10f; }
    }

    public static void Initialize(GameManager m_game_manager)
    {
        game_manager = m_game_manager;
        player = game_manager.Player;

        InitializeAudioFiles();
        InitializePrefabs();
    }

    private static void InitializeAudioFiles()
    {
        audio_files = new Dictionary<string, AudioClip>();

        audio_files.Add("PlayerHit", Resources.Load("Audio/HitVocal1_Truncated") as AudioClip);
        audio_files.Add("EnemyHit", Resources.Load("Audio/EnemyHit") as AudioClip);
        audio_files.Add("ChestOpen", Resources.Load("Audio/ChestOpen") as AudioClip);
        audio_files.Add("DoorOpen", Resources.Load("Audio/DoorOpen") as AudioClip);
        audio_files.Add("PoisonCloud", Resources.Load("Audio/PoisonCloud") as AudioClip);
        audio_files.Add("TextBlip", Resources.Load("Audio/TextBlip") as AudioClip);
        audio_files.Add("TextBlip2", Resources.Load("Audio/TextBlip2") as AudioClip);
        audio_files.Add("Potion", Resources.Load("Audio/Potion") as AudioClip);
        audio_files.Add("KeyPickup", Resources.Load("Audio/KeyPickup") as AudioClip);
        audio_files.Add("Slash", Resources.Load("Audio/Sword") as AudioClip);
    }

    private static void InitializePrefabs()
    {
        object_prefabs = new Dictionary<string, GameObject>();

        dialogue_box_prefab = Resources.Load<GameObject>("DialogueBox");

        object_prefabs.Add("PoisonCloud", Resources.Load<GameObject>("Prefab/PoisonChestParticle"));
        object_prefabs.Add("EnemyDeath", Resources.Load<GameObject>("Prefab/EnemyDeath"));
    }

    public static AudioClip GetAudioClip(string name)
    {
        if (audio_files.ContainsKey(name))
        {
            return audio_files[name];
        }
        else
        {
            return null;
        }
    }

    public static GameObject GetPrefab(string name)
    {
        if (object_prefabs.ContainsKey(name))
        {
            return object_prefabs[name];
        }
        else
        {
            return null;
        }
    }

    public static void PlaySound(string name, float volume)
    {
        if (game_manager != null)
        {
            if (game_manager.Audio_Source != null)
            {
                AudioClip clip = GetAudioClip(name);

                if (clip != null)
                {
                    game_manager.Audio_Source.PlayOneShot(clip, volume);
                }
            }
        }
    }

    public static void PlaySound(AudioClip clip, float volume)
    {
        if (game_manager != null)
        {
            if (game_manager.Audio_Source != null)
            {
                game_manager.Audio_Source.PlayOneShot(clip, volume);
            }
        }
    }
}
