using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DialogCharacter : MonoBehaviour
{
    [SerializeField] string CharacterName = "";
    public Transform TopCaptionBone = null;
    public Transform BottomCaptionBone = null;
    public Color CaptionBackground = new Color(1f, 1f, 1f, 0.4f);
    public float FontSize = 30f;
    public Color TextColor = Color.yellow;
    public bool HasTail = true;
    public CaptionTail.position CaptionPosition = CaptionTail.position.TOP;

    // Start is called before the first frame update
    void Start()
    {
        if (CharacterName == "")
        {
            Debug.Log("No character name provided; defaulting to " + name + ".  ");
            CharacterName = name;
        }

        //Debug.Log("Registering " + CharacterName + "...");
        UIDialog.CharacterDictionary.Add(CharacterName, this);
    }


    private void OnDestroy()
    {
        UIDialog.CharacterDictionary.Remove(CharacterName);
        //Debug.Log("Unregistered " + CharacterName + ".");
    }
}
