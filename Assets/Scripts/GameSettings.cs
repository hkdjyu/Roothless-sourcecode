using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new GameResources", menuName = "RootGame/GameSettings" )]
public class GameSettings : ScriptableObject
{
    [Range(0f, 0.5f)]
    [SerializeField]
    private float musicVolume = 0.5f;
    public float MusicVolume {
        get { return musicVolume; }
        set {
            musicVolume = value;
            musicVolumeUpdated?.Invoke(musicVolume);
        }
    }

    [Range(0f, 1f)]
    [SerializeField]
    private float sfxVolume = 1;
    public float SfxVolume {
        get { return sfxVolume; }
        set {
            sfxVolume = value;
            sfxVolumeUpdated?.Invoke(sfxVolume);
        }
    }

    [Range(0f, 1f)] [SerializeField] private float mouseSensitivity = 0.5f;

    public float MouseSensitivity {
        get { return mouseSensitivity;}
        set {
            mouseSensitivity = value;
            mouseSensitivityUpdated?.Invoke(mouseSensitivity);
        }
    }

    public event Action<float> musicVolumeUpdated, sfxVolumeUpdated, mouseSensitivityUpdated;

}