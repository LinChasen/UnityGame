using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private const float carrotBigScale = 1.2f;
        private const float carrotMinScale = 0.3f;

        private void TurnOverTopCarrotCard(TableCardClickUndo rootUndo)
        {
            var carrotCardBaseList = _procedure_InGame._gameCardList.carrotCardBaseList;
            var rabbitCardBaseList = _procedure_InGame._gameCardList.rabbitCardBaseList;
            for (int i = 0; i < carrotCardBaseList.Count; i++)
            {
                var carrotCardBase = carrotCardBaseList[i];
                if (carrotCardBase.isTop)
                {
                    //选择一个兔子
                    var rabbitBase = rabbitCardBaseList[rabbitCardBaseList.Count - 1];
                    rabbitCardBaseList.Remove(rabbitBase);
                    carrotCardBaseList.RemoveAt(i);
                    i--;
                    //创建chUndo
                    CarrotEatUndo undo = new CarrotEatUndo(carrotCardBase, rabbitBase);
                    rootUndo.AddChUndo(undo);
                    Linc.Timer.ClearByObj(carrotCardBase.card);
                    Linc.EaseDic dic = new Linc.EaseDic();
                    dic.Add("localScale", new Vector3(carrotBigScale, carrotBigScale, carrotBigScale));
                    Linc.Tween.FrameTo(carrotCardBase.card, carrotCardBase.card.transform, dic, 20, Linc.EaseFun.linearIn, () =>
                    {
                        dic.Clear();
                        dic.Add("localScale", new Vector3(carrotMinScale, carrotMinScale, carrotMinScale));
                        Linc.Tween.FrameTo(carrotCardBase.card, carrotCardBase.card.transform, dic, 30, Linc.EaseFun.linearIn, () =>
                        {
                            carrotCardBase.card.SetActive(false);
                            TurnOverTopTableCard(rootUndo);
                            rabbitBase.EatCarrot();
                            Linc.Timer.FrameOnce(100, carrotCardBase.card, (object obj) =>
                            {
                                rabbitBase.card.SetActive(false);
                                TurnOverTopTableCard(rootUndo);
                            });
                        });
                    });
                }
            }
        }
    }
}