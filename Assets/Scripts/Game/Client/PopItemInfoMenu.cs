using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client
{
    public class PopItemInfoMenu : UIComponent
    {
        [SerializeField]
        private GameObject root;
        public GameObject obj_prop;
        public Text helpInfoText;
        public Text weightText;
        public List<Text> attrText;

        public Button receiveBtn;
        public Button equipBtn;
        public Button sendBtn;
        public Button useBtn;

        //public override void OnOpenComplete()
        //{
        //    UIPopManager.getInstence().pushWindows(this.root);
        //    base.OnOpenComplete();
        //}

        //public override void OnCloseComplete()
        //{
        //    UIPopManager.getInstence().popWindows(this.root);
        //    base.OnCloseComplete();
        //}
    }
}
