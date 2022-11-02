using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCamera : MonoBehaviour
{
    public Camera camera;
    public bool inModeB = false;

    public string aLayer;
    public string bLayer;

    private LayerMask aLayerMask;
    private LayerMask bLayerMask;
    
    // Start is called before the first frame update
    void Start()
    {
        aLayerMask = LayerMask.NameToLayer(aLayer);
        bLayerMask = LayerMask.NameToLayer(bLayer);
    }

    public void ToggleCullingMasks()
    {
        // If we're currently in mode A, unset A layer and set B layer. 
        if (!inModeB)
        {
            camera.cullingMask |= (1 << aLayerMask);
            camera.cullingMask &= ~(1 <<bLayerMask);
        }
        else
        {
            // Otherwise, Unset B layer and set A layer. 
            camera.cullingMask |= (1 << bLayerMask);
            camera.cullingMask &= ~(1 <<aLayerMask);
        }
        inModeB = !inModeB;
    }
    
}
