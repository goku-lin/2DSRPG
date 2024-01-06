using System;
using System.Collections.Generic;
using System.IO;

namespace InControl
{
    public struct KeyCombo
    {
        public KeyCombo(params Key[] keys)
        {
            this.includeData = 0UL;
            this.includeSize = 0;
            this.excludeData = 0UL;
            this.excludeSize = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                this.AddInclude(keys[i]);
            }
        }

        private void AddIncludeInt(int key)
        {
            if (this.includeSize == 8)
            {
                return;
            }
            this.includeData |= (ulong)((ulong)((long)key & 255L) << this.includeSize * 8);
            this.includeSize++;
        }

        private int GetIncludeInt(int index)
        {
            return (int)(this.includeData >> index * 8 & 255UL);
        }

        [Obsolete("Use KeyCombo.AddInclude instead.")]
        public void Add(Key key)
        {
            this.AddInclude(key);
        }

        [Obsolete("Use KeyCombo.GetInclude instead.")]
        public Key Get(int index)
        {
            return this.GetInclude(index);
        }

        public void AddInclude(Key key)
        {
            this.AddIncludeInt((int)key);
        }

        public Key GetInclude(int index)
        {
            if (index < 0 || index >= this.includeSize)
            {
                throw new IndexOutOfRangeException(string.Concat(new object[]
                {
                    "Index ",
                    index,
                    " is out of the range 0..",
                    this.includeSize
                }));
            }
            return (Key)this.GetIncludeInt(index);
        }

        private void AddExcludeInt(int key)
        {
            if (this.excludeSize == 8)
            {
                return;
            }
            this.excludeData |= (ulong)((ulong)((long)key & 255L) << this.excludeSize * 8);
            this.excludeSize++;
        }

        private int GetExcludeInt(int index)
        {
            return (int)(this.excludeData >> index * 8 & 255UL);
        }

        public void AddExclude(Key key)
        {
            this.AddExcludeInt((int)key);
        }

        public Key GetExclude(int index)
        {
            if (index < 0 || index >= this.excludeSize)
            {
                throw new IndexOutOfRangeException(string.Concat(new object[]
                {
                    "Index ",
                    index,
                    " is out of the range 0..",
                    this.excludeSize
                }));
            }
            return (Key)this.GetExcludeInt(index);
        }

        public static KeyCombo With(params Key[] keys)
        {
            return new KeyCombo(keys);
        }

        public KeyCombo AndNot(params Key[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                this.AddExclude(keys[i]);
            }
            return this;
        }

        public void Clear()
        {
            this.includeData = 0UL;
            this.includeSize = 0;
            this.excludeData = 0UL;
            this.excludeSize = 0;
        }

        [Obsolete("Use KeyCombo.IncludeCount instead.")]
        public int Count
        {
            get
            {
                return this.includeSize;
            }
        }

        public int IncludeCount
        {
            get
            {
                return this.includeSize;
            }
        }

        public int ExcludeCount
        {
            get
            {
                return this.excludeSize;
            }
        }

        public bool IsPressed
        {
            get
            {
                if (this.includeSize == 0)
                {
                    return false;
                }
                bool flag = true;
                for (int i = 0; i < this.includeSize; i++)
                {
                    int includeInt = this.GetIncludeInt(i);
                    flag = (flag && KeyInfo.KeyList[includeInt].IsPressed);
                }
                for (int j = 0; j < this.excludeSize; j++)
                {
                    int excludeInt = this.GetExcludeInt(j);
                    if (KeyInfo.KeyList[excludeInt].IsPressed)
                    {
                        return false;
                    }
                }
                return flag;
            }
        }

        public static KeyCombo Detect(bool modifiersAsKeys)
        {
            KeyCombo result = default(KeyCombo);
            if (modifiersAsKeys)
            {
                for (int i = 5; i < 13; i++)
                {
                    if (KeyInfo.KeyList[i].IsPressed)
                    {
                        result.AddIncludeInt(i);
                        return result;
                    }
                }
            }
            else
            {
                for (int j = 1; j < 5; j++)
                {
                    if (KeyInfo.KeyList[j].IsPressed)
                    {
                        result.AddIncludeInt(j);
                    }
                }
            }
            for (int k = 13; k < KeyInfo.KeyList.Length; k++)
            {
                if (KeyInfo.KeyList[k].IsPressed)
                {
                    result.AddIncludeInt(k);
                    return result;
                }
            }
            result.Clear();
            return result;
        }

        public override string ToString()
        {
            string text;
            if (!KeyCombo.cachedStrings.TryGetValue(this.includeData, out text))
            {
                text = string.Empty;
                for (int i = 0; i < this.includeSize; i++)
                {
                    if (i != 0)
                    {
                        text += " ";
                    }
                    int includeInt = this.GetIncludeInt(i);
                    text += KeyInfo.KeyList[includeInt].Name;
                }
            }
            return text;
        }

        public static bool operator ==(KeyCombo a, KeyCombo b)
        {
            return a.includeData == b.includeData && a.excludeData == b.excludeData;
        }

        public static bool operator !=(KeyCombo a, KeyCombo b)
        {
            return a.includeData != b.includeData || a.excludeData != b.excludeData;
        }

        public override bool Equals(object other)
        {
            if (other is KeyCombo)
            {
                KeyCombo keyCombo = (KeyCombo)other;
                return this.includeData == keyCombo.includeData && this.excludeData == keyCombo.excludeData;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int num = 17;
            num = num * 31 + this.includeData.GetHashCode();
            return num * 31 + this.excludeData.GetHashCode();
        }

        internal void Load(BinaryReader reader, ushort dataFormatVersion)
        {
            if (dataFormatVersion == 1)
            {
                this.includeSize = reader.ReadInt32();
                this.includeData = reader.ReadUInt64();
                return;
            }
            if (dataFormatVersion == 2)
            {
                this.includeSize = reader.ReadInt32();
                this.includeData = reader.ReadUInt64();
                this.excludeSize = reader.ReadInt32();
                this.excludeData = reader.ReadUInt64();
                return;
            }
            throw new InControlException("Unknown data format version: " + dataFormatVersion);
        }

        internal void Save(BinaryWriter writer)
        {
            writer.Write(this.includeSize);
            writer.Write(this.includeData);
            writer.Write(this.excludeSize);
            writer.Write(this.excludeData);
        }

        private int includeSize;

        private ulong includeData;

        private int excludeSize;

        private ulong excludeData;

        private static Dictionary<ulong, string> cachedStrings = new Dictionary<ulong, string>();
    }
}
