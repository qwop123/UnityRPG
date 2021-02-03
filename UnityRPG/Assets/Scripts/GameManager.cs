using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public TalkManager talkManager;
    public QuestManager questManager;
    public Animator talkPanel;
    public Animator portraitAnim;
    public Image portraitImg;
    public Sprite prevPortrait;

    public TypeEffect talk;
    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;
    

    void Start()
    {
        Debug.Log(questManager.CheckQuest());
    }
    


    public void Action(GameObject scanObj)
    {



        
        scanObject = scanObj;
            ObjData objData = scanObject.GetComponent<ObjData>();          
            Talk(objData.id, objData.isNpc);


              talkPanel.SetBool("isShow", isAction); 

    }

    void Talk(int id, bool isNpc)
    {
        // Set Talk Data
        int questTalkIndex = 0;
        string talkData = "";

        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        } 

        else
        {

             questTalkIndex = questManager.GetQuestTalkIndex(id);
             talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        // End Talk
        if(talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            Debug.Log(questManager.CheckQuest(id));
            return;
        }


        // Continue Talk
        if (isNpc)
        {
            talk.SetMsg(talkData.Split(':')[0]);


            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse (talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);

            // Animation Portrait

            if (prevPortrait != portraitImg.sprite )
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }

              
            
}
        else
        {
            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);

        }

        isAction = true;
        talkIndex++;
    }
}
