using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Procedure_InGame
{
    public class InputLock : Linc.ScBase2D
    {
        private Linc.Image2D _panel;

        // Start is called before the first frame update
        void Start()
        {
            CreateView();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void CreateView()
        {
            _panel=new Linc.Image2D();
            _panel.width=Linc.Stage.width;
            _panel.height=Linc.Stage.height;
            _panel.alpha=0;
            pr.AddChild(_panel);
        }

        public void LockInput()
        {
            _panel.mouseEnable=true;
        }

        public void UnlockInput()
        {
            _panel.mouseEnable=false;
        }
    }
}
