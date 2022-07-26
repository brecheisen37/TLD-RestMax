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
                field.Name == nameof(ForceMod) ||
                field.Name == nameof(SkipMenu) ||
                field.Name == nameof(SkipBedRoll) ||
                field.Name == nameof(InvertCrouch))
            {
                RefreshFields();
            }
        }
        public void RefreshFields()
        {
            SetFieldVisible(nameof(ForceMod), Settings.options.EnableMod);
            SetFieldVisible(nameof(SkipMenu), Settings.options.EnableMod && Settings.options.ForceMod);
            SetFieldVisible(nameof(SkipBedRoll), Settings.options.EnableMod && Settings.options.ForceMod && Settings.options.SkipMenu);
            SetFieldVisible(nameof(InvertCrouch), Settings.options.EnableMod && Settings.options.ForceMod && Settings.options.SkipMenu && Settings.options.SkipBedRoll);
        }



        [Section("General")]

        [Name("Enable Mod")]
        [Description("Set Sleep Time to the Time needed to be Fully Rested.")]
        public bool EnableMod = true;

        [Name("Enforce Duration")]
        [Description("Always Sleep until Fully Rested, without the option to sleep less time. Rest as resource prevents sleeping longer.")]
        public bool ForceMod = true;

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
            Settings.options.RefreshFields();
        }

    }


}
