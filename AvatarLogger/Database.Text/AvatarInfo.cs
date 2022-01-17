using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarLogger.Database.Text
{
    public class AvatarInfo : IAvatarInfo
    {
        public string Id { get; set; }
        public string Name { get ; set; }
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public IDatabase OwningDatabase { get; set; }
        public string ReleaseStatus { get; set; }
        public string PCAssetURL { get; set; }
        public string QuestAssetURL { get; set; }
        public string ImageURL { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public string UnityVersion { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public DateTime TimeStamp { get; set; } = DateTime.Now;
    }
}
