using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCardBase : CardBase
{
    public AddCardBase(GameObject inCard,int addNum):base(inCard)
    {
        _addNum=addNum;
        SetCardArea(CardArea.table);
    }

    private int _addNum=1;

    public void SetAddNum(int num)
    {
        _addNum=num;
    }

    public int addNum{
        get{
            return _addNum;
        }
    }
}
