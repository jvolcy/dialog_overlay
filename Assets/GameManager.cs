using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UICaption uiCaption;

    UIDialog uIDialog;

    //dialog completion callback
    void dialogDone(string s)
    {
        Debug.Log("Callback: " + s);
    }


    //======================= SIMPLE CAPTION EXAMPLE =======================
    // /*

    // Start is called before the first frame update
    void Start()
    {
        uIDialog = GetComponent<UIDialog>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //michelle talks for 4 seconds, then callback executes
            uIDialog.Speak("michelle", "Hello, Amy.  Nice whether we're having.\nDon't you think?", 4, dialogDone);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Amy talks indefinitely.  No callback.  Use "X" or select a different # to erase/replace the caption.
            uIDialog.Speak("Amy", "Yes indeed!  The sky is blue, the birds are singing.\nIt doesn't get much better than this.");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            //Spelmanite talks for 3 seconds, no callback.
            uIDialog.Speak("Spelmanite", "Hey there.  What are you 2 talking about?", 3);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            //caption displayed for 3 seconds, no callback.
            uIDialog.Speak("caption2", "Meanwhile, at the Hall of Justice...", 3);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            //clear any dialog
            uIDialog.Speak();
        }
    }
    // */

    //======================= AUTONOMOUS DIALOG EXAMPLE =======================
    /*

    // Start is called before the first frame update
    void Start()
    {
        uIDialog = GetComponent<UIDialog>();

        uIDialog.NewDialog();   //create a new dialog

        //construct the dialog
        uIDialog.AddLine("michelle", "Hello, Amy.  Nice whether we're having.\nDon't you think?", 3);
        uIDialog.AddLine("caption", "[Top] Meanwhile, at the Hall of Justice...", 2);
        uIDialog.AddLine("Amy", "Yes indeed!  The sky is blue, the birds are singing.\nIt doesn't get much better than this.", 3);
        uIDialog.AddLine("caption2", "[Bottom] Meanwhile, at the Hall of Justice...", 2);
        uIDialog.AddLine("Spelmanite", "Hey there.  What are you 2 talking about?", 2);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //execute the dialog
            uIDialog.RunDialog(dialogDone);
        }
    }
    */




}
