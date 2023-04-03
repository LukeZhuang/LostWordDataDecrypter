using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LostWordDataDecrypter.DataTypes
{
    public class ManifestJSON
    {
        public List<AssestInfo> AssetInfos;
        public List<ResourceInfo> ResourceInfos;
        public List<Unit> Units;
        public List<Picture> Pictures;
    }

    public class ResourceInfo
    {
        public long EndAt;
        public string Name;
    }

    public class Unit
    {
        public long EndAt;
        public int Id;
    }

    public class Picture
    {

    }

    public class AssestInfo
    {
        public int key;
        public int type;
        public string Name { get; set; }
        public string Hash;
        public long crc;
        public long size;
        public List<string> AssetPaths;
        public List<string> DownloadBundles;

        public bool IsMasterData()
        {
            if (AssetPaths.Count != 1)
            {
                return false;
            }
            string pattern = "Assets/East/Master/.*Master[.]bytes";
            foreach (string s in AssetPaths)
            {
                Match m = Regex.Match(s, pattern, RegexOptions.None);
                if (m.Success)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetTableName()
        {
            /* must call IsMasterData first */
            string path = AssetPaths[0];
            return path.Substring(19, path.Length - 31) + "Table";
        }
    }
}
