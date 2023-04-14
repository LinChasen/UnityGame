using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class HandCardAppearTween
    {
        private const float handCardBeginY = -6;
        private const int eachAddFrame = 2;
        private const int appearFrame = 60;
        public static int CardAppear(int beginFrame, List<NormalCardBase> cardBaseList)
        {
            for (int i = 0; i < cardBaseList.Count; i++)
            {
                NormalCardBase cardBase = cardBaseList[i];
                GameObject card = cardBaseList[i].card;
                Vector3 pos = card.transform.position;
                Vector3 rot = card.transform.eulerAngles;
                Vector3 aimPos = pos;
                aimPos.y = handCardBeginY;
                aimPos.x = pos.x + 0.2f;
                card.transform.position = aimPos;
                card.transform.eulerAngles = new Vector3(0, 0, -90);
                rot = Linc.Tween.CalcTweenEuAngles(card.transform.eulerAngles, new Vector3());
                int upFrame = i * eachAddFrame;
                Linc.Timer.FrameOnce(upFrame + beginFrame, card, (object obj2) =>
                {
                    Linc.EaseDic dic = new Linc.EaseDic();
                    dic.Add("position", pos);
                    dic.Add("eulerAngles", rot);
                    Linc.Tween.FrameTo(card, card.transform, dic, appearFrame, Linc.EaseFun.quadInOut);
                });
            }
            return beginFrame + (cardBaseList.Count - 1) * eachAddFrame + appearFrame;
        }
    }
}
