using System.Collections.Generic;
using UnityEngine;

public class BossSoundEffectLibrary : MonoBehaviour
{
    [SerializeField] private BossSoundEffectGroup[] soundEffectGroups;
    private Dictionary<string, List<AudioClip>> soundDictionary;

    void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        soundDictionary = new Dictionary<string, List<AudioClip>>();

        foreach(BossSoundEffectGroup bossSoundEffectGroup in soundEffectGroups)
        {
            soundDictionary[bossSoundEffectGroup.name] = bossSoundEffectGroup.audioClips;
        }
    }

    public AudioClip GetRandomClip(string name)
    {
        if(soundDictionary.ContainsKey(name))
        {
            List<AudioClip> audioClips = soundDictionary[name];
            if(audioClips.Count > 0)
            {
                return audioClips[Random.Range(0, audioClips.Count)];
            }
        }
        return null;
    }
}

[System.Serializable]
public struct BossSoundEffectGroup
{
    public string name;
    public List<AudioClip> audioClips;
}
