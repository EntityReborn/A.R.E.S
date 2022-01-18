using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarLogger.Database.Text
{
    public class TextDatabase : IDatabase
    {
        private string AvatarFile { get; set; } = "GUI\\Log.txt";

        bool IDatabase.HasAvatarId(string id)
        {
            var lines = File.ReadLines(AvatarFile);
            foreach (var line in lines)
            {
                if (line.Contains(id)) return true;
            }

            return false;
        }

        void IDatabase.Load()
        {
            if (!File.Exists(AvatarFile))
            { File.AppendAllText(AvatarFile, "Mod By LargestBoi & Yui\n"); }
        }

        void IDatabase.LoadOptions(dynamic options)
        {
            return;
        }

        IAvatarInfo IDatabase.NewAvatar()
        {
            return new AvatarInfo()
            {
                OwningDatabase = this
            };
        }

        bool IDatabase.Save()
        {
            // Nothing to do
            return true;
        }

        IAvatarInfo IDatabase.SaveAvatar(IAvatarInfo info)
        {
            //Log the following variables to the log file
            File.AppendAllLines(AvatarFile, new string[]
            {
                //Obtains the cuttent system date/time in unix and logs it as the time the avatar was detected
                $"Time Detected:{((DateTimeOffset)info.TimeStamp).ToUnixTimeSeconds().ToString()}",
                //Continues to extract more data from the hash table and write it to the log file such as:
                //Avatar ID, Name, Description, Author ID, Author Name and the PC Asset URL
                $"Avatar ID:{info.Id}",
                $"Avatar Name:{info.Name}",
                $"Avatar Description:{info.Description}",
                $"Author ID:{info.AuthorId}",
                $"Author Name:{info.AuthorName}",
                $"PC Asset URL:{info.PCAssetURL}",
                $"Quest Asset URL:{info.QuestAssetURL}",
                $"Image URL:{info.ImageURL}",
                $"Thumbnail URL:{info.ThumbnailImageUrl}",
                $"Unity Version:{info.UnityVersion}",
                $"Release Status:{info.ReleaseStatus}",
            });

            if (info.Tags.Count > 0)
            {
                //Prepares to create a string from the array of tags
                StringBuilder builder = new StringBuilder();
                //Adds the text "Tags: " to the string being created as an identifer
                builder.Append("Tags: ");
                //For every value in the tags array add it to the string being created
                foreach (string tag in info.Tags)
                {
                    builder.Append($"{tag},");
                }
                //Write the final created string into the log file containing all extracted and sorted tags
                File.AppendAllText(AvatarFile, builder.ToString().Remove(builder.ToString().LastIndexOf(",")));
            }

            File.AppendAllText(AvatarFile, "\n\n");

            return info;
        }
    }
}
