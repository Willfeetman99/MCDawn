/*
	Copyright 2010 MCSharp team (Modified for use with MCZall/MCLawl) Licensed under the
	Educational Community License, Version 2.0 (the "License"); you may
	not use this file except in compliance with the License. You may
	obtain a copy of the License at
	
	http://www.osedu.org/licenses/ECL-2.0
	
	Unless required by applicable law or agreed to in writing,
	software distributed under the License is distributed on an "AS IS"
	BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
	or implied. See the License for the specific language governing
	permissions and limitations under the License.
*/
using System;
using System.IO;

namespace MCDawn
{
    public class CmdMapInfo : Command
    {
        public override string name { get { return "mapinfo"; } }
        public override string[] aliases { get { return new string[] { "status" }; } }
        public override string type { get { return "information"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public CmdMapInfo() { }

        public override void Use(Player p, string message)
        {
            Level foundLevel;

            if (message == "") { foundLevel = p.level; }
            else foundLevel = Level.Find(message);

            if (foundLevel == null) { Player.SendMessage(p, "Could not find specified level."); return; }

            Player.SendMessage(p, "&b" + foundLevel.name + "&g: Width=" + foundLevel.width.ToString() + " Height=" + foundLevel.depth.ToString() + " Depth=" + foundLevel.height.ToString());
            // Level is locked/unlocked
            if (foundLevel.locked == true) { Player.SendMessage(p, "Map is &cLocked"); }
            else { Player.SendMessage(p, "Map is &2Unlocked"); }
            // Level has guns allowed
            if (foundLevel.allowguns == true) { Player.SendMessage(p, "Guns are &benabled"); }
            else { Player.SendMessage(p, "Guns are &bdisabled"); }

            switch (foundLevel.physics)
            {
                case 0: Player.SendMessage(p, "Physics are &cOFF" + "&g on &b" + foundLevel.name); break;
                case 1: Player.SendMessage(p, "Physics are &aNormal" + "&g on &b" + foundLevel.name); break;
                case 2: Player.SendMessage(p, "Physics are &aAdvanced" + "&g on &b" + foundLevel.name); break;
                case 3: Player.SendMessage(p, "Physics are &aHardcore" + "&g on &b" + foundLevel.name); break;
                case 4: Player.SendMessage(p, "Physics are &aInstant" + "&g on &b" + foundLevel.name); break;
                case 5: Player.SendMessage(p, "Physics are &aDoors Only" + "&g on &b" + foundLevel.name); break;
            }

            try
            {
                Player.SendMessage(p, "Build rank = " + Group.findPerm(foundLevel.permissionbuild).color + Group.findPerm(foundLevel.permissionbuild).trueName);
                Player.SendMessage(p, "Visit rank = " + Group.findPerm(foundLevel.permissionvisit).color + Group.findPerm(foundLevel.permissionvisit).trueName);
            } catch (Exception e) { Server.ErrorLog(e); }

            try { Player.SendMessage(p, "SpeedHack Rank: " + foundLevel.speedHackRank.color + foundLevel.speedHackRank.trueName); }
            catch { Player.SendMessage(p, "SpeedHack Rank: " + Group.findPerm(LevelPermission.Banned).color + Group.findPerm(LevelPermission.Banned).trueName); }

            if (Directory.Exists(@Server.backupLocation + "/" + foundLevel.name))
            {
                int latestBackup = Directory.GetDirectories(@Server.backupLocation + "/" + foundLevel.name).Length;
                Player.SendMessage(p, "Latest backup: &a" + latestBackup + "&g at &a" + Directory.GetCreationTime(@Server.backupLocation + "/" + foundLevel.name + "/" + latestBackup).ToString("yyyy-MM-dd HH:mm:ss")); // + Directory.GetCreationTime(@Server.backupLocation + "/" + latestBackup + "/").ToString("yyyy-MM-dd HH:mm:ss"));
            }
            else
            {
                Player.SendMessage(p, "No backups for this map exist yet.");
            }
        }
        public override void Help(Player p)
        {
            Player.SendMessage(p, "/mapinfo <map> - Display details of <map>");
        }
    }
}