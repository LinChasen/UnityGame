using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCardBase : CardBase
{
    public NormalCardBase(GameObject inCard) : base(inCard)
    {

    }

    public int cardNum
    {
        get
        {
            int num = int.Parse(card.gameObject.name.Split('_')[1].Split('(')[0]);
            return num;
        }
    }
}
