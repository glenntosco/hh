using System;
using Android.Content;
using Android.OS;

namespace Pro4Soft.MobileDevice
{
    [BroadcastReceiver]
    public class DataWedgeReceiver : BroadcastReceiver
    {
        public static string IntentAction = "com.pro4soft.p4w.scan";

        private const string DataStringTag = "com.motorolasolutions.emdk.datawedge.data_string";
        public event EventHandler<string> ScanDataReceived;
        public override void OnReceive(Context context, Intent intent)
        {
            if (!intent.Action.Equals(IntentAction))
                return;
            
            ScanDataReceived?.Invoke(this, intent.GetStringExtra(DataStringTag));
        }

        //Static - used for setup
        private const string ActionDatawedgeFrom62 = "com.symbol.datawedge.api.ACTION";
        private const string ExtraCreateProfileCommand = "com.symbol.datawedge.api.CREATE_PROFILE";
        private const string SetConfigCommand = "com.symbol.datawedge.api.SET_CONFIG";
        public static string SwitchToProfileCommand = "com.symbol.datawedge.api.SWITCH_TO_PROFILE";
        private const string ProfileName = "P4W Handheld";

        public static void CreateProfile(Context activity)
        {
            SendDataWedgeIntentWithExtra(ActionDatawedgeFrom62, ExtraCreateProfileCommand, ProfileName, activity);

            //  Now configure that created profile to apply to our application
            var profileConfig = new Bundle();
            profileConfig.PutString("PROFILE_NAME", ProfileName);
            profileConfig.PutString("PROFILE_ENABLED", "true");
            profileConfig.PutString("CONFIG_MODE", "UPDATE");

            var barcodeConfig = new Bundle();
            barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
            barcodeConfig.PutString("RESET_CONFIG", "true");

            var barcodeProps = new Bundle();
            barcodeProps.PutString("scanner_input_enabled", "true");
            barcodeProps.PutString("scanner_selection", "auto");
            barcodeProps.PutString("decoder_ean8", "true");
            barcodeProps.PutString("decoder_ean13", "true");
            barcodeProps.PutString("decoder_code39", "true");
            barcodeProps.PutString("decoder_code128", "true");
            barcodeProps.PutString("decoder_upca", "true");
            barcodeProps.PutString("decoder_upca_preamble", "2");//https://techdocs.zebra.com/datawedge/latest/guide/decoders/
            barcodeProps.PutString("decoder_upce0", "true");
            barcodeProps.PutString("decoder_upce1", "true");
            barcodeProps.PutString("decoder_d2of5", "true");
            barcodeProps.PutString("decoder_i2of5", "true");
            barcodeProps.PutString("decoder_aztec", "true");
            barcodeProps.PutString("decoder_pdf417", "true");
            barcodeProps.PutString("decoder_qrcode", "true");

            barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
            profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);

            var appConfig = new Bundle();
            appConfig.PutString("PACKAGE_NAME", activity.PackageName);      //  Associate the profile with this app
            appConfig.PutStringArray("ACTIVITY_LIST", new[] { "*" });
            profileConfig.PutParcelableArray("APP_LIST", new IParcelable[] { appConfig });
            SendDataWedgeIntentWithExtra(ActionDatawedgeFrom62, SetConfigCommand, profileConfig, activity);

            //  You can only configure one plugin at a time, we have done the barcode input, now do the intent output
            profileConfig.Remove("PLUGIN_CONFIG");

            var intentProps = new Bundle();
            intentProps.PutString("intent_output_enabled", "true");
            intentProps.PutString("intent_action", IntentAction);
            //intentProps.PutString("intent_category", Intent.CategoryDefault);
            intentProps.PutString("intent_delivery", "2");

            var intentConfig = new Bundle();
            intentConfig.PutString("PLUGIN_NAME", "INTENT");
            intentConfig.PutString("RESET_CONFIG", "true");
            intentConfig.PutBundle("PARAM_LIST", intentProps);

            profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);

            SendDataWedgeIntentWithExtra(ActionDatawedgeFrom62, SetConfigCommand, profileConfig, activity);

            profileConfig.Remove("PLUGIN_CONFIG");

            var keyboardProps = new Bundle();
            keyboardProps.PutString("keystroke_output_enabled", "false");
            
            var keyboardConfig = new Bundle();
            keyboardConfig.PutString("PLUGIN_NAME", "KEYSTROKE");
            keyboardConfig.PutString("RESET_CONFIG", "true");
            keyboardConfig.PutBundle("PARAM_LIST", keyboardProps);

            profileConfig.PutBundle("PLUGIN_CONFIG", keyboardConfig);

            SendDataWedgeIntentWithExtra(ActionDatawedgeFrom62, SetConfigCommand, profileConfig, activity);
            SendDataWedgeIntentWithExtra(ActionDatawedgeFrom62, SwitchToProfileCommand, ProfileName, activity);
        }

        private static void SendDataWedgeIntentWithExtra(string action, string extraKey, Bundle extras, Context activity)
        {
            var dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extras);
            activity.SendBroadcast(dwIntent);
        }

        private static void SendDataWedgeIntentWithExtra(string action, string extraKey, string extraValue, Context activity)
        {
            var dwIntent = new Intent();
            dwIntent.SetAction(action);
            dwIntent.PutExtra(extraKey, extraValue);
            activity.SendBroadcast(dwIntent);
        }
    }
}