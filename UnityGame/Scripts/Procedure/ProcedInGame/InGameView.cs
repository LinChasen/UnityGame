using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private class InGameView : Linc.SpriteBehaviour
    {
        private Linc.Image2D _undoBtn;

        // Start is called before the first frame update
        void Start()
        {
            LoadView();
        }

        // Update is called once per frame
        void Update()
        {
            _undoBtn.visible = UndoBase.canUndo;
        }

        public void SetUndoBtn()
        {

        }

        private void LoadView()
        {
            _undoBtn = Linc.Sprite2D.CreateSpriteFromObj(gameObject.transform.Find("undoBtn").gameObject) as Linc.Image2D;
            //监听
            _undoBtn.AddClickFun(() =>
            {
                UndoBase.Undo();
            });
        }
    }
}
