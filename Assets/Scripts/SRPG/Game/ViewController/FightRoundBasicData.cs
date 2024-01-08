using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class FightRoundBasicData
{
    public int roundType;
    public Character attacker;
    public Character defender;
    public Role roleAttacker;
    public Role roleDefender;
    //public WeaponData weaponAttacker;
    //public WeaponData weaponDefender;
    //public EquipData equipAttacker;
    //public EquipData equipDefender;
    //public ContraCoefficient contraAttacker;
    //public ContraCoefficient contraDefender;
    public float[] buffPropertiesAttacker;
    public float[] buffPropertiesDefender;
    //public GWeatherData weather;
    //public GTerrainData terrainAttacker;
    //public GTerrainData terrainDefender;

    public FightRoundBasicData(int roundType, Character attacker, Character defender, bool isSimulate, bool ignoreBuffProp)
    {
        this.roundType = roundType;
        this.attacker = attacker;
        this.defender = defender;
        this.roleAttacker = ((attacker != null) ? attacker.getRole() : null);
        this.roleDefender = ((defender != null) ? defender.getRole() : null);
        //this.weaponAttacker = null;
        //this.weaponDefender = null;
        //this.equipAttacker = null;
        //this.equipDefender = null;
        //if (this.roleAttacker != null)
        //{
        //    Item weapon = attacker.getWeapon();
        //    if (weapon != null)
        //    {
        //        this.weaponAttacker = (WeaponData)weapon.getItemData();
        //    }
        //    if (this.roleAttacker.equip != null)
        //    {
        //        this.equipAttacker = (EquipData)this.roleAttacker.equip.getItemData();
        //    }
        //}
        //if (this.roleDefender != null)
        //{
        //    Item weapon2 = defender.getWeapon();
        //    if (weapon2 != null)
        //    {
        //        this.weaponDefender = (WeaponData)weapon2.getItemData();
        //    }
        //    if (this.roleDefender.equip != null)
        //    {
        //        this.equipDefender = (EquipData)this.roleDefender.equip.getItemData();
        //    }
        //}
        //this.contraAttacker = ((this.weaponAttacker != null && this.roleDefender != null) ? CharacterDataManager.GetContra(this.roleAttacker.classId, this.roleDefender.classId) : new ContraCoefficient());
        //this.contraDefender = ((this.weaponDefender != null && this.roleAttacker != null) ? CharacterDataManager.GetContra(this.roleDefender.classId, this.roleAttacker.classId) : new ContraCoefficient());
        //this.buffPropertiesAttacker = ((attacker != null && !ignoreBuffProp) ? attacker.getBuffProperties(-1, defender, this, isSimulate) : new float[18]);
        //this.buffPropertiesDefender = ((defender != null && !ignoreBuffProp) ? defender.getBuffProperties(-1, attacker, this, isSimulate) : new float[18]);
        //GLevel curLevel = GameLogic.CurLevel;
        //GMap map = curLevel.map;
        //this.weather = BattleSceneController.instance.getWeather();
        //int type = (attacker != null) ? map.getTileAt(attacker.getMapIndex()).getTerrainId() : -1;
        //int type2 = (defender != null) ? map.getTileAt(defender.getMapIndex()).getTerrainId() : -1;
        //this.terrainAttacker = LevelGlobalManager.GetInstance().getTerrainData(type);
        //this.terrainDefender = LevelGlobalManager.GetInstance().getTerrainData(type2);
    }
}
