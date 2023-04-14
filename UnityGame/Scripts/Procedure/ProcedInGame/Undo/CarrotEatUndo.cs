using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public partial class Procedure_InGame
{
    private class CarrotEatUndo : UndoBase
    {
        private CarrotCardBase _cardBase;
        private RabbitCardBase _rabbitCardBase;

        public CarrotEatUndo(CarrotCardBase carrotCardBase, RabbitCardBase rabbitCardBase) : base(false)
        {
            _cardBase = carrotCardBase;
            _rabbitCardBase = rabbitCardBase;
        }

        protected override void UndoStep()
        {
            _procedure_InGame._gameCardList.carrotCardBaseList.Add(_cardBase);
            _procedure_InGame._gameCardList.rabbitCardBaseList.Add(_rabbitCardBase);
            _cardBase.card.SetActive(true);
            _rabbitCardBase.card.SetActive(true);
            _rabbitCardBase.Idle();
            //萝卜变大
            Linc.Timer.ClearByObj(_cardBase.card);
            float minScale = 0.3f;
            _cardBase.card.transform.localScale = new Vector3(minScale, minScale, minScale);
            Linc.EaseDic dic = new Linc.EaseDic();
            dic.Add("localScale", Vector3.one);
            Linc.Tween.FrameTo(_cardBase.card, _cardBase.card.transform, dic, 30, Linc.EaseFun.linearIn);
            //兔子变大
            _rabbitCardBase.card.transform.localScale = new Vector3();
            Linc.EaseDic dic2 = new Linc.EaseDic();
            dic2.Add("localScale", Vector3.one);
            Linc.Tween.FrameTo(_rabbitCardBase.card, _rabbitCardBase.card.transform, dic, 30, Linc.EaseFun.linearIn);
        }
    }
}
