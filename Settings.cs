using System.IO;
using System.Reflection;
using ModSettings;
using UnityEngine;

namespace RestMax
{
    internal class SettingsMain : JsonModSettings
    {
        protected override void OnConfirm()
        {
            base.OnConfirm();
        }

        protected override void OnChange(FieldInfo field, object oldValue, object newValue)
        {
            if (field.Name == nameof(EnableMod) ||
                field.Name == nameof(SkipMenu) ||
                field.Name == nameof(SkipBedRoll) ||
                field.Name == nameof(InvertCrouch))
            {
                Settings.RefreshFields();
            }
        }



        [Section("General")]

        [Name("Enable Mod")]
        [Description("Always Sleep until Fully Rested.")]
        public bool EnableMod = true;

        [Name("Skip Rest Menu")]
        [Description("Sleep Immediately by Selecting Bed")]
        public bool SkipMenu = false;

        [Name("Skip Rest Menu for Bedrolls")]
        [Description("Sleep Immediately at Bedrolls and Beds" + "(if enabled you can still pick up a bedroll by crouching)")]
        public bool SkipBedRoll = false;

        [Name("Invert Bedroll Crouch Interaction")]
        [Description("Must be Crouched to Sleep in Bedroll")]
        public bool InvertCrouch = false;

    }


    internal static class Settings
    {
        public static SettingsMain options;

        public static void OnLoad()
        {
            options = new SettingsMain();
            options.AddToModSettings("Always Sleep Maximum");
            Settings.RefreshFields();
        }

        public static void RefreshFields()
        {
            options.SetFieldVisible("Skip Rest Menu", Settings.options.EnableMod == true);
            options.SetFieldVisible("Skip Rest Menu for Bedrolls", Settings.options.SkipMenu == true);
            options.SetFieldVisible("Invert Bedroll Crouch Interaction", Settings.options.SkipBedRoll == true);
        }
    }


}
