using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;

    public AudioSource audioSource;
    public GameManager gameManager;
    public float songDelayInSeconds;
    public double timingWindow; // timing window for the player, in seconds.
    public int inputDelay; //in milliseconds

    public string fileLocation;

    // Parts that might be different we should see if we need
    public float noteTime; // supposed to be speed from point A to point B
    public Transform noteSpawn; // y pos, will probably be changed.
    public Transform noteTrigger; // where the note is "hit", y pos, will probably be changed
    public Transform despawnPos; // position of despawn, probably will change.

    public static MidiFile midiFile;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetBeatmapFromMidi();
    }

    private void GetBeatmapFromMidi()
    {
        var notes = midiFile.GetNotes();
        var arr = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(arr, 0);
        
        gameManager.SetTimeStamps(arr);
        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        audioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
