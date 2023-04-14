using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class RabbitCardAppearTween
    {
        private const int appearFrame = 60;
        public static int CardAppear(int beginFrame, List<RabbitCardBase> cardBaseList)
        {
            for (int i = 0; i < cardBaseList.Count; i++)//兔子牌
            {
                RabbitCardBase cardBase = cardBaseList[i];
                GameObject aniCard = cardBaseList[i].card;
                aniCard.transform.localScale = Vector3.zero;
                Linc.Timer.FrameOnce(beginFrame, aniCard, (object obj) =>
                {
                    Linc.EaseDic dic = new Linc.EaseDic();
                    dic.Add("localScale", Vector3.one);
                    Linc.Tween.FrameTo(aniCard, aniCard.transform, dic, appearFrame, Linc.EaseFun.quadInOut);
                });
            }
            int frameNum = cardBaseList.Count > 0 ? appearFrame : 0;
            return beginFrame + frameNum;
        }
    }
}
