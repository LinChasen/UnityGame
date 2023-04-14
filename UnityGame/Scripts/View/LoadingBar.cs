using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingBar : Linc.ImageBehaviour
{
    private const int c_loadingFrame=60;

    // Start is called before the first frame update
    void Start()
    {
        FreshBarProgress();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FreshBarProgress()
    {
        int timerId=-1;
        int curFrame=0;
        timerId=Linc.Timer.FrameLoop(1,gameObject,(object obj)=>{
            if(curFrame<c_loadingFrame)
            {
                curFrame+=1;
                imageSp.scaleX=curFrame/(float)c_loadingFrame;
            }
            else{
                Linc.Timer.ClearById(timerId);
                Linc.ViewMgr.CloseView(ViewType.LoadingView);
                Linc.ViewMgr.OpenView(ViewType.GameWinView);
            }
        });
    }
}
