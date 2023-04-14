using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CardBase
{
    private static List<CardBase> _cardList = new List<CardBase>();

    private static bool IntersectFocusCheck(CardBase cardBaseA, CardBase cardBaseB)
    {
        Quaternion quaA = Quaternion.Euler(cardBaseA.focusRot.x, cardBaseA.focusRot.y, cardBaseA.focusRot.z);
        Quaternion quaB = Quaternion.Euler(cardBaseB.focusRot.x, cardBaseB.focusRot.y, cardBaseB.focusRot.z);
        Vector3 rightA = quaA * (new Vector3(1, 0, 0));
        Vector3 upA = quaA * (new Vector3(0, 1, 0));
        Vector3 rightB = quaB * (new Vector3(1, 0, 0));
        Vector3 upB = quaB * (new Vector3(0, 1, 0));
        rightA.Normalize();
        upA.Normalize();
        rightB.Normalize();
        upB.Normalize();
        Vector3 xVec1 = rightA * CardCfg.cardWidth * 0.5f;
        Vector3 yVec1 = upA * CardCfg.cardHeight * 0.5f;
        Vector3 xVec2 = rightB * CardCfg.cardWidth * 0.5f;
        Vector3 yVec2 = upB * CardCfg.cardHeight * 0.5f;
        return Linc.Collision2D.CollisionCheck(cardBaseA.focusPos, cardBaseB.focusPos, xVec1, yVec1, xVec2, yVec2);
    }

    private static bool IntersectRealCheck(CardBase cardBaseA, CardBase cardBaseB)
    {
        Vector3 rotA=cardBaseA.card.transform.eulerAngles;
        Vector3 rotB=cardBaseB.card.transform.eulerAngles;
        Vector3 posA=cardBaseA.card.transform.position;
        Vector3 posB=cardBaseB.card.transform.position;
        Quaternion quaA = Quaternion.Euler(rotA.x, rotA.y, rotA.z);
        Quaternion quaB = Quaternion.Euler(rotB.x, rotB.y, rotB.z);
        Vector3 rightA = quaA * (new Vector3(1, 0, 0));
        Vector3 upA = quaA * (new Vector3(0, 1, 0));
        Vector3 rightB = quaB * (new Vector3(1, 0, 0));
        Vector3 upB = quaB * (new Vector3(0, 1, 0));
        rightA.Normalize();
        upA.Normalize();
        rightB.Normalize();
        upB.Normalize();
        Vector3 xVec1 = rightA * CardCfg.cardWidth * 0.5f;
        Vector3 yVec1 = upA * CardCfg.cardHeight * 0.5f;
        Vector3 xVec2 = rightB * CardCfg.cardWidth * 0.5f;
        Vector3 yVec2 = upB * CardCfg.cardHeight * 0.5f;
        return Linc.Collision2D.CollisionCheck(posA, posB, xVec1, yVec1, xVec2, yVec2);
    }

    public static bool CheckIsSuitCard(CardBase cardBaseA, CardBase cardBaseB)
    {
        // return Mathf.Abs(cardBaseA.cardNum-cardBaseB.cardNum)==1;
        return true;
    }

    public static void SetParent(GameObject levelObj)
    {
        foreach(var item in _cardList){item.card.transform.SetParent(levelObj.transform);};
    }

    public static void Init()
    {
        foreach(var item in _cardList){item.InitFocusValue();};
    }

    public static bool IsInterSectWithCardList(CardBase curCardBase,List<CardBase> cardList)
    {
        for(int i=0;i<cardList.Count;i++)
        {
            if(IntersectRealCheck(curCardBase,cardList[i]))
            {
                return true;
            }
        }
        return false;
    }

}
