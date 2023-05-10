using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptionTail : MonoBehaviour
{
    public enum position { TOP, BOTTOM, BOTH, AUTO };

    //[Header("NPC and Camera")]

    [Tooltip("The Transform of the root bone of the NPC we will follow.  " +
        "This is often called \"Hips\" or \"Armature\".")]
    //the xform of the root bone of the NPC we will follow.  The top and bottom
    //tail bones are automatically detected by searching the bone heirearchy
    //starting from the root bone.  Alternatively, use the TopCaptionBone
    //BottomCaptionBone properties to set the top and bottom bones
    //and eliminate the search entirely.
    public Transform NPC_RootBone
    {
        //find the top and bottom caption tail follower bones of the NPC once the root bone is assigned
        set
        {
            NPC_TopBone = FindBoneWithNameSuffix(value, "HEADTOP_END");
            if (NPC_TopBone == null)
            {
                Debug.Log("Did not find HeadTop_End bone.... using " + value.name);
                //if we didn't find headtop_end bone, use the root bone
                NPC_TopBone = value;
            }

            NPC_BottomBone = FindBoneWithNameSuffix(value, "NECK");
            if (NPC_BottomBone == null)
            {
                Debug.Log("Did not find Neck bone.... using " + value.name);
                //if we didn't find neck bone, use the root bone
                NPC_BottomBone = value;
            }
        }
    }

    /**
     * TopCaptionBone and BottomCaptionBone properties:
     * Rather than searching for the top and bottom caption tail follower bones
     * using the FindBoneWithNameSuffix() function, the caption tail follow bones
     * may be explicitly set through these properties.  This eliminates the overhead
     * of searching the bone heirearchy.  This overhead becomes significant
     * when a single UICaption is used with multiple NPCs.  In such a case,
     * the follower bone needs to be switched from NPC to NPC every time
     * the dialog switches owner.
     * **/
    public Transform TopCaptionBone
    {
        get { return NPC_TopBone; }
        set { NPC_TopBone = value; }
    }

    public Transform BottomCaptionBone
    {
        get { return NPC_BottomBone; }
        set { NPC_BottomBone = value; }
    }

    Transform NPC_TopBone = null; //transform of the NPC top follower bone
    Transform NPC_BottomBone = null;  //transform of the bottom follower bone


    [Tooltip("The camera associated with the UI Canvas.  Leave blank to autoselect.")]
    public Camera camera1 = null;   //the camera associated with the canvas;

    [Header("Panels")]
    [Tooltip("Reference to the top panel object.  Do not edit.")]
    public RectTransform TopPanel;     //RectXform of the top UICaption panel
    [Tooltip("Reference to bottom top panel object.  Do not edit.")]
    public RectTransform BottomPanel;   //RectXform of the bottom UICaption panel
    [Tooltip("Reference to the top caption tail object.  Do not edit.")]
    public RectTransform TopTail;    //RectXform of the caption tail associated with the top UICaption panel
    [Tooltip("Reference to bottom caption tail object.  Do not edit.")]
    public RectTransform BottomTail; //RectXform of the caption tail associated with the bottom UICaption panel

    [Tooltip("Specify the location of the UICaption panel.  AUTO lets the " +
"system decide based on the position of the NPC on the screen.")]
    //property to specify the location of the UICaption panel.  AUTO lets the
    //system decide based on the position of the NPC on the screen.
    public position PanelLocation
    {
        get { return panelLocation; }
        set
        {
            panelLocation = value;

            switch (panelLocation)
            {
                case position.TOP:
                    //enable the top panel and tail; disable the bottom
                    TopPanel.gameObject.SetActive(true);
                    TopTail.gameObject.SetActive(true);
                    BottomPanel.gameObject.SetActive(false);
                    BottomTail.gameObject.SetActive(false);
                    break;
                case position.BOTTOM:
                    //enable the bottom panel and tail; disable the top
                    TopPanel.gameObject.SetActive(false);
                    TopTail.gameObject.SetActive(false);
                    BottomPanel.gameObject.SetActive(true);
                    BottomTail.gameObject.SetActive(true);
                    break;
                case position.BOTH:
                    //enable both top and bottom panels and tails
                    TopPanel.gameObject.SetActive(true);
                    TopTail.gameObject.SetActive(true);
                    BottomPanel.gameObject.SetActive(true);
                    BottomTail.gameObject.SetActive(true); break;
                case position.AUTO:
                    //the syste will decide which panel to enable/disable
                    break;
            }
        }
    }


    position panelLocation = position.AUTO;  //backing field for PanelLocation attribute

    [Tooltip("The point on the screen where we switch from bottom to top " +
    "panel when in AUTO mode.  50% is the middle of the screen.  0% is the " +
    "bottom and 100% is the top")]
    [Range(0f, 1f)]
    public float autoPlacementUpperTriggerPoint = 0.51f;  //The point on the screen
                                                          //where we switch from bottom to top panel placement in AUTO mode
                                                          //50% is the middle of the screen.  0% is the bottom. 100% is the top.

    [Tooltip("The point on the screen where we switch from top to bottom " +
    "panel when in AUTO mode.  50% is the middle of the screen.  0% is the " +
    "bottom and 100% is the top")]
    [Range(0f, 1f)]
    public float autoPlacementLowerTriggerPoint = 0.49f;  //The point on the screen
                                                          //where we switch from to to bottom panel placement in AUTO mode
                                                          //50% is the middle of the screen.  0% is the bottom. 100% is the top.


    RectTransform canvasRectTransform;  //the RectXform of the canvas


    // Start is called before the first frame update
    void Start()
    {

        //get a reference to the canvas RectXform.  We first search for the
        //canvas in the UICaptionTail's parent.
        Canvas canvas = GetComponentInParent<Canvas>();
        canvasRectTransform = canvas.GetComponent<RectTransform>();

        //if no camera is specified, default to the main camera
        if (camera1 == null)
        {
            camera1 = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (NPC_TopBone == null) return;
        if (NPC_BottomBone == null) return;

        //we need to compute the position of the top and bottom caption tails so that
        //they appear to emerge from the top and bottom panels.  This is a multi-
        //step process.

        //1) compute the normalized canvas coordinates of the top and bottom caption follower bones
        Vector2 npc_top_bone_pos = camera1.WorldToViewportPoint(NPC_TopBone.position);
        Vector2 npc_bottom_bone_pos = camera1.WorldToViewportPoint(NPC_BottomBone.position);

        //2) perform a component-wise multiplication with the canvas size vector to
        //compute the pixel positions for the top and bottom caption tails
        Vector2 TopPos = canvasRectTransform.rect.size * npc_top_bone_pos;
        Vector2 BottomPos = canvasRectTransform.rect.size * npc_bottom_bone_pos;

        //3) set the start position (on the NPC) of the top and bottom caption tails.
        TopTail.position = TopPos + Vector2.up;//scoot up 1 pixel (not sure why this is needed, but without it there is a gap between the UICaption panel and the tail.  May be a rounding error in the math.)
        BottomTail.position = BottomPos - Vector2.up;//scoot down 1 pixel (not sure why this is needed, but without it there is a gap between the UICaption panel and the tail.  May be a rounding error in the math.)

        //4) scale the caption tails so that they exactly fill the space between the
        //NPC and the top and bottom UICaption panels.
        float TopHeight = canvasRectTransform.rect.height - TopPanel.rect.height - TopPos.y;
        TopTail.sizeDelta = new Vector2(TopTail.sizeDelta.x, TopHeight ); //only chage y

        float BottomHeight = BottomPos.y - BottomPanel.rect.height;
        BottomTail.sizeDelta = new Vector2(BottomTail.sizeDelta.x, BottomHeight); //only change y

        //5) enable/disable panels as necessary
        if (panelLocation == position.AUTO)
        {
            //switch to top panel
            if ( (npc_top_bone_pos.y + npc_bottom_bone_pos.y) /2  > autoPlacementUpperTriggerPoint)
            {
                TopPanel.gameObject.SetActive(true);
                TopTail.gameObject.SetActive(true);
                BottomPanel.gameObject.SetActive(false);
                BottomTail.gameObject.SetActive(false);
            }

            //swith to bottom panel
            if ((npc_top_bone_pos.y + npc_bottom_bone_pos.y) / 2 < autoPlacementLowerTriggerPoint)
            {
                TopPanel.gameObject.SetActive(false);
                TopTail.gameObject.SetActive(false);
                BottomPanel.gameObject.SetActive(true);
                BottomTail.gameObject.SetActive(true);
            }
        }
    }

    /**
     * ChatGPT assisted code to search for certain bones in a humanoid rig given
     * the root bone and the "endswith" suffix of the bone name.  For example, to
     * find the neck bone, we supply the suffix "neck" because, while different 
     * riggers use different name prefixes, the suffix tends to be more standardized.
     * For this example, 3 common riggers label the neck bone "mixamorig:Neck",
     * "Neck" and "[CharacterName]Neck".  The suffix comparison is case insenstive.
     * If the search fails, the function returns null.  Otherwise, the transform
     * of the matched bone is returned.
     **/
    Transform FindBoneWithNameSuffix(Transform rootBone, string nameSuffix)
    {
        if (rootBone == null) return null;

        //get all bones: these are all the children of the root bone
        Transform[] allBones = rootBone.GetComponentsInChildren<Transform>(); // Get all bones

        //search each bone for a name match
        foreach (Transform bone in allBones)
        {
            if (bone.name.ToUpper().EndsWith(nameSuffix))
            {
                Debug.Log("Found bone: " + bone.name);
                return bone; // Found a bone with a matching suffix
            }
        }
        return null;
    }

}
