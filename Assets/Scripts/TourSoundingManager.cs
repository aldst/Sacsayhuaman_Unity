using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourSoundingManager : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    public List<AudioClip> spanishClips;
    public List<AudioClip> englishClips;

    private int currentClipIndex = -1;

    private Dictionary<Language, List<AudioClip>> clipsDict = new Dictionary<Language, List<AudioClip>>();

    [HideInInspector]
    public Language currentLanguage = Language.english;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {

        clipsDict[Language.english] = englishClips;
        clipsDict[Language.spanish] = spanishClips;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void handleNewDestiny(string soundIndex)
    {
        if (soundIndex != "-")
        {
            int newClipIndex = int.Parse(soundIndex) - 1;
            if (currentClipIndex != newClipIndex)
            {
                audioSource.clip = clipsDict[currentLanguage][newClipIndex];
                currentClipIndex = newClipIndex;
                audioSource.Play();

            }
        }
    }
    public void HandleLanguageChange(Language newLang)
    {
        currentLanguage = newLang;
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}