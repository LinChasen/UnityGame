using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingView : Linc.SpriteBehaviour
{
    private const int c_loadingFrame=60;

    private void Start() {
        FreshBarProgress();
    }

    private void FreshBarProgress()
    {
        GameObject barObj=gameObject.transform.Find("progressBack").GetChild(0).gameObject;
        Linc.Image2D barSp=Linc.Sprite2D.CreateSpriteFromObj(barObj)as Linc.Image2D;
        int timerId=-1;
        int curFrame=0;
        timerId=Linc.Timer.FrameLoop(1,gameObject,(object obj)=>{
            if(curFrame<c_loadingFrame)
            {
                curFrame+=1;
                barSp.scaleX=curFrame/(float)c_loadingFrame;
            }
            else{
                Linc.Timer.ClearById(timerId);
                Linc.ProcedureMgr.ChangeToProcedure<ProcedureMainView>();
            }
        });
    }
}
