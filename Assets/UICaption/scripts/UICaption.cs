using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * You may instantiate one UICaption prefab on your camvas for each NPC or
 * you may instantiate a single UICaption object shared by all NPCs.  In the
 * latter case, simply switch the NPC_RootBone  for the different characters.
 * There is significant overhead in doing so, however as the search for the 
 * top and bottom caption tail follower bones ("headtop_end" and "neck") is
 * repeated everytime we switch NPCs. (the cost can be reduced by specifying the
 * rig neck bone as the root bone, greatly reducing the search).  Altenratively,
 * use the SetTopCaptionTailBone() and SetBottomCaptionTailBone() functions to 
 * specify the top and bottom cpation tail follower bones.  This option 
 * eliminates the search entirely.
 * A "Character" class can be created that manages the switching of captions 
 * between characters.  Such a class could permit differnt characters to use 
 * different color dialogs, different fonts, different tail follower bones, etc.
 * **/


/** class that implements NPC dialogs on screen.  Attributes of the dialog
 * including color, font, position, etc. are updated when the Text field is
 * set.
 **/
[RequireComponent(typeof(CaptionTail))]
public class UICaption : MonoBehaviour
{
    [Header("Appearance")]
    public Color backgroundColor;
    public Color textColor;
    public float fontSize = 20f;
  
    //[Tooltip("Height of the window in terms of % of viewport height.")]
    //[Range(0f, 1f)]
    //public float height;

    public bool hasTail = true;

    [Header("NPC")]
    [Tooltip("The Transform of the root bone of the NPC we will follow.  " +
        "This is often called \"Hips\" or \"Armature\".  " +
        "This field is required only for captions with tails (hasTail = true)." +
        "Also, instead of specifying the root bone, you may specify the top and" +
        "bottom caption follower bones using the TopCaptionBone and BottomCaption" +
        "caption bone properties.")]
    public Transform NPC_RootBone;  //the xform of the root bone of the NPC we will follow

    /**
     * TopCaptionBone and BottomCaptionBone properties:
     * Rather than specifying the NPC_RootBone, we can specify the TopCaptionBone
     * and the BottomCaptionBone with these properties.  Specifying the root
     * bone will trigger a search for the top and bottom caption tail follower bones
     * using CaptionTail's FindBoneWithNameSuffix() function.
     * **/
    public Transform TopCaptionBone
    {
        get { return captionTail.TopCaptionBone; }
        set { captionTail.TopCaptionBone = value; }
    }

    public Transform BottomCaptionBone
    {
        get { return captionTail.BottomCaptionBone; }
        set { captionTail.BottomCaptionBone = value; }
    }


    [Tooltip("Specify the location of the dialog panel.  AUTO lets the " +
    "system decide based on the position of the NPC on the screen." +
    "  AUTO mode applies only to captions with tails (hasTail = true)")]
    public CaptionTail.position PanelLocation = CaptionTail.position.AUTO;

    [Tooltip("The point on the screen where we switch from bottom to top " +
    "panel when in AUTO mode.  50% is the middle of the screen.  0% is the " +
    "bottom and 100% is the top.")]
    [Range(0f, 1f)]
    public float autoPlacementUpperTriggerPoint = 0.51f;  //The point on the screen
                                                         //where we switch from bottom to top panel placement in AUTO mode
                                                         //50% is the middle of the screen.  0% is the bottom. 100% is the top.

    [Tooltip("The point on the screen where we switch from top to bottom " +
    "panel when in AUTO mode.  50% is the middle of the screen.  0% is the " +
    "bottom and 100% is the top.")]
    [Range(0f, 1f)]
    public float autoPlacementLowerTriggerPoint = 0.49f;  //The point on the screen
                                                         //where we switch from to to bottom panel placement in AUTO mode
                                                         //50% is the middle of the screen.  0% is the bottom. 100% is the top.

    //setting the Text property will trigger all updates to the dialog box
    //(color, font, etc.)
    public string Text
    {
        get { return text; }
        set
        {
            text = value;
            StartCoroutine(UpdateDialog());
        }
    }
    
    string text = "";   //backing field for Text property


    //[Header("Panels")]
    CaptionTail captionTail;

    GameObject TopPanel;
    GameObject BottomPanel;
    GameObject TopTail;
    GameObject BottomTail;

    TMP_Text TopPanelText;
    TMP_Text BottomPanelText;



