using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace InControl
{
    public struct VersionInfo : IComparable<VersionInfo>
    {
        public VersionInfo(int major, int minor, int patch, int build)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.Build = build;
        }

        public static VersionInfo InControlVersion()
        {
            return new VersionInfo
            {
                Major = 1,
                Minor = 6,
                Patch = 16,
                Build = 9119
            };
        }

        public static VersionInfo UnityVersion()
        {
            Match match = Regex.Match(Application.unityVersion, "^(\\d+)\\.(\\d+)\\.(\\d+)");
            int build = 0;
            return new VersionInfo
            {
                Major = Convert.ToInt32(match.Groups[1].Value),
                Minor = Convert.ToInt32(match.Groups[2].Value),
                Patch = Convert.ToInt32(match.Groups[3].Value),
                Build = build
            };
        }

        public static VersionInfo Min
        {
            get
            {
                return new VersionInfo(int.MinValue, int.MinValue, int.MinValue, int.MinValue);
            }
        }

        public static VersionInfo Max
        {
            get
            {
                return new VersionInfo(int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue);
            }
        }

        public int CompareTo(VersionInfo other)
        {
            if (this.Major < other.Major)
            {
                return -1;
            }
            if (this.Major > other.Major)
            {
                return 1;
            }
            if (this.Minor < other.Minor)
            {
                return -1;
            }
            if (this.Minor > other.Minor)
            {
                return 1;
            }
            if (this.Patch < other.Patch)
            {
                return -1;
            }
            if (this.Patch > other.Patch)
            {
                return 1;
            }
            if (this.Build < other.Build)
            {
                return -1;
            }
            if (this.Build > other.Build)
            {
                return 1;
            }
            return 0;
        }

        public static bool operator ==(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) != 0;
        }

        public static bool operator <=(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator >=(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >(VersionInfo a, VersionInfo b)
        {
            return a.CompareTo(b) > 0;
        }

        public override bool Equals(object other)
        {
            return other is VersionInfo && this == (VersionInfo)other;
        }

        public override int GetHashCode()
        {
            return this.Major.GetHashCode() ^ this.Minor.GetHashCode() ^ this.Patch.GetHashCode() ^ this.Build.GetHashCode();
        }

        public override string ToString()
        {
            if (this.Build == 0)
            {
                return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
            }
            return string.Format("{0}.{1}.{2} build {3}", new object[]
            {
                this.Major,
                this.Minor,
                this.Patch,
                this.Build
            });
        }

        public string ToShortString()
        {
            if (this.Build == 0)
            {
                return string.Format("{0}.{1}.{2}", this.Major, this.Minor, this.Patch);
            }
            return string.Format("{0}.{1}.{2}b{3}", new object[]
            {
                this.Major,
                this.Minor,
                this.Patch,
                this.Build
            });
        }

        public int Major;

        public int Minor;

        public int Patch;

        public int Build;
    }
}
