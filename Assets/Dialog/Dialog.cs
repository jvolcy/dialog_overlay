using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public enum position { TOP, BOTTOM, AUTO };

    //setting the Text property will trigger all updates to the dialog box
    //(color, font, etc.)
    public string Text
    {
        get { return bottom ? BottomPanelText.text : TopPanelText.text; }
        set { UpdateDialog(value); }
    }

    public Color backgroundColor;
    public Color textColor;
    public bool bottom = false;

    public GameObject TopPanel;
    public GameObject BottomPanel;
    public position PanelLocation = position.AUTO;

    TMP_Text TopPanelText;
    TMP_Text BottomPanelText;

    // Start is called before the first frame update
    void Start()
    {
        TopPanelText = TopPanel.GetComponentInChildren<TMP_Text>();
        BottomPanelText = BottomPanel.GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDialog(string text)
    { }
}
