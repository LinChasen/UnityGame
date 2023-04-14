using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBase
{
    public int level=9;
    public int coinNum=0;
    public Dictionary<int ,int> levelStarNumDic=new Dictionary<int, int>();
    public long harvestTime=Linc.Timer.currentTime+GameCfg.harvestTime*60*1000; 

    public int GetLevelStarNum(int level)
    {
        if(!levelStarNumDic.ContainsKey(level)){levelStarNumDic[level]=0;};
        return levelStarNumDic[level];
    }

    public void SetLevelStarNum(int level,int num)
    {
        levelStarNumDic[level]=num;
    }
}
