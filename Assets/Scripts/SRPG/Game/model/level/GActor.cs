using Game;
using lib.utils;
using SPRG.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace France.Game.model.level
{
    public class GActor
    {
        //基础属性
        protected int uid;
        protected string name;
        public int hp;  //不止是战斗单位，我希望非战斗单位也能被破坏
        public int maxHp;
        protected int type;
        protected float[] param;
        //显示属性
        protected string prefabFilename;
        //protected GameObject containerObject;
        public GameObject actorObject;
        //protected float x;
        //protected float y;
        //protected float z;
        //protected float scaleX = 1f;
        //protected float scaleY = 1f;
        //protected float scaleZ = 1f;
        protected bool _isVisible = true;
        private bool isTranparented;

        protected int actorFlag;
        protected int mapIndex;

        public GActor(int id, int actorType)
        {
            this.uid = id;
            this.type = actorType;
            this.actorFlag = 1;
            //TODO:目前没有必要弄这个，等以后项目大了再考虑,先在子类中一点一点实现吧
            //this.param = new float[GDATA.PARAM_COUNTS_ACTOR[(GDATA.ACTOR_TYPE)actorType]];
        }

        public virtual void createObj()
        {
            //// 获取预制体文件名
            //string key = this.getPrefabFilename();

            //// 实例化角色对象，设置其父对象为容器对象
            //GameObject characterPrafab = ResourcesExt.Load<GameObject>(key);
            //this.actorObject = UnityEngine.Object.Instantiate<GameObject>(characterPrafab);
            ////this.actorObject.transform.parent = BattleSceneController.instance.view.actorsLayer.transform;

            //// 设置角色对象的本地坐标为零
            //this.actorObject.transform.localPosition = Vector3.zero;

            //// 设置角色对象的标签为 "Actor"，名称为预制体文件名
            //this.actorObject.tag = "Actor";
            //this.actorObject.name = key;

            //// 更新角色对象的视图
            //this.updateView();
        }

        public virtual string getPrefabFilename()
        {
            switch (getActorType())
            {
                case GDATA.ACTOR_TYPE.Decoration:
                    break;
                case GDATA.ACTOR_TYPE.FightUnit:
                    return "Prefabs/Character";
                case GDATA.ACTOR_TYPE.MovingObj:
                    break;
                case GDATA.ACTOR_TYPE.Barrier:
                    break;
                case GDATA.ACTOR_TYPE.Terrian:
                    break;
                case GDATA.ACTOR_TYPE.Interact:
                    break;
                case GDATA.ACTOR_TYPE.SelfDestruction:
                    break;
                case GDATA.ACTOR_TYPE.Sfx3d:
                    break;
                case GDATA.ACTOR_TYPE.BlockMainmenu:
                    break;
                default:
                    break;
            }
            return this.prefabFilename;
        }

        public void updateView()
        {
            //if (this.containerObject != null)
            //{
            //    // 检查角色标志中是否包含“1”位，设置活跃状态
            //    bool active = MathUtils.HasFlag(this.actorFlag, 1);
            //    this.setContainerPos(new Vector3(this.x, this.y, this.z));
            //    this.containerObject.SafeActive(active);
            //    this.setOnTile(this.getMapIndex(), MathUtils.HasFlag(this.actorFlag, 1));
            //}
            //if (this.actorObject != null)
            //{
            //    this.actorObject.transform.localScale = new Vector3(this.scaleX, this.scaleY, this.scaleZ);
            //}
        }

        public static GActor CreateActor(int id, int actorType)
        {
            if (actorType == 1) return new GFightUnit(id);
            throw new System.Exception();
        }

        public int GetUid()
        {
            return this.uid;
        }

        public bool isVisible()
        {
            return this._isVisible;
        }

        public GDATA.ACTOR_TYPE getActorType()
        {
            return (GDATA.ACTOR_TYPE)this.type;
        }

        public void enableActorFlag(DataType.ACTOR_FLAG flag)
        {
            this.setActorFlag(MathUtils.AddFlag(this.getActorFlag(), (int)flag));
        }

        public void disableActorFlag(DataType.ACTOR_FLAG flag)
        {
            this.setActorFlag(MathUtils.RemoveFlag(this.getActorFlag(), (int)flag));
        }

        public bool hasActorFlag(int flag)
        {
            return MathUtils.HasFlag(this.actorFlag, flag);
        }

        public bool hasActorFlag(DataType.ACTOR_FLAG flag)
        {
            return this.hasActorFlag((int)flag);
        }

        public virtual void setActorFlag(int flag)
        {
            this.actorFlag = flag;
            //this.updateView();
        }

        public int getActorFlag()
        {
            return this.actorFlag;
        }

        public int getMapIndex()
        {
            return this.mapIndex;
        }

        protected void setOnTile(int tileIndex, bool isOn)
        {
            if (tileIndex >= 0 && GameLogic.CurLevel != null && GameLogic.CurLevel.map != null)
            {
                GMapTile tileAt = GameLogic.CurLevel.map.GetTileAt(tileIndex);
                if (isOn)
                {
                    //tileAt.actorOn(this);
                }
                else
                {
                    //tileAt.actorOff(this);
                }
            }
        }

        public void setMapIndex(int index)
        {
            if (this.mapIndex != index)
            {
                this.setOnTile(this.mapIndex, false);
                this.mapIndex = index;
                this.setOnTile(this.mapIndex, MathUtils.HasFlag(this.actorFlag, 1));
            }
        }

        public int getParamsCount()
        {
            return this.param.Length;
        }

        public float getParamAt(int index)
        {
            return this.param[index];
        }

        public virtual void setParamAt(int index, float value)
        {
            this.param[index] = value;
        }
    }
}
