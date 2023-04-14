using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Playables;

public partial class Procedure_InGame : Linc.ProcedureBase
{
    private GameObject _levelObj;
    private GameInput _input;
    private InputLock _inputLock;
    private GameCardList _gameCardList;

    protected override void OnInit()
    {
        _levelObj = Linc.Game.gameObject.transform.GetChild(0).gameObject;
    }

    protected override void OnEnter(object data)
    {
        _gameCardList = data as GameCardList;
        _input = new GameInput(this);
        Linc.ViewMgr.GetView(ViewType.InGameView).AddComponent<InGameView>();
        _inputLock=Linc.Stage.AddViewCom<InputLock>();
        UndoBase.Init(this);
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {

    }

    private CardBase currentTopCardBase
    {
        get
        {
            return _gameCardList.currentCardBaseList[_gameCardList.currentCardBaseList.Count - 1];
        }
    }

    private Vector3 nextCurrentTopPos
    {
        get
        {
            Vector3 curTopPos = currentTopCardBase.focusPos;
            curTopPos.z -= CardCfg.cardPosOffsetZ;
            return curTopPos;
        }
    }

    private List<NormalCardBase> tableNormalCardBaseList
    {
        get
        {
            List<NormalCardBase> tableList = new List<NormalCardBase>();
            foreach (var item in _gameCardList.normalCardBaseList) { if (item.cardArea == CardArea.table) tableList.Add(item); };
            return tableList;
        }
    }

    private List<NormalCardBase> handNormalCardBaseList
    {
        get
        {
            List<NormalCardBase> handList = new List<NormalCardBase>();
            foreach (var item in _gameCardList.normalCardBaseList) { if (item.cardArea == CardArea.hand) handList.Add(item); };
            return handList;
        }
    }
}
