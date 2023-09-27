//
// Auto Generated Code By excel2json
// https://neil3d.gitee.io/coding/excel2json.html
// 1. 每个 Sheet 形成一个 Struct 定义, Sheet 的名称作为 Struct 的名称
// 2. 表格约定：第一行是变量名称，第二行是变量类型

// Generate From C:\Users\legend\Desktop\skill.xlsx.xlsx

public class Skill
{
    public string Sid; // スキル
    public string Name; // 名前
    public string Help; // ヘルプ
    public string IconLabel; // アイコンラベル
    public int Priority; // 習得優先度
    public int Order; // 実行順
    public int Cycle; // 更新周期，即CD
    public int CD; // 剩余CD数
    public int MPCost; // 消耗MP数
    public int Life; // 寿命，持续回合
    public int Timing; // タイミング
    public int Target; // 対象
    public string Condition; // 発動条件
    public string ActNames; // パラメータ
    public string ActOperations; // 演算
    public string ActValues; // 値，如果是回血的话（目前看后面type，之后打算整合到timing），目前只设计第一个生效，后面可以设置中心点大，其他小等等
    public int AroundCenter; // 中心
    public short AroundTarget; // 対象
    public string AroundCondition; // 発動条件
    public string AroundName; // パラメータ
    public string AroundOperation; // 演算
    public string AroundValue; // 値
    public short GiveTarget; // 付与対象
    public string GiveCondition; // 付与条件
    public string GiveSids; // 付与スキル
    public string RemoveSids; // 削除スキル
    public string SyncConditions; // 同時習得条件
    public string SyncSids; // 同時習得スキル
    public string RebirthSid; // 周期変化スキル
    public string EngageSid; // エンゲージ変化スキル
    public string ChangeSids; // 切替変化スキル
    public string AttackRange; // 攻撃範囲
    public string OverlapRange; // 配置範囲
    public string OverlapTerrain; // 配置地形
    public int Power; // 強さ
    public int Rewarp; // 転移力
    public int Removable; // 再移動力
    public int Cost; // 技コスト
    public int MoveSelf; // 自分移動
    public int MoveTarget; // 相手移動
    public uint SkillType; // 技能类型（0：被动，1：治疗，2：伤害）
    public int RangeTarget; // 射程対象（自己，我方，敌方，格子，所有）
    public int RangeI; // 内射程，技能射程
    public int RangeO; // 外射程
    public int ActionRangeI; // 作用范围内
    public int ActionRangeO; // 作用范围外
    public int ActionValue; // 技能的倍率
    public int RangeAdd; // 射程加算
    public int RangeExtend; // 範囲拡張
    public int Efficacy; // 特効
    public int EfficacyValue; // 特効係数
    public int EfficacyIgnore; // 特効無効
    public int BadState; // 状態異常
    public int BadIgnore; // 状態無効
    public int None; // 無
    public int Sword; // 剣
    public int Lance; // 槍
    public int Axe; // 斧
    public int Bow; // 弓
    public int Dagger; // 短
    public int Magic; // 魔
    public int Rod; // 杖
    public int Fist; // 拳
    public int Special; // 特
    public string EquipIids; // 強制装備
    public string Effect; // エフェクト名
    public ActiveSkillAction activeSkillAction; // 主动技能行为
}


// End of Auto Generated Code
