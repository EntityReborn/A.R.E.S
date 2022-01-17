using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarLogger.Database
{
    public interface IDatabase
    {
        void LoadOptions(dynamic options);
        void Load();
        bool Save();

        bool HasAvatarId(string id);
        IAvatarInfo NewAvatar();
        IAvatarInfo SaveAvatar(IAvatarInfo info);
    }
}
