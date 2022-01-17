using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarLogger.Database
{
    public interface IAvatarInfo
    {
        IDatabase OwningDatabase { get; set; }
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        string AuthorId { get; set; }
        string AuthorName { get; set; }
        string ReleaseStatus { get; set; }
        string PCAssetURL { get; set; }
        string QuestAssetURL { get; set; }
        string ImageURL { get; set; }
        string ThumbnailImageUrl { get; set; }
        string UnityVersion { get; set; }
        List<string> Tags { get; set; }
        DateTime TimeStamp { get; set; }
    }
}
