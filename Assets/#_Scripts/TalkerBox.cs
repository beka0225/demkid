using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TalkerBox : MonoBehaviour
{

    public Image BackImageObject;
    public Text NameText;
    public Text TextField;

    private string getText;
    private List<string> getTextList = new List<string>();
    private int readerCount = 0;

    private TalkManager talkManger;

    private void ClearSetting()
    {
        getText = null;
        getTextList.Clear();
        readerCount = 0;
    }

    // Use this for initialization
    void Start()
    {
        talkManger = GameObject.Find("Canvas").GetComponent<TalkManager>();
        if(null == talkManger)
        {
            Debug.LogError("Talk Manager Canvas is NULL !! ");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TextFieldInit(TextAsset text)
    {
        ClearSetting();

        StringReader reader = new StringReader(text.text);
        string readLine = "";

        do
        {
            readLine = reader.ReadLine();
            getTextList.Add(readLine);
        } while (null != readLine);

        reader.Close();
        getTextList.RemoveAt(getTextList.Count - 1); //Remove Last Member

        if(getTextList.Count > 0)
        {
            TextField.text = getTextList[readerCount];
            readerCount++;
        }
        Debug.Log("ReadText Size : " + getTextList.Count.ToString());
    }

    public void OnNextTextUpdate()
    {
        if (getTextList.Count > readerCount)
        {
            TextField.text = getTextList[readerCount];
            readerCount++;
        }
        else
        {
            talkManger.OnTalkEnd();
        }
    }
}
