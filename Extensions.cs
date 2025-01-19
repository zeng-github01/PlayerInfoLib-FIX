﻿using MySql.Data.MySqlClient;
using SDG.Unturned;
using Steamworks;
using System;
//using SDG.NetTransport;
using Rocket.Unturned.Player;
using System.Text;
using System.Collections.Generic;

namespace PlayerInfoLibrary
{
    public static class Extensions
    {
        public static DateTime FromTimeStamp(this long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp).ToLocalTime();
        }

        public static long ToTimeStamp(this DateTime datetime)
        {
            return (long)(datetime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds);
        }

        public static bool IsDBNull (this MySqlDataReader reader, string fieldname)
        {
            return reader.IsDBNull(reader.GetOrdinal(fieldname));
        }

        public static string GetIP(this CSteamID cSteamID)
        {
            // Grab an active players ip address from CSteamID.
            //P2PSessionState_t sessionState;
            //SteamGameServerNetworking.GetP2PSessionState(cSteamID, out sessionState);
            //return Parser.getIPFromUInt32(sessionState.m_nRemoteIP);
            //SteamGameServerNetworking.GetP2PSessionState(cSteamID, out P2PSessionState_t p2PSessionState_T);
            var player = UnturnedPlayer.FromCSteamID(cSteamID);
            var IP = player.Player.channel.owner.getIPv4AddressOrZero();
            return Parser.getIPFromUInt32(IP);
        }
        public static List<string> GetHWID(this CSteamID cSteamID)
        {
            List<string> hwidList = new List<string>();
            var player = UnturnedPlayer.FromCSteamID(cSteamID);
            var hwid = player.Player.channel.owner.playerID.GetHwids().GetEnumerator();
            while (hwid.MoveNext())
            {
                hwidList.Add(Convert.ToBase64String(hwid.Current));
            }

            return hwidList;
        }

        // Returns a Steamworks.CSteamID on out from a string, and returns true if it is a CSteamID.
        public static bool isCSteamID(this string sCSteamID, out CSteamID cSteamID)
        {
            ulong ulCSteamID;
            cSteamID = (CSteamID)0;
            if (ulong.TryParse(sCSteamID, out ulCSteamID))
            {
                if ((ulCSteamID >= 0x0110000100000000 && ulCSteamID <= 0x0170000000000000) || ulCSteamID == 0)
                {
                    cSteamID = (CSteamID)ulCSteamID;
                    return true;
                }
            }
            return false;
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        // Returns formatted string with how long they've played on the server in d, h, m, s.
        public static string FormatTotalTime(this int totalTime)
        {
            string totalTimeFormated = "";
            if (totalTime >= (60 * 60 * 24))
            {
                totalTimeFormated = ((int)(totalTime / (60 * 60 * 24))).ToString() + "d ";
            }
            if (totalTime >= (60 * 60))
            {
                totalTimeFormated += ((int)((totalTime / (60 * 60)) % 24)).ToString() + "h ";
            }
            if (totalTime >= 60)
            {
                totalTimeFormated += ((int)((totalTime / 60) % 60)).ToString() + "m ";
            }
            totalTimeFormated += ((int)(totalTime % 60)).ToString() + "s";
            return totalTimeFormated;
        }
    }
}
