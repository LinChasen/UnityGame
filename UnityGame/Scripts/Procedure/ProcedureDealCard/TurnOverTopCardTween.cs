using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class TurnOverTopCardTween
    {
        private const int eachAddFrame = 3;
        private const int turnOverFrame = 10;
        public static int TurnOverCard(int beginFrame, List<NormalCardBase> cardBaseList)
        {
            for (int i = 0; i < cardBaseList.Count; i++)
            {
                int turnFrame = i * eachAddFrame;
                NormalCardBase cardBase = cardBaseList[i];
                Linc.Timer.FrameOnce(beginFrame + turnFrame, cardBase.card, (object obj2) =>
                {
                    Vector3 pos = cardBase.card.transform.position;
                    Vector3 rot = cardBase.card.transform.eulerAngles;
                    Quaternion qua = Quaternion.AngleAxis(180, cardBase.card.transform.up);
                    Quaternion aimQua = qua * cardBase.card.transform.rotation;
                    Vector3 aimRot = aimQua.eulerAngles;
                    aimRot = Linc.Tween.CalcTweenEuAngles(cardBase.card.transform.eulerAngles, aimRot);
                    Linc.EaseDic dic = new Linc.EaseDic();
                    dic.Add("eulerAngles", aimRot);
                    //卡牌上升翻转
                    Linc.Tween.FrameTo(cardBase.card, cardBase.card.transform, dic, turnOverFrame, Linc.EaseFun.linearIn);
                });
            }
            return beginFrame + (cardBaseList.Count - 1) * eachAddFrame + turnOverFrame;
        }
    }
}
