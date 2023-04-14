using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    private partial class GameInput
    {

        private void TurnOverTopTableCard(TableCardClickUndo rootUndo)
        {
            TurnOverTopNormalCard(rootUndo);
            TurnOverTopCarrotCard(rootUndo);
            TurnOverTopAddCard(rootUndo);
        }
    }
}
