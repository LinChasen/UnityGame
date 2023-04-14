using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData
{
    private GameObject _baseObj;
    private List<GameObject> _starShineList=new List<GameObject>();
    private List<GameObject> _starOutList=new List<GameObject>();
    private int _levelNum=1;

    public LevelData(GameObject inBaseObj,int inLevelNum)
    {
        _baseObj=inBaseObj;
        _levelNum=inLevelNum;
        //生成关卡数字
        GameObject numberObj=_baseObj.transform.Find("number").gameObject;
        string numStr=inLevelNum.ToString();
        if(numStr.Length==1)//<10关
        {
            GameObject numPref=Resources.Load<GameObject>("Prefabs/Number/"+numStr);
            GameObject numObj=GameObject.Instantiate(numPref,numberObj.transform);
            numObj.transform.localPosition=new Vector3(0,0.26f,0);
        }
        else{
            GameObject numPref1=Resources.Load<GameObject>("Prefabs/Number/"+numStr[0]);
            GameObject numObj1=GameObject.Instantiate(numPref1,numberObj.transform);
            numObj1.transform.localPosition=new Vector3(-0.1f,0.26f,0);
            GameObject numPref2=Resources.Load<GameObject>("Prefabs/Number/"+numStr[1]);
            GameObject numObj2=GameObject.Instantiate(numPref2,numberObj.transform);
            numObj2.transform.localPosition=new Vector3(0.1f,0.26f,0);
        }
        //星星
        GameObject starPr=_baseObj.transform.Find("star").gameObject;
        for(int i=0;i<3;i++)
        {
            GameObject shineStar=starPr.transform.Find("star0"+(i+1).ToString()).gameObject;
            GameObject outStar=starPr.transform.Find("star0"+(i+1).ToString()+"_G").gameObject;
            _starShineList.Add(shineStar);
            _starOutList.Add(outStar);
        }
        //设置星星数量
        SetStarNum(AllGlobal.storage.GetLevelStarNum(_levelNum));
    }

    public GameObject baseObj{
        get{
            return _baseObj;
        }
    }

    private void SetStarNum(int starNum)
    {
        for(int i=0;i<3;i++)
        {
            bool isShine=i<starNum;
            _starShineList[i].SetActive(isShine);
            _starOutList[i].SetActive(!isShine);
            if(AllGlobal.storage.level<=_levelNum){_starOutList[i].SetActive(false);}
        }
    }
}
