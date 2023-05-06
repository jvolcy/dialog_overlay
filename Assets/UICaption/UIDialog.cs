using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialog : MonoBehaviour
{
    //Dictionary of DialogCharacter objects.
    //Each DialogCharacter self registers into the dictionary in their Start()
    //functions and unregisters in their OnDestroy() functions.
    //The current class uses the dictionary, but does not manage it.
    static public Dictionary<string, DialogCharacter> CharacterDictionary = new Dictionary<string, DialogCharacter>();

    public UICaption uICaption;

    [SerializeField]
    public Tuple<string, string, float> dialogLine;

    //Dialog is a dictionary of character names and text/duration tuples
    List<(string CharacterName, string Text, float duration)> Dialog;


    string CurrentSpeaker = "-not assigned-";   //initialize with a random string

    int dialogPointer;  //our place in the dialog
    System.Action<string> dialogCompleteCallback;
    string callbackIDString;       //user-supplied ID string used with dialogCompletionCallback


    public void NewDialog()
    {
        Dialog = new();
    }

    public void AddLine(string CharacterName, string Text, float duration)
    {
        Dialog.Add((CharacterName, Text, duration));
    }

    /**
     * RunDialog()
     * This function will execute the dialog line by line.  Optionally, provide a callback function
     * and a callback string.  The callback function prototype is void FN(string).  The string passed
     * to the function is the optionally provided IDstring.
     * **/
    public void RunDialog(System.Action<string> callback = null, string IDstring = "Dialog Complete")
    {
        dialogCompleteCallback = callback;
        callbackIDString = IDstring;
        dialogPointer = 0;  //point to the first line of the dialog
        executeLine(null);
    }


    void executeLine(string s)
    {
        Debug.Log("ExecuteLine..." + dialogPointer);

        if (dialogPointer == Dialog.Count)  //end of dialog
        {
            dialogCompleteCallback?.Invoke(callbackIDString);
            Debug.Log("Dialog Done!");
            return;
        }

        Speak(Dialog[dialogPointer].CharacterName, Dialog[dialogPointer].Text, Dialog[dialogPointer].duration, executeLine);
        dialogPointer++;
    }

    public void Speak(string CharacterName, string captionText, float duration=-1, System.Action<string> callback=null)
    {
        Debug.Log("Speak: " + CharacterName + ":" + captionText);
        if (SwitchNPC(CharacterName))   //verify this is a valid character and switch to its properties if it is
        {
            uICaption.SetText(captionText, duration, callback);
        }
    }


    bool SwitchNPC(string CharacterName)
    {
        if (CharacterName == CurrentSpeaker) return true; //nothing to do

        //if the provided name is not in the dictionary, simply return
        if (!CharacterDictionary.ContainsKey(CharacterName))
        {
            Debug.Log("Could not find character '" + CharacterName + "'.");
            return false;
        }

        uICaption.TopCaptionBone = CharacterDictionary[CharacterName].TopCaptionBone;
        uICaption.BottomCaptionBone = CharacterDictionary[CharacterName].BottomCaptionBone;
        uICaption.backgroundColor = CharacterDictionary[CharacterName].CaptionBackground;
        uICaption.fontSize = CharacterDictionary[CharacterName].FontSize;
        uICaption.textColor = CharacterDictionary[CharacterName].TextColor;
        uICaption.hasTail = CharacterDictionary[CharacterName].HasTail;
        uICaption.PanelLocation = CharacterDictionary[CharacterName].CaptionPosition;

        CurrentSpeaker = CharacterName;

        return true;
    }
}



/**
 * HOW TO USE
 * 
 * 1) Get a reference to the CaptionDialogCharacter component
 *      captionDialogChar = GetComponent<>(CaptionDialogCharacter);
 * 2) Create a caption character.  In this example, our character will be
 * named "Laura".  Character names must be unique.  These are the keys to
 * an ibnternally maintained character dictionary.
 * 
 *      captionDialogChar.AddCharacter("Laura")
 *      
 * 3) Customize the caption properties for this character.
 * 
 * **/

/*
public class CaptionDialogCharacter : MonoBehaviour
{
    public UICaption uICaption;
    //public Dictionary<string, DialogCharacter> Character = new Dictionary<string, DialogCharacter>();

    //Dialog is a dictionary of character names and text/duration tuples
    public DialogCharactersDictionary dialogCharacters;
    //public Dictionary<string, (string, float)> Dialog = new Dictionary<string, (string, float)>();




}
*/