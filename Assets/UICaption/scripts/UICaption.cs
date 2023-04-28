using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    int fontSize = 20;
  
    //[Tooltip("Height of the window in terms of % of viewport height.")]
    //[Range(0f, 1f)]
    //public float height;

    public bool hasTail = true;

    [Header("NPC")]
    [Tooltip("The Transform of the root bone of the NPC we will follow.  " +
        "This is often called \"Hips\" or \"Armature\".  " +
        "This field is required only for captions with tails (hasTail = true)")]
    public Transform NPC_RootBone;  //the xform of the root bone of the NPC we will follow

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
            UpdateDialog();
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
        //get a reference to the required NPCFollower component
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

        Text = "Hello World!!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void UpdateDialog()
    {
        //set the panel texts
        TopPanelText.text = text;
        BottomPanelText.text = text;

        //set the panel background colors
        TopPanel.GetComponent<Image>().color = backgroundColor;
        BottomPanel.GetComponent<Image>().color = backgroundColor;

        //set the text color
        TopPanelText.color = textColor;
        BottomPanelText.color = textColor;

        //set the font size
        TopPanelText.fontSize = fontSize;
        BottomPanelText.fontSize = fontSize;

        TopPanelText.maxVisibleLines = 5;

        if (hasTail)
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
                default:
                    Debug.Log(" Caption position of AUTO not a valid option for a tailless caption.  Defaulting to TOP placement.");
                    TopPanel.SetActive(true);
                    BottomPanel.SetActive(false);
                    break;
            }

        }
    }
}
