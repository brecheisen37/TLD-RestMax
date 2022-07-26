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
            if (AllowedtoSleep())
            {
                InterfaceManager.m_Panel_Rest.DoRest(hourstosleep, true);
            }
            else
            {
                InterfaceManager.m_Panel_Rest.DoPassTime(1);
                InterfaceManager.m_Panel_Rest.Enable(false);
            }
        }

        void HandleBed()
        {
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

                };
            }
        }

        bool AllowedtoSleep()
        {
            Rest restcomponent = GameManager.GetRestComponent();
            Fatigue fatiguecomponent = GameManager.GetFatigueComponent();
            bool notnull = restcomponent != null && fatiguecomponent != null && InterfaceManager.m_Panel_Rest != null;
            if (notnull && RestasResource() && fatiguecomponent.m_CurrentFatigue <= InterfaceManager.m_Panel_Rest.m_AllowRestFatigueThreshold && !restcomponent.RestNeededForAffliction())
            {
                return false;
            }
            return notnull;
        }

        bool RestasResource()
        {
            if(GameManager.GetRestComponent() == null) return false;
            else return !GameManager.GetRestComponent().AllowUnlimitedSleep();
        }

        void EnforceHourstoSleep()
        {
            int maxhours;
            if (Settings.options.Limit10) maxhours = 10;
            else maxhours = 12;
            hourstosleep = Mathf.CeilToInt(maxhours * GameManager.GetFatigueComponent().GetNormalizedFatigue());
            if (!InterfaceManager.m_Panel_Rest.m_PassTimeOnlyObject.active) InterfaceManager.m_Panel_Rest.m_SleepHours = hourstosleep;
        }

        bool Panel_Opening = false;
        bool Panel_Enabled = false;
        bool Panel_Enabled_Last = false;

        void UpdatePanel()
        {
            if (Settings.options.EnableMod && InterfaceManager.m_Panel_Rest != null)
            {
                Panel_Enabled = InterfaceManager.m_Panel_Rest.IsEnabled();
                if (Panel_Enabled && !Panel_Enabled_Last) Panel_Opening = true;
                if (Panel_Opening)
                {
                    EnforceHourstoSleep();
                    HandleBed();
                    Panel_Opening = false;
                }
                else 
                if (Settings.options.ForceMod)
                {
                    if(RestasResource()) EnforceHourstoSleep();
                    else if (InterfaceManager.m_Panel_Rest.m_SleepHours < hourstosleep) EnforceHourstoSleep();
                }
                Panel_Enabled_Last = Panel_Enabled;
            }
        }
        public override void OnUpdate()
        {
            UpdatePanel();
        }



    }
}






