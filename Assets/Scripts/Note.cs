using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private double _timeInstantiated;
    public Animator animator;
    public float assignedTime;

    public float length; // how slow/fast the note should go
    // Start is called before the first frame update
    void Start()
    {
        _timeInstantiated = SongManager.GetAudioSourceTime();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    // void Update()
    // {
    //     double timeSinceInstantiated = SongManager.GetAudioSourceTime() - _timeInstantiated;
    //     float t = (float)(timeSinceInstantiated / (SongManager.Instance.noteTime * 2));
    //     
    //     // This part will likely change to account for spawning game objects will already be at the correct location.
    //     if (t > 1)
    //     {
    //         Destroy(gameObject);
    //     }
    //     else
    //     {
    //         // this is calling for a lerp function but I think we can do something with the whole animation.
    //         // The WIP phase may do best to have a single cube moving across the track from position A to position B
    //         // See their lerp code otherwise.
    //         transform.localPosition = Vector3.Lerp(SongManager.Instance.noteSpawn.position,
    //             SongManager.Instance.despawnPos.position, t);
    //     }
    // }

    const int animFPS = 24;
    const int hitFrame = 39;
    const int animTotalFrames = 80;

    // Set the timing of the animation.
    public void SetTiming(float spawnTime, float hitTime, float bpm)
    {
        length = hitTime - spawnTime;
        float secondsPerFrame = 1f / animFPS;
        float secondsToHitFrame = secondsPerFrame * hitFrame;
        float speedMult = secondsToHitFrame / length;


        animator.Play("Move", 0, 0);
        animator.speed = speedMult;
    }
}
