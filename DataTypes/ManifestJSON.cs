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

    public class AssestInfo
    {
        public int key;
        public int type;
        public string Name;
        public string Hash;
        public int crc;
        public int size;
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
