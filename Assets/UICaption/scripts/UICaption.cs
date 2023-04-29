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
    public int fontSize = 20;
  
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

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator UpdateDialog()
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
    }


}
