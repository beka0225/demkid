using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using UnityEngine.EventSystems;


public class TalkManager : MonoBehaviour
    , IPointerClickHandler
    , IPointerDownHandler
    , IPointerEnterHandler
    , IPointerExitHandler
    , IPointerUpHandler
    , IDragHandler{
    

    public GameObject PlayerObject;
    public Button TalkButtonObject;
    public GameObject TalkerBoxObject;

    [Header("Talker Target")]
    public GameObject[] TargetList;

    [Header("Talk Script")]
    public TextAsset[] TalkScriptList;

    [Space(5)]
    public float TalkDistance = 1.0f;

    private int talkerIndex = 0;
    private bool isTalkerBoxOn = false;
    private bool isInTalkerRange = false;

    private string playerObjectName = "";
    private bool isGetPlayer = false;
    

    // Use this for initialization
    void Start () {
        GameObject levelManagerObj = GameObject.Find("LevelManager");
        if (null != levelManagerObj)
        {
            MoreMountains.CorgiEngine.LevelManager levelMgr = levelManagerObj.GetComponent<MoreMountains.CorgiEngine.LevelManager>();
            if(levelMgr.PlayerPrefabs.Length > 0)
            {
                playerObjectName = levelMgr.PlayerPrefabs[0].gameObject.name.ToString();
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (false == isGetPlayer)
        {
            PlayerObject = GameObject.Find(playerObjectName);
            if (null != PlayerObject)
                isGetPlayer = true;
        }

        //Debug.Log("Player Pos : " + PlayerObject.transform.position.ToString());
        if (false == isInTalkerRange) 
        {
            for (int i = 0; i < TargetList.Length; ++i)
            {
                //Debug.Log(Vector2.Distance(PlayerObject.transform.position, TargetList[i].transform.position));
                if (Vector2.Distance(PlayerObject.transform.position, TargetList[i].transform.position) < TalkDistance)
                {
                    Debug.Log("In Talker Range : " + i.ToString());
                    if (null != TalkButtonObject)
                    {
                        Debug.Log("On button Active");
                        TalkButtonObject.gameObject.SetActive(true);

                        talkerIndex = i;
                        isInTalkerRange = true;
                    }
                }
            }
        }
        else if (true == isInTalkerRange) 
        {
            //too far from active talker
            if (Vector2.Distance(PlayerObject.transform.position, TargetList[talkerIndex].transform.position) > TalkDistance)
            {
                if (null != TalkButtonObject)
                {
                    TalkButtonObject.gameObject.SetActive(false);
                    talkerIndex = 0;

                    isInTalkerRange = false;
                }
            }
        }

    }

    private void ClearSetting()
    {
        talkerIndex = 0;
        isTalkerBoxOn = false;
        isInTalkerRange = false;
    }

    public void OnTalkButtonClick()
    {
        if(false == isTalkerBoxOn)
        {
            Debug.Log("Talk Button Called");

            isTalkerBoxOn = true;
            TalkerBoxObject.SetActive(true);
            TalkerBoxObject.GetComponent<TalkerBox>().TextFieldInit(TalkScriptList[talkerIndex]);

            //disable input manager by talk start
            GameObject.Find("InputManager").GetComponent<MoreMountains.CorgiEngine.InputManager>().enabled = false;

            TalkButtonObject.gameObject.SetActive(false);
        }
    }

    public void OnTalkEnd()
    {
        isTalkerBoxOn = false;
        TalkerBoxObject.SetActive(false);

        //enable input manager by talk start
        GameObject.Find("InputManager").GetComponent<MoreMountains.CorgiEngine.InputManager>().enabled = true;

        TalkButtonObject.gameObject.SetActive(true);
        
    }

    public void OnPointerClick(PointerEventData pointerEvent)
    {
        if (true == isTalkerBoxOn)
        {
            TalkerBoxObject.GetComponent<TalkerBox>().OnNextTextUpdate();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ((IPointerDownHandler)TalkButtonObject).OnPointerDown(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)TalkButtonObject).OnPointerEnter(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ((IPointerExitHandler)TalkButtonObject).OnPointerExit(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)TalkButtonObject).OnPointerUp(eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
