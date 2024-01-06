using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client
{
    public class PopDialog : UIComponent
    {
        public Image bg;
        public Image nameBgLeft;
        public Image nameBgRight;
        public GameObject textContainer;
        public Text lblText;
        public Text lblNameLeft;
        public Text lblNameRight;
        public Image nextIcon;
        public CharacterIllustration[] characters;
        public AudioSource sfx;
        [SerializeField]
        public GameObject root;
        [SerializeField]
        public GameObject root_dialog;
        public GameObject dialog_panel;
        public Button touchArea;
        public Button skipButton;
        public Button logButton;
        public Button autoButton;
        private Action OnCloseDialog;

        public override void OnOpenComplete()
        {
            //UIPopManager.getInstence().pushWindows(this.root);
            //this.textContainer.depth = this.root.depth + 1;
            base.OnOpenComplete();
        }

        public override void OnCloseComplete()
        {
            //UIPopManager.getInstence().popWindows(this.root);
            //this.textContainer.depth = this.root.depth + 1;
            base.OnCloseComplete();
        }

        public void RegistOnCloseDialog(Action onClose)
        {
            this.OnCloseDialog = onClose;
        }
    }
}
