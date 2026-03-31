/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine.Audio;


[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{

    /* enums */
    public enum RepeatMode
    {
        None,
        Track,
        Playlist,
    };

    /* Global/Runtime Settings */
    [Tooltip("Audio Clips To Play")]
    [SerializeField] public List <AudioClip> Clips;
    [Tooltip("Current Audio Index To Play")]
    public int AudioIndex = 0;
    [Tooltip("Delay music start, (if using PlayOnStart) for set amount of seconds.")]
    public float StartDelay = 0f;
    [Tooltip("Play Music On Start")]
    public bool PlayOnStart = false;
    [Tooltip("Auto Shuffle.")]
    public bool Shuffle = false;
    [Tooltip("Looping Mode.")]
    public RepeatMode LoopMode = RepeatMode.None;
    [Tooltip("Target Audio Volume")]
    [Range(0f, 1f)]
    public float Volume = 1.0f;
    [Tooltip("Volume change time in seconds (0 -> instant)")]
    [Range(0f, 5f)]
    public float VolumeRampTime = 1.0f; // this is just lerp but time based for you programmers.
    [Tooltip("Playback Speed")]
    [Range(-5f, 5f)]
    public float PlayBackSpeed = 1.0f;
    [Tooltip("Playback speed change time in seconds (0 -> instant)")]
    [Range(0f, 2f)]
    public float PlaybackRampTime = 1f;
    [Tooltip("Fade Time (In Seconds) between Tracks")]
    [Range(0f, 10f)]
    public float FadeTime = 0.15f;

    [Tooltip("Playback History max length")]
    [Range(0f, 256f)]
    public int HistoryMax = 50;

    public float TrackLength 
    { 
        get 
        { 
            if(this.audioSource && this.audioSource.clip)
            {   return this.audioSource.clip.length;
            }

            return 0;
        } 
    }

    // Gets the current timestamp of the current song.
    public float CurrentTime
    { 
        get
        {
            if(this.audioSource != null && this.audioSource.clip != null)
            {   _CurrentTime = this.audioSource.time;
            }

            return _CurrentTime;
        }
        set
        {   
            if(this.audioSource == null || this.audioSource.clip == null)
            {   _CurrentTime = value;
            }
            else
            {
                _CurrentTime = Mathf.Clamp(value, 0f, this.audioSource.clip.length);

                this.audioSource.time = _CurrentTime;
            }
        }
    }

    public bool IsPlaying
    {
        get
        {   return this.audioSource != null && this.audioSource.clip != null && this.audioSource.isPlaying;
        }
        set
        {   
            if(this.audioSource == null)
            {   return;
            }

            if(this.audioSource == null || this.audioSource.clip == null)
            {   
                if(Clips.Count > 0)
                {   this.audioSource.clip = Clips[this.AudioIndex];
                }
            }

            if(this.audioSource != null && this.audioSource.clip != null)
            {   
                if(value)
                {   

                    if(this.Paused)
                    {   
                        this.audioSource.UnPause();
                        this.Paused = false;
                    }
                    else
                    {   
                        /* rewind any previous clips */
                        this.audioSource.Stop();
                        this.audioSource.Play();
                    }
                }
                else
                {   
                    this.audioSource.Pause();
                    this.Paused = true;
                }
            }
        }
    }

    public bool Paused { get; set; }

    // cant use new() because the type isnt enough to be descriptive ????
    private List<int> playHistory = new List<int>();

    // These are the sources we need 2 to prevent choppy transitions.

    [HideInInspector] private AudioSource audioSource;
    [HideInInspector] private AudioSource audioSourceBuff;

    // Loop back sampling.
    private int loopStartSample = 0;
    private int loopEndSample = 0;

    // current time private holder.
    private float _CurrentTime = 0.0f;
    // track fade time so we dont fade forever.
    private float fadeElapsed = 0f;
    // this just tracks courtine to to terminate if we call another fade.
    private Coroutine fadeCoroutine;
    // this is just a hack to fix the Update() stuff we did to call Next and Prev when we end music.
    private bool isNextPrevQueued = false;

    // Unused.
    private static float DbToVolume(float db)
    {   return Mathf.Pow(10f, db / 20f);
    }

    // Unused.
    // Convert linear volume to dB
    private static float VolumeToDb(float volume)
    {   return 20f * Mathf.Log10(Mathf.Max(volume, 0.0001f)); // avoid log(0)
    }

    // This gets the Music size, can only use in the editor though...
    private static long GetMusicSize(AudioClip clip)
    {
        long length = 0L;
#if UNITY_EDITOR
        if (clip == null)
        {
            Debug.LogWarning("No clip assigned!");
            return 0;
        }

        string path = AssetDatabase.GetAssetPath(clip);

        if (!File.Exists(path))
        {
            Debug.LogWarning("File not found at path: " + path);
            return 0;
        }

        FileInfo fileInfo = new FileInfo(path);

        length = fileInfo.Length;
#endif
        return length;
    }

    // This makes sure other programmers, artists etc... fix the clip size since most dont know how to do that...
    public static void AudioSizeTypeAssertion(AudioClip clip)
    {
#if UNITY_EDITOR
        float bytesToMiB = (1024f * 1024f);
        float mem = GetMusicSize(clip) / bytesToMiB;

        float DECOMPRESS_MAX_MIB = 5;
        float COMPRESS_MAX_MIB = 50;

        if(mem == 0f)
        {   return;
        }

        //Debug.Log(mem);

        switch(clip.loadType)
        {
            case AudioClipLoadType.DecompressOnLoad:
                if(mem > DECOMPRESS_MAX_MIB)
                {   
                    if(mem > COMPRESS_MAX_MIB)
                    {   
                        Debug.LogWarning($"The audio clip \"{clip.name}\" is too big to use with 'DecompressOnLoad' please switch to 'CompressInMemory'");
                    }
                    else
                    {   
                        Debug.LogWarning($"The audio clip \"{clip.name}\" is too big to use with 'DecompressOnLoad' please switch to 'Streaming'");
                    }
                }

                break;
            case AudioClipLoadType.CompressedInMemory:
                if(mem < DECOMPRESS_MAX_MIB)
                {   Debug.LogWarning($"The audio clip \"{clip.name}\" is too small to use with 'CompressInMemory' please switch to 'DecompressOnLoad'");
                }
                else if(mem > COMPRESS_MAX_MIB)
                {   Debug.LogWarning($"The audio clip \"{clip.name}\" is too big to use with 'CompressInMemory' please switch to 'Streaming'");
                }
                break;
            case AudioClipLoadType.Streaming:
                if(mem < DECOMPRESS_MAX_MIB)
                {   Debug.LogWarning($"The audio clip \"{clip.name}\" is too small to use with 'Streaming' please switch to 'DecompressOnLoad'");
                }
                else if(mem < COMPRESS_MAX_MIB)
                {   Debug.LogWarning($"The audio clip \"{clip.name}\" is too small to use with 'Streaming' please switch to 'CompressInMemory'");
                }
                break;
        }
#endif
    }

    // This fades 2 clips.
    private IEnumerator FadeBetween(AudioSource from, AudioSource to, float fadeTime)
    {
        float startFromVolume = from.volume;
        float startToVolume = to.volume;

        to.volume = startToVolume; 
        to.Play();

        float elapsed = this.fadeElapsed;

        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeTime;

            from.volume = Mathf.Lerp(startFromVolume, 0f, t);
            to.volume = Mathf.Lerp(startToVolume, Volume, t);

            yield return null;
        }

        from.Stop();

        from.volume = this.Volume;
        to.volume = this.Volume;

        this.audioSource = to;
        this.audioSourceBuff = from;
        this.isNextPrevQueued = false;
    }

    // This is the NextPrev Handler that handles the Play History and changing song.
    private void _NextPrev(int audioIndex)
    {
        // Play History
        playHistory.Add(this.AudioIndex);

        if(playHistory.Count > this.HistoryMax)
        {   playHistory.RemoveAt(0);
        }

        int prevIndex = this.AudioIndex;

        this.AudioIndex = audioIndex;

        if(fadeCoroutine != null)
        {   StopCoroutine(fadeCoroutine);
        }

        this.audioSourceBuff.clip = Clips[this.AudioIndex];

        this.fadeCoroutine = StartCoroutine(FadeBetween(this.audioSource, this.audioSourceBuff, this.FadeTime));
    }

    /// <summary> nukes russia </summary>
    public void Next()
    {
        int nextIndex;

        // shuffle has different behaviour.
        if(this.Shuffle)
        {
            // Pick a random track, avoid repeating current
            do
            {
                nextIndex = Random.Range(0, Clips.Count);
            } while (nextIndex == AudioIndex && Clips.Count > 1);

            playHistory.Add(this.AudioIndex);
        }
        else
        {   nextIndex = (this.AudioIndex + 1) % this.Clips.Count;
        }

        this._NextPrev(nextIndex);
    }

    // This calls the previous song.
    public void Prev()
    {
        // resume history if we have any
        if(playHistory.Count > 0)
        {
            int i = playHistory.Count - 1;
            int lastIndex = playHistory[i];

            playHistory.RemoveAt(i);
            this._NextPrev(lastIndex);
        }
        // else just do previous.
        else
        {   this._NextPrev((this.AudioIndex - 1 + this.Clips.Count) % this.Clips.Count);
        }
    }

    // This adds music.
    public void AddMusic(ref AudioClip clip)
    {   
        if(clip != null)
        {   AudioSizeTypeAssertion(clip);
        }

        this.Clips.Add(clip);
    }
    
    // This removes music
    public void RemoveMusic(ref AudioClip clip)
    {   
        this.Clips.Remove(clip);
    }

    // This plays music
    public void Play()
    {   this.IsPlaying = true;
    }

    // This pauses music
    public void Pause()
    {   this.IsPlaying = false;
    }

    // This stops music
    public void Stop()
    {   
        this.Pause();
        this.Paused = false;
    }

    // This plays or pauses music depending on if its playing.
    public void PlayPause()
    {   
        this.IsPlaying = !this.IsPlaying;
    }

    // This rewinds the clip and plays it.
    public void Rewind()
    {   
        this.Stop();
        this.Play();
    }

    // This seeks the playlist to the trackTimeSeconds vraible.
    public void Seek(float trackTimeSeconds)
    {   this.CurrentTime = trackTimeSeconds;
    }

    // Intiates a loopback to loop through in the song.
    public void SetLoopBack(float startTime, float endTime)
    {
        if(this.audioSource == null || this.audioSource.clip == null)
        {   return;
        }

        startTime = Mathf.Clamp(startTime, 0, this.TrackLength);
        endTime = Mathf.Clamp(endTime, 0, this.TrackLength);

        if(startTime > endTime)
        {   
            float tmp = startTime;
            Debug.LogWarning("Start time is greater than endTime.");

            startTime = endTime;
            endTime = tmp;
        }

        this.loopStartSample = (int)(startTime * this.audioSource.clip.frequency);
        this.loopEndSample = (int)(endTime * this.audioSource.clip.frequency);
    }

    // unsets the set loopback.
    public void UnsetLoopBack()
    {   
        this.loopStartSample = 0;
        this.loopEndSample = 0;
    }

    // do we have a loop back running ?
    public bool IsLoopBackEnabled()
    {   return this.loopStartSample != 0 && this.loopEndSample != 0;
    }

    // get the current clip playing.
    public AudioClip GetCurrentClip()
    {
        if(this.Clips.Count == 0)
        {   return null;
        }

        return this.Clips[this.AudioIndex];
    }

    // intialize stuff.
    void Start()
    {
        AudioSource[] sources = GetComponents<AudioSource>();

        if (sources.Length == 0)
        {
           this.audioSource = gameObject.AddComponent<AudioSource>();
           this.audioSourceBuff = gameObject.AddComponent<AudioSource>();
        }
        else if (sources.Length == 1)
        {
           this.audioSource = sources[0];
           this.audioSourceBuff = gameObject.AddComponent<AudioSource>();
        }
        else
        {
           this.audioSource = sources[0];
           this.audioSourceBuff = sources[1];
        }

        foreach(AudioClip clip in this.Clips)
        {   AudioSizeTypeAssertion(clip);
        }

        if(this.Clips.Count == 0)
        {   this.AudioIndex = 0;
        }
        else
        {
            // make sure no random stuff
            this.AudioIndex %= this.Clips.Count;
            this.audioSource.clip = Clips[this.AudioIndex];
        }


        if(this.PlayOnStart)
        {   this.Play();
        }
    }

    // handles all the dynamic changes we have
    void Update()
    {
        if(this.audioSource == null)
        {   return;
        }

        float playbackSpeed;
        float volume;


        if(this.VolumeRampTime <= 0f)
        {   volume = this.Volume;
        }
        else
        {   
            float step = Time.deltaTime / VolumeRampTime;

            volume = Mathf.MoveTowards(this.audioSource.volume, Volume, step);
        }

        if(this.PlaybackRampTime <= 0f)
        {   playbackSpeed = this.PlayBackSpeed;
        }
        else
        {   
            float step = Time.deltaTime / PlaybackRampTime;

            playbackSpeed = Mathf.MoveTowards(this.audioSource.pitch, this.PlayBackSpeed, step);
        }

        // Run Settings

        // reversal of sampling (by default) is not allowed in non decompressOnLoad clips for some reason.
        // TODO: Not implemented.
        if(this.audioSource.clip != null)
        {
            if(this.audioSource.clip.loadType == AudioClipLoadType.DecompressOnLoad)
            {   // Debug.Log("normal sampling...");
            }
            else if(playbackSpeed > 0f)
            {
            }
            else if(playbackSpeed < 0f)
            {
            }
        }

        this.audioSource.pitch = playbackSpeed;
        this.audioSource.volume = volume;
        this.audioSource.loop = false;

        this.loopStartSample = 0;
        this.loopEndSample = 0;

        // set the loop back plug
        if(this.IsLoopBackEnabled())
        {
            if(this.audioSource.timeSamples >= this.loopEndSample)
            {   this.audioSource.timeSamples -= this.loopEndSample - this.loopStartSample;
            }
        }

        if(this.LoopMode == RepeatMode.Track)
        {   this.audioSource.loop = true;
        }

        // set playlist behaviour.
        if(this.LoopMode == RepeatMode.Playlist)
        {
            if(this.PlayBackSpeed > 0)
            {
                if(this.CurrentTime >= this.TrackLength - (this.FadeTime * this.PlayBackSpeed))
                {   
                    if(!this.isNextPrevQueued)
                    {
                        this.Next();
                        this.isNextPrevQueued = true;
                    }
                }
            }
            else if (this.PlayBackSpeed == 0)
            {   // TODO.
            }
            else
            {
                if(this.CurrentTime <= (this.FadeTime * this.PlayBackSpeed))
                {   
                    if(!this.isNextPrevQueued)
                    {
                        this.Prev();
                        this.isNextPrevQueued = true;
                    }
                    
                }
            }
        }
    }

    // Inspector spoofing
    // This just makes it easier for devs to do debugging.
#if UNITY_EDITOR
    [CustomEditor(typeof(MusicPlayer))] 
    public class AudioTimeControllerEditor : Editor
    {
        private double lastRepaintTime = 0;
        private const double repaintInterval = .1; 

        private void OnEnable()
        {
            EditorApplication.update += EditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
        }

        private void EditorUpdate()
        {
            if (!Application.isPlaying) 
            {   return;
            }

            if (EditorApplication.timeSinceStartup - lastRepaintTime > repaintInterval)
            {
                lastRepaintTime = EditorApplication.timeSinceStartup;
                Repaint(); 
            }
        }

        public override void OnInspectorGUI()
        {
            MusicPlayer player = (MusicPlayer)target;

            DrawDefaultInspector();

            float val;
            bool playClick;
            bool prevClick;
            bool stopClick;
            bool nextClick;
            int buttonWidthPx = 40;

            int minutes = Mathf.FloorToInt(player.CurrentTime / 60f);
            int seconds = Mathf.FloorToInt(player.CurrentTime % 60f);

            string timeLabel = string.Format("[{0}:{1:00}]", minutes, seconds);

            val = EditorGUILayout.Slider(
                new GUIContent("Current Time: " + timeLabel),
                player.CurrentTime,
                0f,
                player.TrackLength
            );

            AudioClip audio = player.GetCurrentClip();
            string track = "";

            if(audio != null)
            {   track = audio.name;
            }

            EditorGUILayout.LabelField($"Track: [{track}]");

            if(!player.IsPlaying)
            {
                /* make sure we dont mess up other stuff */

                if(val != player.CurrentTime)
                {   player.Play();
                }
            }

            player.CurrentTime = val;

            EditorGUILayout.BeginHorizontal();

            playClick = GUILayout.Button("�", GUILayout.Width(buttonWidthPx));
            prevClick = GUILayout.Button("�", GUILayout.Width(buttonWidthPx));
            stopClick = GUILayout.Button("�", GUILayout.Width(buttonWidthPx));
            nextClick = GUILayout.Button("�", GUILayout.Width(buttonWidthPx));

            if(playClick)
            {   
                player.PlayPause();
                EditorUpdate();
            }

            if(nextClick)
            {   
                player.Next();
                EditorUpdate();
            }

            if(stopClick)
            {   
                player.Stop();
                EditorUpdate();
            }

            if(prevClick)
            {   
                player.Prev();
                EditorUpdate();
            }

            EditorGUILayout.EndHorizontal(); 
        }
    }
#endif
}
