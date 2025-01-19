﻿using Rocket.API;
using Rocket.Core;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayerInfoLibrary
{
    public class PlayerData
    {
        public CSteamID SteamID { get; private set; }
        public string SteamName { get; internal set; }
        public string CharacterName { get; internal set; }
        public string IP { get; internal set; }
        public List<string> HWID { get; internal set; }
        public DateTime LastLoginGlobal { get; internal set; }
        public int TotalPlayime { get; internal set; }
        public ushort LastServerID { get; internal set; }
        public string LastServerName { get; internal set; }
        public ushort ServerID { get; private set; }
        public DateTime LastLoginLocal { get; internal set; }
        public bool CleanedBuildables { get; internal set; }
        public bool CleanedPlayerData { get; internal set; }
        public DateTime CacheTime { get; internal set; }

        /// <summary>
        /// Checks to see if the server specific data stored in this class is from this server(local).
        /// </summary>
        /// <returns>true if the data is from this server.</returns>
        public bool IsLocal()
        {
            if (!IsValid())
                return false;
            return ServerID == PlayerInfoLib.Database.InstanceID;
        }

        /// <summary>
        /// Checks to see if the data is valid.
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return SteamID != CSteamID.Nil;
        }

        /// <summary>
        /// Checks to see if the Cache time on this data has expired.
        /// </summary>
        /// <returns></returns>
        public bool IsCacheExpired()
        {
            if (CacheTime != null)
                return ((DateTime.Now - CacheTime).TotalSeconds >= (PlayerInfoLib.Instance.Configuration.Instance.CacheTime * 60));
            return true;
        }

        /// <summary>
        /// Checks players groups against the config value set in the plugin config to see if they're a VIP.
        /// </summary>
        /// <returns>true if a match is found.</returns>
        public bool IsVip()
        {
            if (!IsValid())
                return false;
            return R.Permissions.GetGroups(new RocketPlayer(SteamID.ToString()), true).FirstOrDefault(g => g.Id == PlayerInfoLib.Instance.Configuration.Instance.VipCheckGroupName) != null;
        }

        internal PlayerData()
        {
            SteamID = CSteamID.Nil;
            TotalPlayime = 0;
        }
        internal PlayerData(CSteamID steamID, string steamName, string characterName, string ip, List<string> Hwid, DateTime lastLoginGlobal, ushort lastServerID, string lastServerName, ushort serverID, DateTime lastLoginLocal, bool cleanedBuildables, bool cleanedPlayerData, int totalPlayTime)
        {
            SteamID = steamID;
            SteamName = steamName;
            CharacterName = characterName;
            IP = ip;
            HWID = Hwid;
            LastLoginGlobal = lastLoginGlobal;
            LastServerID = lastServerID;
            LastServerName = lastServerName;
            ServerID = serverID;
            LastLoginLocal = lastLoginLocal;
            CleanedBuildables = cleanedBuildables;
            CleanedPlayerData = cleanedPlayerData;
            TotalPlayime = totalPlayTime;
        }
    }
}