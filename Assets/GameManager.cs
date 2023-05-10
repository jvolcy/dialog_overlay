using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UICaption uiCaption;
    public Transform MichelleNeckBone;
    public Transform AmyNeckBone;
    public Transform SpelmaniteNeckBone;

    UIDialog uIDialog;

    // Start is called before the first frame update
    void Start()
    {
        uIDialog = GetComponent<UIDialog>();

        uIDialog.NewDialog();
        uIDialog.AddLine("michelle", "Hello, Amy.  Nice whether we're having.\nDon't you think?", 2);
        uIDialog.AddLine("caption", "[Top] Meanwhile, at the Hall of Justice...", 3);
        uIDialog.AddLine("Amy", "Yes indeed!  The sky is blue, the birds are singing.\nIt doesn't get much better than this.", 5);
        uIDialog.AddLine("caption2", "[Bottom] Meanwhile, at the Hall of Justice...", 3);
        uIDialog.AddLine("Spelmanite", "Hey there.  What are you 2 talking about?", 2);

    }

    void Michelle(string txt, float duration)
    {
        uiCaption.NPC_RootBone = MichelleNeckBone;
        uiCaption.SetText(txt, duration);
    }

    void Amy(string txt, float duration)
    {
        uiCaption.NPC_RootBone = AmyNeckBone;
        uiCaption.SetText(txt, duration);
    }

    void Spelmanite(string txt, float duration)
    {
        uiCaption.NPC_RootBone = SpelmaniteNeckBone;
        uiCaption.SetText(txt, duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // uiCaption.SetText("The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog. " + i.ToString(),
            //     3);
            // i++;
            //Michelle("Hello, Amy.  Nice whether we're having.\nDon't you think?", 5);
            //Amy("Yes indeed!  The sky is blue, the birds are singing.\nIt doesn't get much better than this.", 5);
            //Spelmanite("Hey there.  What are you 2 talking about?", 5);

            uIDialog.RunDialog();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            uiCaption.Text = "";
        }
    }

}
