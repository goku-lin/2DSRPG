using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RecordInfo
{
    public long id;                      // 记录的唯一标识符
    public bool isAutoSave;               // 是否为自动保存记录
    public bool isLevelRecord;            // 是否为关卡记录
    public DateTime recordTime;           // 记录的时间戳
    public long gameTotalTime;            // 游戏总时间
    public int chapterId;                 // 章节 ID
    public int locId;                     // 位置 ID
    public int buttonId;                  // 按钮 ID
    public string levelFilename;          // 关卡文件名
    public int battleRound;               // 战斗回合数
    public int difficulty;                // 游戏难度
    public int newgamePlus;               // 是否进行了 New Game Plus
    public bool isCleanup;                // 是否进行了清理
    public byte[] thumb;                  // 缩略图数据
    public const int THUMB_WIDTH = 222;   // 缩略图宽度
    public const int THUMB_HEIGHT = 125;  // 缩略图高度

    // 获取记录的文件名
    public string getFilename()
    {
        return "R" + this.id;
    }

    // 保存记录信息到二进制流
    public void save(BinaryWriter writer, bool autoSave)
    {
        this.isAutoSave = autoSave;
        writer.Write(this.id);
        writer.Write(this.isAutoSave);
        writer.Write(this.isLevelRecord);
        writer.Write(this.recordTime.Ticks);
        writer.Write(this.gameTotalTime);
        writer.Write(this.chapterId);
        writer.Write(this.locId);
        writer.Write(this.buttonId);
        writer.Write(this.levelFilename);
        writer.Write(this.battleRound);
        writer.Write(this.difficulty);

        if (this.thumb == null)
        {
            writer.Write(0);
        }
        else
        {
            writer.Write(this.thumb.Length);
            writer.Write(this.thumb);
        }

        writer.Write(this.newgamePlus);
        writer.Write(this.isCleanup);
    }

    // 从二进制流中加载记录信息
    public static RecordInfo Load(BinaryReader reader, int version)
    {
        RecordInfo recordInfo = new RecordInfo();
        recordInfo.id = reader.ReadInt64();
        recordInfo.isAutoSave = reader.ReadBoolean();
        recordInfo.isLevelRecord = reader.ReadBoolean();
        recordInfo.recordTime = new DateTime(reader.ReadInt64());
        recordInfo.gameTotalTime = reader.ReadInt64();
        recordInfo.chapterId = reader.ReadInt32();
        recordInfo.locId = reader.ReadInt32();
        recordInfo.buttonId = reader.ReadInt32();
        recordInfo.levelFilename = reader.ReadString();
        recordInfo.battleRound = reader.ReadInt32();
        recordInfo.difficulty = reader.ReadInt32();
        int num = reader.ReadInt32();
        if (num > 0)
        {
            recordInfo.thumb = reader.ReadBytes(num);
        }
        if (version >= 15)
        {
            recordInfo.newgamePlus = reader.ReadInt32();
            recordInfo.isCleanup = reader.ReadBoolean();
        }
        return recordInfo;
    }
}