    // Start is called before the first frame update
    void Start()
    {
        //get a reference to the required CaptionTail component
        captionTail = GetComponent<CaptionTail>();

        //get a reference to the top and bottom panel GOs
        TopPanel = captionTail.TopPanel.gameObject;
        BottomPanel = captionTail.BottomPanel.gameObject;

        //get a reference to the top and bottom caption tail GOs
        TopTail = captionTail.TopTail.gameObject;
        BottomTail = captionTail.BottomTail.gameObject;

        //get references to the panel TMP_Text components
        TopPanelText = TopPanel.GetComponentInChildren<TMP_Text>();
        BottomPanelText = BottomPanel.GetComponentInChildren<TMP_Text>();

        Text = "";
        //Text = "The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.";
    }


    public void SetText(string Txt, float duration = -1, System.Action<string> callback = null)
    {
        //duration = 0 --> return right away
        //duration < 0 --> infinity

        text = Txt;
        StartCoroutine(UpdateDialog(duration, callback));
    }

    IEnumerator UpdateDialog(float duration = -1, System.Action<string> callback = null)
    {
        //set the panel background colors
        TopPanel.GetComponent<Image>().color = backgroundColor;
        BottomPanel.GetComponent<Image>().color = backgroundColor;

        //set the text color
        TopPanelText.color = textColor;
        BottomPanelText.color = textColor;

        //set the font size
        TopPanelText.fontSize = fontSize;
        BottomPanelText.fontSize = fontSize;

        //set the panel texts

        float boundsYSize;

        if (text == "")
        {
            //if there is no text, set the y size to 0
            boundsYSize = 0;
        }
        else
        {
            //we want to add a line before and after the text as padding in the
            //panel.  Unfortunately, simply adding a "\n" before and after the
            //text [as in TopPanelText.text = "\n" + text + "\n" ] doesn't work
            //because the TMP object strips the newline when computing the bounds.
            //Instead, we will create a modified string for computing the bounds,
            //then assign the desired string in the next frame.
            //(TMP = TextMeshPro)

            //Create a string with a line above and below the provided text.
            //The \n chars in this string won't be stripped off.
            TopPanelText.text = ".\n" + text + "\n.";

            //now, pause until the next frame to allow the TopPanelText TMP to update
            yield return null;  //pause until the next frame

            //we can now compute the size of the updated TMP
            boundsYSize = TopPanelText.textBounds.size.y; //computer the y size
        }

        //TOP Panel
        //set the panel text & height
        TopPanelText.text = text; //set the top panel text
        RectTransform TopRT = TopPanel.GetComponent<RectTransform>();
        TopRT.sizeDelta = new Vector2(TopRT.sizeDelta.x, boundsYSize);

        //BOTTOM Panel
        //set the panel text & height
        BottomPanelText.text = text;    //set the bottom panel text
        RectTransform BottomRT = BottomPanel.GetComponent<RectTransform>();
        BottomRT.sizeDelta = new Vector2(BottomRT.sizeDelta.x, boundsYSize);


        //add the tails, if requested and necessary
        if (hasTail && text != "")
        {
            //in this case, we will deletage the enabling/disabling of
            //the caption panels to the CaptionTail object.

            //enable the tail
            captionTail.enabled = true;
            TopTail.SetActive(true);
            BottomTail.SetActive(true);

            //set the caption tail background colors
            TopTail.GetComponent<Image>().color = backgroundColor;
            BottomTail.GetComponent<Image>().color = backgroundColor;

            //specify the follower panel location
            captionTail.PanelLocation = PanelLocation;
            captionTail.autoPlacementUpperTriggerPoint = autoPlacementUpperTriggerPoint;
            captionTail.autoPlacementLowerTriggerPoint = autoPlacementLowerTriggerPoint;

            //specify the NPC root bone
            captionTail.NPC_RootBone = NPC_RootBone;
        }
        else
        {
            //disable the tails
            captionTail.enabled = false;
            TopTail.SetActive(false);
            BottomTail.SetActive(false);

            //here, since we are not using the tail controller, we must
            //manually manage the enabling/disabling of the caption panels
            switch (PanelLocation)
            {
                case CaptionTail.position.TOP:
                    TopPanel.SetActive(true);
                    BottomPanel.SetActive(false);
                    break;
                case CaptionTail.position.BOTTOM:
                    TopPanel.SetActive(false);
                    BottomPanel.SetActive(true);
                    break;
                case CaptionTail.position.BOTH:
                    TopPanel.SetActive(true);
                    BottomPanel.SetActive(true);
                    break;
                default:
                    Debug.Log(" Caption position of AUTO not a valid option for a tailless caption.  Defaulting to TOP placement.");
                    TopPanel.SetActive(true);
                    BottomPanel.SetActive(false);
                    break;
            }

        }

        if (duration < 0)
        {
            //infinite duration (no callback)
            yield break;
        }
        else if (duration > 0)
        {
            yield return new WaitForSeconds(duration);

        }
        callback?.Invoke(text);

        SetText("");
    }

}
