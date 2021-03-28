using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer shared;
    public AudioSource player;
    public AudioClip[] songs;
    public float duration;
    public float targetVolume;

    public int currentSong = 0;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (shared == null)
        {
            shared = this;
        }
        else if (shared != this)
        {
            Destroy(this.gameObject);
        }
        player = GetComponent<AudioSource>();
        StartCoroutine(StartFade(player, duration,targetVolume));
    }

    public static IEnumerator StartFade(AudioSource player,float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = player.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            player.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

        // Update is called once per frame
        void Update()
    {
        
        if (!player.isPlaying || Input.GetKeyDown(KeyCode.Backspace))
        {
            currentSong++;
            currentSong = currentSong % songs.Length;
            player.clip = songs[currentSong];
            player.Play();
        }
    }
}
