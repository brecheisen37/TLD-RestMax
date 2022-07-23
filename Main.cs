using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using MelonLoader;
using ModSettings;
using UnityEngine;

namespace RestMax
{

    public class Implementation : MelonMod
    {
        public override void OnApplicationLateStart()
        {
            MelonDebug.Msg($"[{Info.Name}] Version {Info.Version} loaded!");

        }

        public override void OnApplicationStart()
        {
            Settings.OnLoad();
        }

        int hourstosleep = 1;
        void SleeporWait()
        {
            if (hourstosleep > 1)
            {
                InterfaceManager.m_Panel_Rest.DoRest(hourstosleep, true);
            }
            else
            {
                InterfaceManager.m_Panel_Rest.DoPassTime(1);
                InterfaceManager.m_Panel_Rest.Enable(false);
            }
        }

        bool Panel_Opening = false;
        bool Panel_Enabled = false;
        bool Panel_Enabled_Last = false;
        public override void OnUpdate()
        {
            if (InterfaceManager.m_Panel_Rest != null)
            {
                Panel_Enabled = InterfaceManager.m_Panel_Rest.IsEnabled();
                if (Panel_Enabled && !Panel_Enabled_Last) Panel_Opening = true;
                if (Settings.options.EnableMod && Panel_Opening)
                {
                    float fatigueamount = GameManager.GetFatigueComponent().GetNormalizedFatigue();
                    hourstosleep = Mathf.CeilToInt(12 * fatigueamount);
                    if (!InterfaceManager.m_Panel_Rest.m_PassTimeOnlyObject.active) InterfaceManager.m_Panel_Rest.m_SleepHours = hourstosleep;
                    if (Settings.options.SkipMenu)
                    {
                        if (InterfaceManager.m_Panel_Rest.m_Bed.m_Bedroll == null) SleeporWait();
                        else
                        {
                            if (Settings.options.SkipBedRoll)
                            {
                                bool PlayerisCrouched = GameManager.GetPlayerManagerComponent().PlayerIsCrouched();
                                if (Settings.options.InvertCrouch) PlayerisCrouched = !PlayerisCrouched;
                                if (PlayerisCrouched)
                                    InterfaceManager.m_Panel_Rest.OnPickUp();
                                else
                                    SleeporWait();
                            }

                        }
                    }
                    Panel_Opening = false;
                }
                Panel_Enabled_Last = Panel_Enabled;
            }
            
        }



    }
}






