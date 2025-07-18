package crc6409f1fb5e6ac6cefd;


public class LocationListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.location.LocationListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onLocationChanged:(Landroid/location/Location;)V:GetOnLocationChanged_Landroid_location_Location_Handler:Android.Locations.ILocationListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onProviderDisabled:(Ljava/lang/String;)V:GetOnProviderDisabled_Ljava_lang_String_Handler:Android.Locations.ILocationListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onProviderEnabled:(Ljava/lang/String;)V:GetOnProviderEnabled_Ljava_lang_String_Handler:Android.Locations.ILocationListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onStatusChanged:(Ljava/lang/String;ILandroid/os/Bundle;)V:GetOnStatusChanged_Ljava_lang_String_ILandroid_os_Bundle_Handler:Android.Locations.ILocationListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onFlushComplete:(I)V:GetOnFlushComplete_IHandler:Android.Locations.ILocationListener, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Pro4Soft.MobileDevice.GeoPositioning.LocationListener, P4WHandheld.Android", LocationListener.class, __md_methods);
	}


	public LocationListener ()
	{
		super ();
		if (getClass () == LocationListener.class) {
			mono.android.TypeManager.Activate ("Pro4Soft.MobileDevice.GeoPositioning.LocationListener, P4WHandheld.Android", "", this, new java.lang.Object[] {  });
		}
	}

	public LocationListener (android.location.LocationManager p0, java.lang.String p1)
	{
		super ();
		if (getClass () == LocationListener.class) {
			mono.android.TypeManager.Activate ("Pro4Soft.MobileDevice.GeoPositioning.LocationListener, P4WHandheld.Android", "Android.Locations.LocationManager, Mono.Android:System.String, mscorlib", this, new java.lang.Object[] { p0, p1 });
		}
	}


	public void onLocationChanged (android.location.Location p0)
	{
		n_onLocationChanged (p0);
	}

	private native void n_onLocationChanged (android.location.Location p0);


	public void onProviderDisabled (java.lang.String p0)
	{
		n_onProviderDisabled (p0);
	}

	private native void n_onProviderDisabled (java.lang.String p0);


	public void onProviderEnabled (java.lang.String p0)
	{
		n_onProviderEnabled (p0);
	}

	private native void n_onProviderEnabled (java.lang.String p0);


	public void onStatusChanged (java.lang.String p0, int p1, android.os.Bundle p2)
	{
		n_onStatusChanged (p0, p1, p2);
	}

	private native void n_onStatusChanged (java.lang.String p0, int p1, android.os.Bundle p2);


	public void onFlushComplete (int p0)
	{
		n_onFlushComplete (p0);
	}

	private native void n_onFlushComplete (int p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
