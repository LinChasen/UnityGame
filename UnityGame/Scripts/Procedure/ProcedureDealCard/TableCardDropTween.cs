using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class TableCardDropTween
    {
        private const float cardXRotDelta = 90;
        private const float cardYRotDelta = 90;
        private const int eachAddFrame = 5;
        private const int dropFrame = 60;
        private const float dropBaseY = 7;

        public static int CardDrop(List<CardBase> cardBaseList)
        {
            for (int i = 0; i < cardBaseList.Count; i++)
            {
                CardBase cardBase = cardBaseList[i];
                GameObject card = cardBase.card;
                Vector3 pos = card.transform.position;
                //旋转偏转计算
                float rotX = cardXRotDelta;
                float rotY = pos.x > 0 ? -cardYRotDelta : cardYRotDelta;
                Vector3 aimRot = card.transform.eulerAngles;
                aimRot = Linc.Tween.CalcTweenEuAngles(new Vector3(rotX, rotY), aimRot);
                //卡牌初始位置及旋转
                Vector3 beginPos = pos;
                beginPos.x = 0;
                beginPos.y = dropBaseY;
                card.transform.position = beginPos;
                card.transform.eulerAngles = new Vector3(rotX, rotY);
                Linc.Timer.FrameOnce(i * eachAddFrame, card, (object obj) =>
                {
                    Linc.EaseDic posDic = new Linc.EaseDic();
                    posDic.Add("position", pos);
                    posDic.Add("eulerAngles", aimRot);
                    Linc.Tween.FrameTo(card, card.transform, posDic, dropFrame, Linc.EaseFun.linearIn);
                });
            }
            return (cardBaseList.Count - 1) * eachAddFrame + dropFrame;
        }
    }
}
