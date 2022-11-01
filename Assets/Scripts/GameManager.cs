using System;
using System.Collections;
using System.Collections.Generic;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    // Variable references from https://github.com/megaminerjenny/HeavenStudio/blob/master/Assets/Scripts/Games/BuiltToScaleDS/BuiltToScaleDS.cs
    // For now, since the player is the shooter in VR. Some animators and variables may not be used.
    public enum BTSObject { HitPieces, MissPieces, FlyingRod }
    [Header("References")]
    public SkinnedMeshRenderer environmentRenderer;
    public GameObject flyingRodBase;
    public GameObject movingBlocksBase;
    public GameObject hitPartsBase;
    public GameObject missPartsBase;
    public GameObject testBase;
    public Transform partsHolder;
    public Transform blocksHolder;
    public Animator shooterAnim;
    public Animator elevatorAnim;

    [Header("Properties")]
    public float beltSpeed = 1f;
    private Material beltMaterial;
    private Material[] environmentMaterials;
    private float currentBeltOffset;

    [Header("Song Properties")] 
    public NoteName blockSpawnNote;
    public NoteName hitNote;
    // Input would go here. Will probably get it from input action manager?
    private List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();
    public List<double> spawnTimeStamps = new List<double>();

    private int _spawnIndex = 0;
    private int inputIndex = 0;

    [NonSerialized] public bool shootingThisFrame;
    
    private void Awake()
    {
        environmentMaterials = environmentRenderer.materials;
        beltMaterial = Instantiate(environmentMaterials[8]);
        environmentMaterials[8] = beltMaterial;
        elevatorAnim.Play("MakeRod", 0, 1f);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= spawnTimeStamps[_spawnIndex])
            {
                var note = Instantiate(testBase, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[_spawnIndex];
                _spawnIndex++;
            }
        }

        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double timingWindow = SongManager.Instance.timingWindow;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelay / 1000.0);

            // @TODO Get VR Input
            // Also TODO, we don't want to mess up the input index if the player slaps the button a lot?
            // Might need to confirm this isn't buggy.
            if (Input.GetMouseButtonDown(0))
            {
                if (Math.Abs(audioTime - timeStamp) < timingWindow)
                {
                    Hit();
                    inputIndex++;
                }
                else
                {
                    SlightMiss(); // might need to play around with an else-if for if they're close to touching but haven't yet.
                    print($"Hit inaccurate on {inputIndex} note with {Math.Abs(audioTime - timeStamp)} delay");
                }
            }

            if (timeStamp + timingWindow <= audioTime)
            {
                Miss();
                inputIndex++;
            }
        }

        UpdateConveyorBelt();
    }

    private void Miss()
    {
        print($"Missed {inputIndex} note");
    }

    private void SlightMiss()
    {
        // Shoot
        // Spawn the missed pieces prefab
        // destroy the note
        // play the "crumble" sfx
    }

    private void Hit()
    {
        print($"Hit on {inputIndex} note");
        // run the shoot function
        // Spawn a hit piece prefab
        // play the "hit" sfx
        Destroy(notes[inputIndex].gameObject);
    }

    private void UpdateConveyorBelt()
    {
        currentBeltOffset = (currentBeltOffset + Time.deltaTime * -beltSpeed) % 1f;
        beltMaterial.mainTextureOffset = new Vector2(0f, currentBeltOffset);
        environmentRenderer.materials = environmentMaterials;
    }

    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName != hitNote && note.NoteName != blockSpawnNote)
            {
                Debug.LogWarning("Invalid note found in midi beatmap: " + note.NoteName);
                continue;
            }
            
            var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
            //convert minutes to milliseconds. 
            double convertedTime = (double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                                   (double)metricTimeSpan.Milliseconds / 1000f;
            if (note.NoteName == hitNote)
            {
                timeStamps.Add(convertedTime);
            }
            else spawnTimeStamps.Add(convertedTime);
        }
        
        if (spawnTimeStamps.Count != timeStamps.Count) Debug.LogWarning("Input events and spawn events don't match. Check your beatmap?");
    }
}
