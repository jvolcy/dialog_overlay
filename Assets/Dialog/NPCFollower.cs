using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollower : MonoBehaviour
{
    public Transform NPC_RootBone;
    public Camera camera1 = null;
    bool bottom = false;

    RectTransform canvasRectTransform;
    //RectTransform parentRectTransform;
    RectTransform rectTransform;

    Transform NPC_Head_Top;
    Transform NPC_Head_Bottom;

    // Start is called before the first frame update
    void Start()
    {
        //get a reference to our RectTransform
        rectTransform = GetComponent<RectTransform>();

        //get a reference to the canvas
        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        //get our parent's RectTransform
        //parentRectTransform = GetComponentInParent<RectTransform>();


        if (camera1 == null)
        {
            camera1 = Camera.main;
        }

        FindHeadBones(NPC_RootBone);

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 npc_pos = camera1.WorldToViewportPoint(NPC_Head_Top.position);
        //perform a component-wise multiplication of the 2 Vector2s
        Vector2 Pos = canvasRectTransform.rect.size * npc_pos;
        //Vector2 Pos = parentRectTransform.rect.size * npc_pos;
        rectTransform.position = Pos;
    }

    void FindHeadBones(Transform rootBone)
    {
        NPC_Head_Top = null;
        NPC_Head_Bottom = null;

        Transform[] allBones = rootBone.GetComponentsInChildren<Transform>(); // Get all bones

        foreach (Transform bone in allBones)
        {
            if (bone.name.ToUpper().EndsWith("HEADTOP_END"))
            {
                //Debug.Log("Found bone: " + bone.name);
                NPC_Head_Top = bone; // Found the head bone
            }

            if (bone.name.ToUpper().EndsWith("NECK"))
            {
                //Debug.Log("Found bone: " + bone.name);
                NPC_Head_Bottom = bone; // Found the neck bone
            }
        }

    }
}
