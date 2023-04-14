using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private class UndoBase
    {
        public UndoBase(bool isRootUndo)
        {
            if (isRootUndo) { _undoList.Add(this); }
        }

        private static List<UndoBase> _undoList = new List<UndoBase>();
        private List<UndoBase> _chUndoList = new List<UndoBase>();

        protected static Procedure_InGame _procedure_InGame;

        public void AddChUndo(UndoBase undoBase)
        {
            _chUndoList.Add(undoBase);
        }

        private void UndoCh()
        {
            foreach (var item in _chUndoList) { item.UndoStep(); };
        }

        protected virtual void UndoStep() { }

        public static void Init(Procedure_InGame procedure_InGame)
        {
            _procedure_InGame=procedure_InGame;
        }

        public static void Undo()
        {
            if (_undoList.Count > 0)
            {
                var undoBase = _undoList[_undoList.Count - 1];
                undoBase.UndoCh();
                undoBase.UndoStep();
                _undoList.RemoveAt(_undoList.Count - 1);
            }
        }

        public static bool canUndo
        {
            get
            {
                return _undoList.Count > 0;
            }
        }
    }
}
