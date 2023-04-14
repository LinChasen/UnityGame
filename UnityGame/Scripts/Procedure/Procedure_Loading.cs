using System.Collections;
using System.Collections.Generic;
using Linc;
using UnityEngine;

public class Procedure_Loading : Linc.ProcedureBase
{
    protected override void OnEnter(object data)
    {
        Linc.ViewMgr.OpenView(ViewType.LoadingView).AddComponent<LoadingView>();
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit()
    {
        Linc.ViewMgr.CloseView(ViewType.LoadingView);
    }
}
