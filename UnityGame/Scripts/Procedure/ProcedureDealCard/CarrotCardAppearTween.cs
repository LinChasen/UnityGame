using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ProcedureDealCard
{
    private class CarrotCardAppearTween
    {
        private const int appearFrame = 30;
        private const int tweenTime = 1;
        private const int tweenFrame = 20;
        private const int stayFrame = 6;
        private const float beginScale = 0.2f;
        private const float endScale = 1.8f;
        private const float nextScale = 1.6f;

        public static int CardAppear(int beginFrame, List<CarrotCardBase> cardBaseList)
        {
            System.Action<GameObject, System.Action> cardAppearFun = (GameObject card, System.Action onComplete) =>
            {
                card.SetActive(true);
                card.transform.localScale = new Vector3(beginScale, beginScale, beginScale);
                Linc.EaseDic dic = new Linc.EaseDic();
                dic.Add("localScale", new Vector3(endScale, endScale, endScale));
                Linc.Tween.FrameTo(card, card.transform, dic, appearFrame, Linc.EaseFun.linearIn, onComplete);
            };
            System.Action<GameObject> cardTweenFun = (GameObject card) => { };
            cardTweenFun = (GameObject card) =>
            {
                Linc.EaseDic tweenDic = new Linc.EaseDic();
                tweenDic.Add("localScale", new Vector3(nextScale, nextScale, nextScale));
                Linc.Tween.FrameTo(card, card.transform, tweenDic, tweenFrame, Linc.EaseFun.linearIn, () =>
                {
                    tweenDic.Clear();
                    tweenDic.Add("localScale", new Vector3(endScale, endScale, endScale));
                    Linc.Tween.FrameTo(card, card.transform, tweenDic, tweenFrame, Linc.EaseFun.linearIn);
                });
            };
            Vector3 stageLdPos = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 stagerdPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0));
            float eachCardWidth = CardCfg.cardWidth * endScale;
            float eachCarrotCardDisX = eachCardWidth * 0.3f;
            float carrotBeginX = (stagerdPos.x - stageLdPos.x - cardBaseList.Count * eachCardWidth - (cardBaseList.Count - 1) * eachCarrotCardDisX) / 2 + stageLdPos.x;
            for (int i = 0; i < cardBaseList.Count; i++)
            {
                GameObject carrotCard = cardBaseList[i].card;
                Vector3 beginPos = new Vector3(carrotBeginX + eachCardWidth * 0.5f + eachCardWidth * i + eachCarrotCardDisX * i, 0, CardCfg.cardTurnZ);
                carrotCard.transform.position = beginPos;
                carrotCard.transform.eulerAngles = new Vector3();
                Linc.Timer.FrameOnce(beginFrame, carrotCard, (object obj) =>
                {
                    cardAppearFun(carrotCard, () =>
                    {
                        cardTweenFun(carrotCard);
                    });
                });
            }
            int addFrame=cardBaseList.Count>0?appearFrame + tweenFrame * 2 * tweenTime + stayFrame:0;
            return beginFrame + addFrame;
        }
    }
}

