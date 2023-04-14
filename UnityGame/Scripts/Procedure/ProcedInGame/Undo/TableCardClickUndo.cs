using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private class TableCardClickUndo : UndoBase
    {
        private Vector3 _beginPos = new Vector3();
        private Vector3 _beginRot = new Vector3();
        private NormalCardBase _cardBase;
        private const int tableCardClickFlyFrame=40;
        private const int turnTopCardFrame=10;

        public TableCardClickUndo(NormalCardBase cardBase) : base(true)
        {
            _cardBase = cardBase;
            _beginPos = cardBase.focusPos;
            _beginRot = cardBase.focusRot;
        }

        protected override void UndoStep()
        {
            _procedure_InGame._gameCardList.currentCardBaseList.Remove(_cardBase);
            _procedure_InGame._gameCardList.normalCardBaseList.Add(_cardBase);
            _cardBase.SetFocusPos(_beginPos);
            _cardBase.SetFocusRot(_beginRot);
            BezierRotTweenModel.Tween(_cardBase,tableCardClickFlyFrame);
            TurnOverNoTopTableCard();
        }

        private void TurnOverNoTopTableCard()//不在最上层的卡牌，必须背面显示
        {
            var tableList = _procedure_InGame.tableNormalCardBaseList;
            for (int i = 0; i < tableList.Count; i++)
            {
                var tableCardBase = tableList[i];
                if (!tableCardBase.isTop && tableCardBase.cardIsFront)
                {
                    Vector3 aimRot = tableCardBase.turnEulerAngle;
                    aimRot = Linc.Tween.CalcTweenEuAngles(tableCardBase.card.transform.eulerAngles, aimRot);
                    tableCardBase.SetFocusRot(aimRot);
                    Linc.Timer.ClearByObj(tableCardBase.card);
                    Linc.EaseDic tableDic = new Linc.EaseDic();
                    tableDic.Add("position",tableCardBase.focusPos);
                    tableDic.Add("eulerAngles", aimRot);
                    //卡牌上升翻转
                    Linc.Tween.FrameTo(tableCardBase.card, tableCardBase.card.transform, tableDic, turnTopCardFrame, Linc.EaseFun.linearIn);
                }
            }
        }
    }
}
