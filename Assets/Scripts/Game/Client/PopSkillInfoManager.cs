using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

namespace Game.Client
{
    public class PopSkillInfoManager : PopManager
    {
        protected PopSkillInfoMenu _menu;
        protected const string UI_PREFAB = "PopSkillInfoMenu";
        protected GameObject _window;
        private bool _needRefresh;
        private bool _MenuLoaded;
        protected Vector3 _pos = Vector3.zero;
        protected Skill skill;
        private Role nowRole;
        private Dictionary<string, GameObject> learnedSkillDic;
        protected string _desc;

        public override void DestroyUI(object arg)
        {
            if (_menu != null)
            {
                //_menu.Close();
                Destroy(_menu.gameObject);
            }
            Destroy(gameObject);
        }

        public override void OpenUI(object arg)
        {
            base.OpenUI(arg);
            if (arg != null)
            {
                List<object> list = (List<object>)arg;
                if (list[0] != null)
                {
                    this._pos = (Vector3)list[0];
                }
                if (list[1] != null)
                {
                    this.skill = (Skill)list[1];
                }
                if (list[2] != null)
                {
                    nowRole = (Role)list[2];
                }
                if (list[3] != null)
                {
                    learnedSkillDic = (Dictionary<string, GameObject>)list[3];
                }
            }
            string[] uiName = new string[]
            {
                "PopSkillInfoMenu"
            };
            OnInstantiateOver[] onInstOvers = new OnInstantiateOver[]
            {
                new OnInstantiateOver(this.OnInstantiated)
            };
            UIManager.GetInstance().OpenUI(uiName, new OnOpenComplete(this.OnOpenCompleted), onInstOvers, this.anchor, true);
        }

        private void OnOpenCompleted(GameObject go)
        {
        }

        private void OnInstantiated(GameObject window)
        {
            _window = window;
            if (_window != null)
            {
                _menu = _window.GetComponent<PopSkillInfoMenu>();
                _menu.equipBtn.onClick.AddListener(ChangeSkill);
                _MenuLoaded = true;
                _needRefresh = true;
                InitData();
            }
        }

        private void InitData()
        {
            if (_menu != null)
            {
                _window.transform.position = _pos;// + new Vector3(100, 0, 0);
                SkillInfo skillData = skill.Info;
                //_menu.obj_prop.SafeActive(true);
                _menu.attrText[0].text = "技能类型：" + skillData.SkillType;
                _menu.attrText[1].text = skillData.Help;

                if (nowRole.equipedSkills.Contains(skill))
                {
                    _menu.actionTypeText.text = "卸下";
                }
                else
                {
                    _menu.actionTypeText.text = "装备";
                }
            }
        }

        private void ChangeSkill()
        {
            if (nowRole.equipedSkills.Contains(skill))
            {
                RemoveSkill();
            }
            //装备的技能没满才加
            else if (nowRole.equipedSkills.Count < PlayerData.skillNumber)
            {
                AddSkill();
            }
            Destroy(_menu.gameObject);
            UpdateAction();
        }

        private void RemoveSkill()
        {
            nowRole.equipedSkills.Remove(skill);
            learnedSkillDic[skill.Info.Sid].SetActive(false);
        }

        private void AddSkill()
        {
            nowRole.equipedSkills.Add(skill);
            learnedSkillDic[skill.Info.Sid].SetActive(true);
        }

    }
}