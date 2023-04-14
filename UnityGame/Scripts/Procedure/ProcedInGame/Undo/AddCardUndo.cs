using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private class AddCardUndo:UndoBase
    {
        private AddCardBase _addCardBase;
        private  List<NormalCardBase> _newCardList=new List<NormalCardBase>();
        private Vector3 _beginPos=new Vector3();
        private Vector3 _beginRot=new Vector3();

        public AddCardUndo(AddCardBase addCardBase):base(false)
        {
            _addCardBase=addCardBase;
            _beginPos=_addCardBase.focusPos;
            _beginRot=_addCardBase.focusRot;
        }

        public void AddNewCard(NormalCardBase newCardBase)
        {
            _newCardList.Add(newCardBase);
        }

        protected override void UndoStep()
        {
            _addCardBase.card.SetActive(true);
            Transform cardTrs=_addCardBase.card.transform;
            cardTrs.eulerAngles=Vector3.zero;
            cardTrs.localScale=Vector3.one;
            Vector3 beginPos=new Vector3(_beginPos.x,6,_beginPos.z);
            cardTrs.position=beginPos;
            Vector3 aimRot=Linc.Tween.CalcTweenEuAngles(cardTrs.eulerAngles,_beginRot);
            //focus
            _addCardBase.SetFocusPos(_beginPos);
            _addCardBase.SetFocusRot(_beginRot);
            //tween
            Linc.Timer.ClearByObj(_addCardBase.card);
            Linc.EaseDic dic=new Linc.EaseDic();
            dic.Add("position",_addCardBase.focusPos);
            dic.Add("eulerAngles",aimRot);
            Linc.Tween.FrameTo(_addCardBase.card,cardTrs,dic,30,Linc.EaseFun.linearIn);
            //新增手牌移动
            for(int i=0;i<_newCardList.Count;i++)
            {
                var newBase=_newCardList[i];
                _procedure_InGame._gameCardList.normalCardBaseList.Remove(newBase);
                Linc.Timer.ClearByObj(newBase.card);
                Linc.EaseDic dic2=new Linc.EaseDic();
                dic2.Add("position",_addCardBase.focusPos);
                Linc.Tween.FrameTo(newBase.card,newBase.card.transform,dic2,30,Linc.EaseFun.linearIn,()=>{
                    newBase.RemoveFromList();
                    Linc.Game.DestroyObject(newBase.card);
                });
            }
            _procedure_InGame._input.MoveHandCard(true,_addCardBase.addNum);
        }
    }
}
