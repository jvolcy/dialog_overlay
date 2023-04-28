using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/** class that implements NPC dialogs on screen.  Attributes of the dialog
 * including color, font, position, etc. are updated when the Text field is
 * set.
 **/
//[RequireComponent(typeof(NPCFollower))]
public class Dialog : MonoBehaviour
{
    [Header("Main")]
    [Tooltip("The Transform of the root bone of the NPC we will follow.  " +
        "This is often called \"Hips\" or \"Armature\".")]
    public Transform NPC_RootBone;  //the xform of the root bone of the NPC we will follow

    [Tooltip("Specify the location of the dialog panel.  AUTO lets the " +
"system decide based on the position of the NPC on the screen.")]
    public NPCFollower.position PanelLocation = NPCFollower.position.AUTO;

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

    [Header("Appearance")]
    public Color backgroundColor;
    public Color textColor;

    //[Header("Panels")]
    NPCFollower npcFollower;

    GameObject TopPanel;
    GameObject BottomPanel;
    GameObject TopPointer;
    GameObject BottomPointer;

    TMP_Text TopPanelText;
    TMP_Text BottomPanelText;


    // Start is called before the first frame update
    void Start()
    {
        //get a reference to the required NPCFollower component
        npcFollower = GetComponent<NPCFollower>();

        //get a reference to the top and bottom panel GOs
        TopPanel = npcFollower.TopPanel.gameObject;
        BottomPanel = npcFollower.BottomPanel.gameObject;

        //get a reference to the top and bottom pointer GOs
        TopPointer = npcFollower.TopPointer.gameObject;
        BottomPointer = npcFollower.BottomPointer.gameObject;

        //get references to the panel TMP_Text components
        TopPanelText = TopPanel.GetComponentInChildren<TMP_Text>();
        BottomPanelText = BottomPanel.GetComponentInChildren<TMP_Text>();

        Text = "Hello World!";
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

        //set the pointer background colors
        TopPointer.GetComponent<Image>().color = backgroundColor;
        BottomPointer.GetComponent<Image>().color = backgroundColor;

        //set the text color
        TopPanelText.color = textColor;
        BottomPanelText.color = textColor;

        //specify the follower panel location
        npcFollower.PanelLocation = PanelLocation;
        npcFollower.autoPlacementUpperTriggerPoint = autoPlacementUpperTriggerPoint;
        npcFollower.autoPlacementLowerTriggerPoint = autoPlacementLowerTriggerPoint;

        //specify the NPC root bone
        npcFollower.NPC_RootBone = NPC_RootBone;
    }
}
