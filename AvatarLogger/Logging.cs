//Importing reqired modules
using VRC.Core;
using MelonLoader;
using System.IO;
using System;
using System.Text;
using static AvatarLogger.Main;
using AvatarLogger.Database;
//Contains code responsible for the actual logging of avatars themsleves
namespace Logging
{
    internal static class Logging
    {
        //Make string to contain friend avatars
        public static string FriendIDs = null;
        //Fetches the frend IDs on the ARES user
        public static System.Collections.IEnumerator FetchFriends()
        {
            while (RoomManager.field_Internal_Static_ApiWorld_0 == null) { yield return null; }
            string[] pals = APIUser.CurrentUser.friendIDs.ToArray();
            foreach (string pal in pals) { FriendIDs += $"{pal},"; }
        }
        //Executes logging of the avatar
        public static void ExecuteLog(dynamic playerHashtable)
        {
            var aviDict = playerHashtable["avatarDict"];

            //Get database handler and get ready to throw avis in it.
            var db = DatabaseManager.GetDatabase();
            if (db == null)
            {
                //Shouldn't happen, but apparently we didn't find a database to log to
                if (Config.ConsoleError)
                {
                    MelonLogger.Msg($"{aviDict["name"].ToString()} was not logged, there's no database loaded!");
                    
                }
                return;
            }

            //If avatar loggin is enabled
            if (Config.LogAvatars)
            {
                //If logging of public avatars is disabled
                if (!Config.LogPublicAvatars)
                {
                    //Check to see if the avatar is public and refuse to log if so
                    if (aviDict["releaseStatus"].ToString() == "public") { return; }
                }
                //If logging of private avatars is disabled
                if (!Config.LogPrivateAvatars)
                {
                    //Check to see if the avatar is private and refuse to log if so
                    if (aviDict["releaseStatus"].ToString() == "private") { return; }
                }
                //If logging own avatars is disabled
                if (!Config.LogOwnAvatars)
                {
                    //Check if the avatar about to be uploaded belongs to the user and was uploaded from their account
                    if (APIUser.CurrentUser.id == aviDict["authorId"].ToString())
                    {
                        //If the avatar was uploaded by the user inform them the avatr was not logged and why it was not logged
                        if (Config.ConsoleError) { MelonLogger.Msg($"Your avatar {aviDict["name"].ToString()} was not logged, you have log own avatars disabled!"); }
                        return;
                    }
                }
                //If logging of friends avatars is disabled
                if (!Config.LogFriendsAvatars)
                {
                    //Check if the avatar about to be logged is uploaded by a friend
                    if (FriendIDs.Contains(aviDict["authorId"].ToString()))
                    {
                        //If the user is a friend inform the user the log has not occurred and why so
                        if (Config.ConsoleError) { MelonLogger.Msg($"{aviDict["authorName"].ToString()}'s avatar {aviDict["name"].ToString()} was not logged, they are a friend!"); }
                        return;
                    }
                }

                //If the hash table passed into the method contains a new avatar ID that is not already present within the log file
                if (!db.HasAvatarId(aviDict["id"].ToString()))
                {
                    //Ask DB to create a new avatar instance
                    var avi = db.NewAvatar();

                    //Base avi information
                    avi.Id = aviDict["id"].ToString();
                    avi.Name = aviDict["name"].ToString();
                    avi.Description = aviDict["description"].ToString();
                    avi.AuthorId = aviDict["authorId"].ToString();
                    avi.AuthorName = aviDict["authorName"].ToString();
                    avi.ImageURL = aviDict["imageUrl"].ToString();
                    avi.ThumbnailImageUrl = aviDict["thumbnailImageUrl"].ToString();
                    avi.ReleaseStatus = aviDict["releaseStatus"].ToString();

                    //New optimised Quest/PC asset URL logging 
                    string pcasset = "None";
                    string qasset = "None";

                    //Find what packages are exposed by this avatar
                    foreach (dynamic unitypackage in aviDict["unityPackages"])
                    {
                        try
                        {
                            switch (unitypackage["platform"].ToString())
                            {
                                //Checks for avi version and logs accordingly for Quest and PC
                                case "standalonewindows":
                                    if (pcasset == "None")
                                    {
                                        pcasset = unitypackage["assetUrl"].ToString();
                                        PC = PC + 1;
                                    }
                                    break;
                                case "android":
                                    if (qasset == "None")
                                    {
                                        qasset = unitypackage["assetUrl"].ToString();
                                        Q = Q + 1;
                                    }
                                    break;
                            }
                        }
                        catch { }
                    }

                    avi.PCAssetURL = pcasset;
                    avi.QuestAssetURL = qasset;
                    avi.UnityVersion = aviDict["unityPackages"][0]["unityVersion"].ToString();

                    //Adjust counter values to whatever the avatars release status is
                    if (avi.ReleaseStatus == "public") { Pub = Pub + 1; };
                    if (avi.ReleaseStatus == "private") { Pri = Pri = 1; };

                    //The last variables extracted are the tags of the avatar, these are added by the avatar uploader or by VRChat administrators/developers,
                    //they are initally stored as an array, if no tags are set the if statemnt will just continue with its else
                    if (aviDict["tags"].Count > 0)
                    {
                        foreach (string tag in aviDict["tags"]) {
                            avi.Tags.Add(tag);
                        }
                    }
                    //If there are no tags present the default text "Tags: None" is written into the log file
                    else { 
                        avi.Tags.Add("None"); 
                    }

                    //Inform the user of the successful log
                    if (Config.LogToConsole) { 
                        MelonLogger.Msg($"Logged: {avi.Name}|{avi.ReleaseStatus}!"); 
                    };

                    //Tell the database to save the avatar data
                    db.SaveAvatar(avi);
                }
            }
        }
    }
}
