using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public UICaption uiCaption;

    int i = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            uiCaption.SetText("The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog.  The quick brown fox jumps over the lazy dog. " + i.ToString(),
                3);
            i++;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            uiCaption.Text = "";
        }
    }

}
