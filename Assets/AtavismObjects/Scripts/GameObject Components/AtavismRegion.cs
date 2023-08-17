using UnityEngine;
using System.Collections;

namespace Atavism
{
    public enum AtavismRegionType
    {
        Water,
        Dismount,
        Teleport,
        ApplyEffect,
        StartQuest,
        CompleteTask,
        PvP,
        Sanctuary,
    }

    public class AtavismRegion : MonoBehaviour
    {
        /// <summary>
        /// P4 Development Studio
        /// </summary>
        public bool lastTargetInSameGuild;

        public int id;
        public AtavismRegionType regionType;
        public int actionID = -1;
        public string actionData1;
        public string actionData2;
        public string actionData3;


        private void OnTriggerEnter(Collider other)
        {
            void CheckGuildPvpState()
            {
                long playerOidValue = ClientAPI.GetPlayerOid();
                long targetOidValue = ClientAPI.GetTargetOid();

                if (playerOidValue != 0 && targetOidValue != 0 && targetOidValue != playerOidValue)
                {
                    OID playerOid = OID.fromLong(playerOidValue);
                    OID targetOid = OID.fromLong(targetOidValue);

                    AtavismGuild guild = AtavismGuild.Instance;
                    if (guild == null)
                    {
                        Debug.LogError("Guild instance is null");
                        return;
                    }

                    AtavismGuildMember playerMember = guild.GetGuildMemberByOid(playerOid);
                    AtavismGuildMember targetMember = guild.GetGuildMemberByOid(targetOid);

                    if (playerMember == null || targetMember == null)
                    {
                        Debug.LogError("One or both players are not guild members");
                        return;
                    }

                    string playerGuildName = ClientAPI.GetPlayerObject().GetProperty("guildName").ToString();
                    string targetGuildName = ClientAPI.GetTargetObject().GetProperty("guildName").ToString();

                    AtavismRegion pvpRegion = FindObjectOfType<AtavismRegion>();

                    if (playerGuildName != null && playerGuildName == targetGuildName)
                    {
                        Debug.Log("Desligou o PVP :DDD" + " eles são da mesma guild " + targetGuildName);

                        if (pvpRegion != null && pvpRegion.regionType == AtavismRegionType.PvP)
                        {
                            pvpRegion.regionType = AtavismRegionType.Sanctuary;
                            lastTargetInSameGuild = true;
                        }
                    }
                    else if (lastTargetInSameGuild)
                    {
                        if (pvpRegion != null && pvpRegion.regionType == AtavismRegionType.Sanctuary)
                        {
                            pvpRegion.regionType = AtavismRegionType.PvP;
                        }
                        lastTargetInSameGuild = false;
                    }
                }
            }
        }
       

        /*   void OnTriggerEnter(Collider other)
           {
               if (regionType == AtavismRegionType.Water)
               {
                   if (other.GetComponent<AtavismNode>() != null)
                   {
                       long oid = other.GetComponent<AtavismNode>().Oid;
                       if (oid > 0)
                       {
                           ClientAPI.GetObjectNode(oid).MobController.WaterRegionEntered(id);
                       }
                   }
               }



           }
       // tried using this and sentStrike, false as well
           void OnTriggerExit(Collider other)
           {
               if (regionType == AtavismRegionType.Water)
               {
                   if (other.GetComponent<AtavismNode>() != null)
                   {
                       long oid = other.GetComponent<AtavismNode>().Oid;
                       if (oid > 0)
                       {
                           ClientAPI.GetObjectNode(oid).MobController.WaterRegionLeft(id);
                       }
                   }
               }
           }*/


    }
}