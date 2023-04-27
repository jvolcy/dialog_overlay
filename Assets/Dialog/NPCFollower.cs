using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCFollower : MonoBehaviour
{
    public enum position {TOP, BOTTOM, AUTO};

    public Transform NPC_RootBone;  //the xform of the root bone of the NPC we will follow
    public Camera camera1 = null;   //the camera associated with the canvas;  null=autoselect
    public RectTransform TopPanel;     //RectXform of the top dialog panel
    public RectTransform BottomPanel;   //RectXform of the bottom dialog panel
    public RectTransform TopPointer;    //RectXform of the pointer associated with the top dialog panel
    public RectTransform BottomPointer; //RectXform of the pointer associated with the bottom dialog panel
    public position PanelLocation
    {
        get { return panelLocation; }
        set
        {
            panelLocation = value;

            switch (panelLocation)
            {
                case position.TOP:
                    TopPanel.gameObject.SetActive(true);
                    TopPointer.gameObject.SetActive(true);
                    BottomPanel.gameObject.SetActive(false);
                    BottomPointer.gameObject.SetActive(false);
                    break;
                case position.BOTTOM:
                    TopPanel.gameObject.SetActive(false);
                    TopPointer.gameObject.SetActive(false);
                    BottomPanel.gameObject.SetActive(true);
                    BottomPointer.gameObject.SetActive(true);
                    break;
                case position.AUTO:
                    break;
            }
        }
    }
    position panelLocation = position.AUTO;

    RectTransform canvasRectTransform;  //the RectXform of the canvas

    Transform NPC_Head_Top; //transform of the NPC head end bone
    Transform NPC_Head_Bottom;  //transform of the NPC neck bone

    // Start is called before the first frame update
    void Start()
    {

        //get a reference to the canvas RectXform.  We first search for the
        //canvas in the dialog's parent.
        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        //if no camera is specified, default to the main camera
        if (camera1 == null)
        {
            camera1 = Camera.main;
        }

        //find the head end bone and the neck bone of the NPC
        FindHeadBones(NPC_RootBone);
    }

    // Update is called once per frame
    void Update()
    {
        //we need to compute the position of the top and bottom pointers so that
        //they appear to emerge from the top and bottom panels.  This is a multi-
        //step process.

        //1) compute the normalized canvas coordinates of the nech and head end bones
        Vector2 npc_head_top_pos = camera1.WorldToViewportPoint(NPC_Head_Top.position);
        Vector2 npc_head_bottom_pos = camera1.WorldToViewportPoint(NPC_Head_Bottom.position);

        //2) perform a component-wise multiplication with the canvas size vector to
        //compute the pixel positions for the top and bottom pointers
        Vector2 TopPos = canvasRectTransform.rect.size * npc_head_top_pos;
        Vector2 BottomPos = canvasRectTransform.rect.size * npc_head_bottom_pos;

        //3) set the start position (on the NPC) of the top and bottom pointers.
        TopPointer.position = TopPos + 3 * Vector2.up;//scoot up 3 pixels (not sure why this is needed, but without it there is a gap between the dialog box and the pointer
        BottomPointer.position = BottomPos - 3 * Vector2.up;//scoot down 3 pixels (not sure why this is needed, but without it there is a gap between the dialog box and the pointer

        //4) scale the pointers so that they exactly fill the space between the
        //NPC's head and the top and bottom dialog panels.
        float TopHeight = canvasRectTransform.rect.height - TopPanel.rect.height - TopPos.y;
        TopPointer.sizeDelta = new Vector2(TopPointer.sizeDelta.x, TopHeight ); //only chage y

        float BottomHeight = BottomPos.y - BottomPanel.rect.height;
        BottomPointer.sizeDelta = new Vector2(BottomPointer.sizeDelta.x, BottomHeight); //only change y

        //5) enable/disable panels as necessary
        if (panelLocation == position.AUTO)
        {
            if (npc_head_top_pos.y + npc_head_bottom_pos.y < 1)
            {
                TopPanel.gameObject.SetActive(true);
                TopPointer.gameObject.SetActive(true);
                BottomPanel.gameObject.SetActive(false);
                BottomPointer.gameObject.SetActive(false);
            }
            else
            {
                TopPanel.gameObject.SetActive(false);
                TopPointer.gameObject.SetActive(false);
                BottomPanel.gameObject.SetActive(true);
                BottomPointer.gameObject.SetActive(true);
            }
        }
    }

    /**
     * ChatGPT assisted code to search for certain bones in a humanoid rig given
     * the root bone.  In this case, we are searching for the head end bone
     * (top of the head) and the neck bone (bottom of the head).  We assume that
     * the names of these bones endswith "HEADTOP_END" and "NECK" respectively.
     * If the search fails, NPC_Head_Top and/or NPC_Head_Bottom is set to null.
     **/
    void FindHeadBones(Transform rootBone)
    {
        NPC_Head_Top = null;
        NPC_Head_Bottom = null;

        //get all bones: these are all the children of the root bone
        Transform[] allBones = rootBone.GetComponentsInChildren<Transform>(); // Get all bones

        //search each bone for a name match
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
