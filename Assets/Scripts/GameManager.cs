using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    // Variable references from 
    public enum BTSObject { HitPieces, MissPieces, FlyingRod }
    
    [Header("References")]
    public SkinnedMeshRenderer environmentRenderer;
    public GameObject flyingRodBase;
    public GameObject movingBlocksBase;
    public GameObject hitPartsBase;
    public GameObject missPartsBase;
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
    public Melanchall.DryWetMidi.MusicTheory.NoteName blockSpawnNote;
    public Melanchall.DryWetMidi.MusicTheory.NoteName hitNote;
    // Input would go here. Will probably get it from input action manager?
    private List<Note> notes = new List<Note>();
    public List<double> timeStamps = new List<double>();

    private int spawnIndex = 0;
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
        UpdateConveyorBelt();
    }

    private void UpdateConveyorBelt()
    {
        currentBeltOffset = (currentBeltOffset + Time.deltaTime * -beltSpeed) % 1f;
        beltMaterial.mainTextureOffset = new Vector2(0f, currentBeltOffset);
        environmentRenderer.materials = environmentMaterials;
    }

    public void SetTimeStamps()
    {
        
    }
}
