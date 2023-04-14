using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotCardBase : CardBase
{
    public CarrotCardBase(GameObject inCard):base(inCard)
    {
        SetCardArea(CardArea.table);
    }
}
