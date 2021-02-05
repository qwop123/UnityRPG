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
    public Text questText;

    public Text UINameText;


    public GameObject menuSet;

    public GameObject scanObject;
    public GameObject player;

    public bool isAction;
    public int talkIndex;

    public string name;


    void Start()
    {
        GameLoad();
        questText.text = questManager.CheckQuest();
        
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            SubMenuActive();
        }
      
    }

    public void SubMenuActive()
    {
        //Sub Menu
        if (menuSet.activeSelf)
            menuSet.SetActive(false);
        else
            menuSet.SetActive(true);
    }
    


    public void Action(GameObject scanObj)
    {

      


    scanObject = scanObj;

        name = scanObject.name;

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
            questText.text = questManager.CheckQuest(id);
            return;
        }


        // Continue Talk
        if (isNpc)
        {
            
            talk.SetMsg(talkData.Split(':')[0]);

            UINameText.text = name;


            // Show Portrait
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse (talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            // Animation Portrait

            if (prevPortrait != portraitImg.sprite )
            {
                portraitAnim.SetTrigger("doEffect");
                prevPortrait = portraitImg.sprite;
            }

              
            
}
        else
        {
            UINameText.text = null;


            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);

        }

       
            
          
            
         

        isAction = true;
        talkIndex++;
    }

    public void GameSave()
    {
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        PlayerPrefs.Save();

        menuSet.SetActive(false);


        // player.x  
        // player.y
        // Quest Id
        // Quest Action Index

    }

    public void GameLoad()
    {

        if (!PlayerPrefs.HasKey("PlayerX"))
            return;

        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        questManager.ControlObject();

    }


    public void GameExit()
    {
        Application.Quit();
    }


}
