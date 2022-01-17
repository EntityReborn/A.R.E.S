using AvatarLogger.Database.Text;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AvatarLogger.Database
{
    public class DatabaseManager
    {
        private static IDatabase Database = null;

        private DatabaseManager() { 
            //Lazy singleton
        }

        public static void Startup()
        {
            //try
            //{
            //    //Load database extensions and find out what types they expose.
            //    Assembly assembly = Assembly.LoadFrom($"{MelonUtils.UserDataDirectory}\\A.R.E.S\\TextDatabase.dll");
            //    Type[] types = assembly.GetTypes();

            //    foreach (var type in types)
            //    {
            //        //Only instantiate types in the loaded DLL if they implement IDatabase
            //        if (typeof(IDatabase).IsAssignableFrom(type))
            //        {
            //            try
            //            {
            //                //Instantiate the database, and load anything it needs.
            //                var db = (IDatabase)Activator.CreateInstance(type); //creates an instance of that class
            //                db.Load();

            //                //Only allow this DB to be used if it properly loads
            //                Database = db;

            //                return;
            //            } catch (Exception ex)
            //            {
            //                //Something went wrong. Lets tell the user.
            //                MelonLogger.Error($"Unable to start database type {type.FullName}: {ex.Message}\r\n{ex.StackTrace}");
            //            }
            //        }
            //    }
            //} catch (Exception ex)
            //{
            //    //Something went wrong. Lets tell the user.
            //    MelonLogger.Error($"Unable to find a proper database to log to!\r\n{ex.Message}\r\n{ex.StackTrace}");
            //}

            //Crappy workaround for not using reflection. Should use something like above to load from a DLL for custom database types!
            Database = new TextDatabase();
            Database.Load();
            
        }

        public static IDatabase GetDatabase()
        {
            return Database;
        }
    }
}
