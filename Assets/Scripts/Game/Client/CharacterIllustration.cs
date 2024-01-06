using System;
using lib.notify;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Client
{
    public class CharacterIllustration : MonoBehaviour
    {
        public const float FADE_SPEED = 0.05f;
        public Image bodyImg;
        public Image faceImg;
        public INOUT_STATE inoutState;
        public float offsetX;
        private Texture2D mainTexture;
        private static Color32[] MainColorArray;

        public enum INOUT_STATE
        {
            NONE,
            MOVE_IN_LEFT,
            MOVE_IN_RIGHT,
            MOVE_OUT_LEFT,
            MOVE_OUT_RIGHT,
            FADE_IN,
            FADE_OUT
        }

        public void Update()
        {
            switch (this.inoutState)
            {
                case INOUT_STATE.MOVE_IN_LEFT:
                case INOUT_STATE.MOVE_IN_RIGHT:
                case INOUT_STATE.MOVE_OUT_LEFT:
                case INOUT_STATE.MOVE_OUT_RIGHT:
                    if (this.offsetX != 0f)
                    {
                        float num = Mathf.Abs(this.offsetX);
                        if (num > 1f)
                        {
                            float num2 = (num <= 10f) ? (-this.offsetX / num) : (-this.offsetX / 10f);
                            this.offsetX += num2;
                            Vector3 localPosition = this.bodyImg.transform.localPosition;
                            this.bodyImg.transform.localPosition = new Vector3(localPosition.x + num2, localPosition.y, localPosition.z);
                        }
                        else
                        {
                            INOUT_STATE inout_STATE = this.inoutState;
                            Vector3 localPosition2 = this.bodyImg.transform.localPosition;
                            this.bodyImg.transform.localPosition = new Vector3(localPosition2.x - this.offsetX, localPosition2.y, localPosition2.z);
                            this.offsetX = 0f;
                            this.inoutState = INOUT_STATE.NONE;
                            if (inout_STATE == INOUT_STATE.MOVE_IN_LEFT || inout_STATE == INOUT_STATE.MOVE_IN_RIGHT)
                            {
                                Notifier.Notify(103, new object[0]);
                            }
                            else if (inout_STATE == INOUT_STATE.MOVE_OUT_LEFT || inout_STATE == INOUT_STATE.MOVE_OUT_RIGHT)
                            {
                                Notifier.Notify(104, new object[0]);
                            }
                        }
                    }
                    break;
                case INOUT_STATE.FADE_IN:
                    {
                        float num3 = this.bodyImg.color.a;
                        if (num3 < 1f)
                        {
                            num3 = Mathf.Min(1f, num3 + 0.05f);
                            this.bodyImg.color = new Color(1f, 1f, 1f, num3);
                        }
                        else
                        {
                            this.inoutState = INOUT_STATE.NONE;
                            Notifier.Notify(103, new object[0]);
                        }
                        break;
                    }
                case INOUT_STATE.FADE_OUT:
                    {
                        float num4 = this.bodyImg.color.a;
                        if (num4 > 0f)
                        {
                            num4 = Mathf.Max(0f, num4 - 0.05f);
                            float r = this.bodyImg.color.r;
                            this.bodyImg.color = new Color(r, r, r, num4);
                        }
                        else
                        {
                            this.inoutState = INOUT_STATE.NONE;
                            Notifier.Notify(104, new object[0]);
                        }
                        break;
                    }
            }
        }

        public void setData(string bodyImgFilename, Vector3 pos, int flipFlag, bool initColor, string faceImgFilename, int faceX, int faceY)
        {
            //Texture2D bodyTex = null;
            //AssetBundleLoader.getInstance().loadAssetBundle("art/" + bodyImgFilename, delegate (AssetBundle assetbundle)
            //{
            //    bodyTex = (Texture2D)assetbundle.LoadAsset(bodyImgFilename);
            //}, false, ".assetbundle");
            Texture2D bodyTex = Resources.Load<Texture2D>("Picture/" + bodyImgFilename);
            if (this.mainTexture == null || this.mainTexture.width != bodyTex.width || this.mainTexture.height != bodyTex.height)
            {
                if (this.mainTexture != null)
                {
                    DestroyImmediate(this.mainTexture);
                }
                this.mainTexture = new Texture2D(bodyTex.width, bodyTex.height, TextureFormat.RGBA32, false);
                MainColorArray = new Color32[bodyTex.width * bodyTex.height];
            }
            mainTexture = bodyTex;
            //MainColorArray = bodyTex.GetPixels32();
            if (!string.IsNullOrEmpty(faceImgFilename))
            {
                Texture2D faceTex = null;
                AssetBundleLoader.getInstance().loadAssetBundle("art/" + faceImgFilename, delegate (AssetBundle assetbundle)
                {
                    faceTex = (Texture2D)assetbundle.LoadAsset(faceImgFilename);
                }, false, ".assetbundle");
                Color32[] pixels = faceTex.GetPixels32();
                for (int i = 0; i < pixels.Length; i++)
                {
                    int num = i % faceTex.width;
                    int num2 = i / faceTex.width;
                    int num3 = num + faceX;
                    int num4 = num2 + faceY;
                    if (num3 < bodyTex.width && num4 < bodyTex.height && pixels[i].a == 255)
                    {
                        int num5 = num3 + num4 * bodyTex.width;
                        MainColorArray[num5] = pixels[i];
                    }
                }
            }
            //this.mainTexture.SetPixels32(0, 0, bodyTex.width, bodyTex.height, MainColorArray);
            //this.mainTexture.Apply();
            //AssetBundleLoader.getInstance().unloadAssetBundle("art/" + bodyImgFilename, true);
            if (this.bodyImg != null)
            {
                // 创建一个新的 Sprite，使用新的主纹理
                Sprite newSprite = Sprite.Create(mainTexture, new Rect(0, 0, mainTexture.width, mainTexture.height), new Vector2(0.5f, 0.5f));
                // 将新的 Sprite 分配给 Image 组件
                bodyImg.sprite = newSprite;

                if (initColor)
                {
                    this.bodyImg.color = new Color(1f, 1f, 1f);
                }
                bodyImg.rectTransform.sizeDelta = new Vector2(bodyImg.mainTexture.width, bodyImg.mainTexture.height);
                Vector3 localScale = this.bodyImg.transform.localScale;
                localScale.x = Mathf.Abs(localScale.x) * (float)flipFlag;
                this.bodyImg.transform.localScale = localScale;
                this.bodyImg.transform.localPosition = pos;
            }
            base.gameObject.SafeActive(true);
            if (this.faceImg != null)
            {
                this.faceImg.gameObject.SafeActive(false);
            }
        }
    }
}
