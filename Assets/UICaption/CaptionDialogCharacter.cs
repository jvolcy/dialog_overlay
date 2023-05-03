using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class DialogCharacter
{
    public string Name = "name";
    public Transform TopCaptionBone = null;
    public Transform BottomCaptionBone = null;
    public Color CaptionBackground = new Color(255, 255, 255, 100);
    public float FontSize = 30f;
    public Color TextColor = Color.yellow;
    public bool HasTail = true;
    public CaptionTail.position CaptionPosition = CaptionTail.position.TOP;
}

public class CaptionDialogCharacter : MonoBehaviour
{
    public UICaption uICaption;
    public Dictionary<string, DialogCharacter> Character = new Dictionary<string, DialogCharacter>();

    //Dialog is a dictionary of character names and text/duration tuples
    public Dictionary<string, (string, float)> Dialog = new Dictionary<string, (string, float)>();


    // Start is called before the first frame update
    void Start()
    {
        //create a default "None" character with no caption tail
        AddCharacter("None");
        Character["None"].HasTail = false;
        Character["None"].CaptionPosition = CaptionTail.position.TOP;
    }

    public void Speak(string name, string captionText)
    {
        if (SwitchNPC(name))
        {
            uICaption.Text = captionText;
        }
    }

    public void AddCharacter(string name)
    {
        if (!Character.ContainsKey(name))
        {
            Character.Add(name, new DialogCharacter());
        }
        else
        {
            Debug.Log("'" + name + "' already exists in the list of characters.");
        }
    }

    bool SwitchNPC(string name)
    {
        //if the provided name is not in the dictionary, simply return
        if (!Character.ContainsKey(name))
        {
            Debug.Log("Could not find character '" + name + "'.");
            return false;
        }

        uICaption.SetCaptionTailBones(Character[name].TopCaptionBone, Character[name].BottomCaptionBone);
        uICaption.backgroundColor = Character[name].CaptionBackground;
        uICaption.fontSize = Character[name].FontSize;
        uICaption.textColor = Character[name].TextColor;
        uICaption.hasTail = Character[name].HasTail;
        uICaption.PanelLocation = Character[name].CaptionPosition;

        return true;
    }
}
