using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPool
{
    private static List<string> _cardPoolList = new List<string>();

    private static void InitCardPool()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                string str = (i + 1).ToString() + "_" + (j + 1).ToString();
                _cardPoolList.Add(str);
            }
        }
    }

    public static List<string> poolList{
        get{
            return _cardPoolList;
        }
    }

    public static GameObject rndCardPref
    {
        get
        {
            if(_cardPoolList.Count==0)InitCardPool();
            int rndIndex = Random.Range(0, _cardPoolList.Count);
            GameObject pref = Resources.Load<GameObject>("Prefabs/Cards/" + _cardPoolList[rndIndex]);
            return pref;
        }
    }

}
