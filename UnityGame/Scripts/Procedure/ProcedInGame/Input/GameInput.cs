using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {
        private Procedure_InGame _procedure_InGame;
        public GameInput(Procedure_InGame procedure_InGame)
        {
            _procedure_InGame = procedure_InGame;
            Init();
        }

        private Linc.Image2D _panel;

        private void Init()
        {
            //panel
            _panel = new Linc.Image2D();
            _panel.width = Linc.Stage.width;
            _panel.height = Linc.Stage.height;
            Linc.Stage.AddChild(_panel);
            _panel.alpha = 0;
            _panel.gameObject.transform.SetAsFirstSibling();
            //监听
            _panel.on(Linc.Event.mouseDown, OnPanelDown);
        }

        private void OnPanelDown()
        {
            Vector3 posV3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var tableList = _procedure_InGame.tableNormalCardBaseList;
            var handList = _procedure_InGame.handNormalCardBaseList;
            for (int i = 0; i < tableList.Count; i++)
            {
                NormalCardBase cardBase = tableList[i];
                if (cardBase.IsPosInCardRange(posV3) && cardBase.isTop && CardBase.CheckIsSuitCard(_procedure_InGame.currentTopCardBase, cardBase))
                {
                    TableCardClick(cardBase);
                    return;
                }
            }
            for (int i = 0; i < handList.Count; i++)
            {
                NormalCardBase cardBase = handList[i];
                if (cardBase.IsPosInCardRange(posV3) && cardBase.isTop)
                {
                    HandCardClick(cardBase);
                    return;
                }
            }
        }
    }
}
