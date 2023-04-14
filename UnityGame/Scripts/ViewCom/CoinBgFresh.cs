using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBgFresh : Linc.TextBehaviour
{
    private const int freshFrame=20;
    // Start is called before the first frame update
    void Start()
    {
        textSp.text=AllGlobal.storage.coinNum.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        FreshCoinShow();
    }

    private void FreshCoinShow()
    {
        int curCoin=int.Parse(textSp.text);
        int aimCoin=AllGlobal.storage.coinNum;
        int subCoin=aimCoin-curCoin;
        if(subCoin>0)
        {
            if(subCoin<=freshFrame)
            {
                curCoin+=1;
                textSp.text=curCoin.ToString();
            }
            else{
                int eachAdd=subCoin/freshFrame;
                curCoin+=eachAdd;
                textSp.text=curCoin.ToString();
            }
        }
    }
}
