using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera1;
    public Transform AmyRootBone;
    public Transform MichelleRootBone;
    public Transform SpelmaniteRootBone;

    Transform AmyHeadT;
    Transform MichelleHeadT;
    Transform SpelmaniteHeadT;

    // Start is called before the first frame update
    void Start()
    {
        AmyHeadT = FindHeadBone(AmyRootBone);
        SpelmaniteHeadT = FindHeadBone(SpelmaniteRootBone);
        MichelleHeadT = FindHeadBone(MichelleRootBone);

        //print the normalized screen position of the head xforms
        Debug.Log("Michelle's head position = " + camera1.WorldToViewportPoint(MichelleHeadT.position));
        Debug.Log("Amy's head position = " + camera1.WorldToViewportPoint(AmyHeadT.position));
        Debug.Log("Spelmanite's head position = " + camera1.WorldToViewportPoint(SpelmaniteHeadT.position));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    Transform FindHeadBone(Transform rootBone)
    {
        Transform headBone = null;
        
        Transform[] allBones = rootBone.GetComponentsInChildren<Transform>(); // Get all bones

        foreach (Transform bone in allBones)
        {
            if (bone.name.ToUpper().EndsWith("HEAD"))
            {
                Debug.Log("Found bone: " + bone.name);
                headBone = bone; // Found the head bone
                return headBone;
            }
        }

        Debug.LogError("Head bone not found in Avatar!"); // Head bone not found, show an error message
        return null;


    }
}
