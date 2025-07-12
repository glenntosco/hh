; ModuleID = 'obj\Debug\130\android\marshal_methods.x86_64.ll'
source_filename = "obj\Debug\130\android\marshal_methods.x86_64.ll"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 8
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [288 x i64] [
	i64 120698629574877762, ; 0: Mono.Android => 0x1accec39cafe242 => 29
	i64 196720943101637631, ; 1: System.Linq.Expressions.dll => 0x2bae4a7cd73f3ff => 140
	i64 210515253464952879, ; 2: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 74
	i64 232391251801502327, ; 3: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 102
	i64 295915112840604065, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 103
	i64 347331204332682223, ; 5: ImageCircle.Forms.Plugin => 0x4d1f7e3dda87bef => 20
	i64 634308326490598313, ; 6: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 89
	i64 702024105029695270, ; 7: System.Drawing.Common => 0x9be17343c0e7726 => 126
	i64 720058930071658100, ; 8: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x9fe29c82844de74 => 83
	i64 799765834175365804, ; 9: System.ComponentModel.dll => 0xb1956c9f18442ac => 12
	i64 872800313462103108, ; 10: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 80
	i64 940822596282819491, ; 11: System.Transactions => 0xd0e792aa81923a3 => 124
	i64 996343623809489702, ; 12: Xamarin.Forms.Platform => 0xdd3b93f3b63db26 => 114
	i64 1000557547492888992, ; 13: Mono.Security.dll => 0xde2b1c9cba651a0 => 138
	i64 1060858978308751610, ; 14: Azure.Core.dll => 0xeb8ed9ebee080fa => 16
	i64 1120440138749646132, ; 15: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 116
	i64 1315114680217950157, ; 16: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 69
	i64 1342439039765371018, ; 17: Xamarin.Android.Support.Fragment => 0x12a14d31b1d4d88a => 61
	i64 1425944114962822056, ; 18: System.Runtime.Serialization.dll => 0x13c9f89e19eaf3a8 => 3
	i64 1451832606041849089, ; 19: SignaturePad.Forms.dll => 0x1425f21024743d01 => 38
	i64 1507091876539346714, ; 20: Plugin.SimpleAudioPlayer.Abstractions => 0x14ea4413a9012f1a => 34
	i64 1624659445732251991, ; 21: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 67
	i64 1628611045998245443, ; 22: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 91
	i64 1636321030536304333, ; 23: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 84
	i64 1731380447121279447, ; 24: Newtonsoft.Json => 0x18071957e9b889d7 => 31
	i64 1743969030606105336, ; 25: System.Memory.dll => 0x1833d297e88f2af8 => 44
	i64 1795316252682057001, ; 26: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 68
	i64 1836611346387731153, ; 27: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 102
	i64 1865037103900624886, ; 28: Microsoft.Bcl.AsyncInterfaces => 0x19e1f15d56eb87f6 => 26
	i64 1875917498431009007, ; 29: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 65
	i64 1938067011858688285, ; 30: Xamarin.Android.Support.v4.dll => 0x1ae565add0bd691d => 63
	i64 1981742497975770890, ; 31: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 90
	i64 2040001226662520565, ; 32: System.Threading.Tasks.Extensions.dll => 0x1c4f8a4ea894a6f5 => 131
	i64 2133195048986300728, ; 33: Newtonsoft.Json.dll => 0x1d9aa1984b735138 => 31
	i64 2134971073272545971, ; 34: Microsoft.AspNet.SignalR.Client.dll => 0x1da0f0e12c2502b3 => 24
	i64 2136356949452311481, ; 35: Xamarin.AndroidX.MultiDex.dll => 0x1da5dd539d8acbb9 => 95
	i64 2165725771938924357, ; 36: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 72
	i64 2262844636196693701, ; 37: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 80
	i64 2284400282711631002, ; 38: System.Web.Services => 0x1fb3d1f42fd4249a => 130
	i64 2287834202362508563, ; 39: System.Collections.Concurrent => 0x1fc00515e8ce7513 => 5
	i64 2329709569556905518, ; 40: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 87
	i64 2335503487726329082, ; 41: System.Text.Encodings.Web => 0x2069600c4d9d1cfa => 53
	i64 2337758774805907496, ; 42: System.Runtime.CompilerServices.Unsafe => 0x207163383edbc828 => 51
	i64 2470498323731680442, ; 43: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 75
	i64 2479423007379663237, ; 44: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 106
	i64 2497223385847772520, ; 45: System.Runtime => 0x22a7eb7046413568 => 52
	i64 2547086958574651984, ; 46: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 64
	i64 2592350477072141967, ; 47: System.Xml.dll => 0x23f9e10627330e8f => 55
	i64 2624866290265602282, ; 48: mscorlib.dll => 0x246d65fbde2db8ea => 30
	i64 2694427813909235223, ; 49: Xamarin.AndroidX.Preference.dll => 0x256487d230fe0617 => 99
	i64 2783046991838674048, ; 50: System.Runtime.CompilerServices.Unsafe.dll => 0x269f5e7e6dc37c80 => 51
	i64 2960931600190307745, ; 51: Xamarin.Forms.Core => 0x2917579a49927da1 => 112
	i64 3017704767998173186, ; 52: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 116
	i64 3022227708164871115, ; 53: Xamarin.Android.Support.Media.Compat.dll => 0x29f11c168f8293cb => 62
	i64 3289520064315143713, ; 54: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 86
	i64 3303437397778967116, ; 55: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 66
	i64 3311221304742556517, ; 56: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 46
	i64 3522470458906976663, ; 57: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 104
	i64 3531994851595924923, ; 58: System.Numerics => 0x31042a9aade235bb => 45
	i64 3571415421602489686, ; 59: System.Runtime.dll => 0x319037675df7e556 => 52
	i64 3716579019761409177, ; 60: netstandard.dll => 0x3393f0ed5c8c5c99 => 1
	i64 3727469159507183293, ; 61: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 101
	i64 3869649043256705283, ; 62: System.Diagnostics.Tools => 0x35b3c14d74bf0103 => 10
	i64 3966267475168208030, ; 63: System.Memory => 0x370b03412596249e => 44
	i64 4154383907710350974, ; 64: System.ComponentModel => 0x39a7562737acb67e => 12
	i64 4187479170553454871, ; 65: System.Linq.Expressions => 0x3a1cea1e912fa117 => 140
	i64 4255796613242758200, ; 66: zxing.portable => 0x3b0fa078b8a52438 => 121
	i64 4292233171264798357, ; 67: ZXing.Net.Mobile.Core.dll => 0x3b911353fa62fe95 => 118
	i64 4525561845656915374, ; 68: System.ServiceModel.Internals => 0x3ece06856b710dae => 132
	i64 4636684751163556186, ; 69: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 108
	i64 4782108999019072045, ; 70: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0x425d76cc43bb0a2d => 71
	i64 4794310189461587505, ; 71: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 64
	i64 4795410492532947900, ; 72: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 104
	i64 5081566143765835342, ; 73: System.Resources.ResourceManager.dll => 0x4685597c05d06e4e => 4
	i64 5099468265966638712, ; 74: System.Resources.ResourceManager => 0x46c4f35ea8519678 => 4
	i64 5142919913060024034, ; 75: Xamarin.Forms.Platform.Android.dll => 0x475f52699e39bee2 => 113
	i64 5202753749449073649, ; 76: Plugin.Media => 0x4833e4f841be63f1 => 33
	i64 5203618020066742981, ; 77: Xamarin.Essentials => 0x4836f704f0e652c5 => 111
	i64 5205316157927637098, ; 78: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 93
	i64 5233983725610684227, ; 79: FastAndroidCamera => 0x48a2d877b5334f43 => 18
	i64 5334137958787756892, ; 80: System.Reactive.Linq.dll => 0x4a06aa364878b35c => 49
	i64 5336705035019254128, ; 81: Matcha.BackgroundService.Droid => 0x4a0fc8f44b5c7970 => 23
	i64 5348796042099802469, ; 82: Xamarin.AndroidX.Media => 0x4a3abda9415fc165 => 94
	i64 5376510917114486089, ; 83: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 106
	i64 5408338804355907810, ; 84: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 105
	i64 5446034149219586269, ; 85: System.Diagnostics.Debug => 0x4b94333452e150dd => 8
	i64 5507995362134886206, ; 86: System.Core.dll => 0x4c705499688c873e => 40
	i64 5605194967438741945, ; 87: Azure.Messaging.ServiceBus => 0x4dc9a72012fe51b9 => 17
	i64 5692067934154308417, ; 88: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 110
	i64 5767696078500135884, ; 89: Xamarin.Android.Support.Annotations.dll => 0x500af9065b6a03cc => 57
	i64 5767749323661124970, ; 90: ZXing.Net.Mobile.Core => 0x500b29737652256a => 118
	i64 5814345312393086621, ; 91: Xamarin.AndroidX.Preference => 0x50b0b44182a5c69d => 99
	i64 5819465594466874502, ; 92: SignaturePad.Forms => 0x50c2e52014ce3486 => 38
	i64 5896680224035167651, ; 93: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 88
	i64 6085203216496545422, ; 94: Xamarin.Forms.Platform.dll => 0x5472fc15a9574e8e => 114
	i64 6086316965293125504, ; 95: FormsViewGroup.dll => 0x5476f10882baef80 => 19
	i64 6130998342382878844, ; 96: Azure.Core.Amqp.dll => 0x5515ae824c3d807c => 15
	i64 6222399776351216807, ; 97: System.Text.Json.dll => 0x565a67a0ffe264a7 => 54
	i64 6319713645133255417, ; 98: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 89
	i64 6401687960814735282, ; 99: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 87
	i64 6504860066809920875, ; 100: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 72
	i64 6548213210057960872, ; 101: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 78
	i64 6588599331800941662, ; 102: Xamarin.Android.Support.v4 => 0x5b6f682f335f045e => 63
	i64 6591024623626361694, ; 103: System.Web.Services.dll => 0x5b7805f9751a1b5e => 130
	i64 6659513131007730089, ; 104: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0x5c6b57e8b6c3e1a9 => 83
	i64 6710414900318471453, ; 105: System.Reactive.Interfaces.dll => 0x5d202ecc6cf8851d => 48
	i64 6876862101832370452, ; 106: System.Xml.Linq => 0x5f6f85a57d108914 => 56
	i64 6894844156784520562, ; 107: System.Numerics.Vectors => 0x5faf683aead1ad72 => 46
	i64 7036436454368433159, ; 108: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x61a671acb33d5407 => 85
	i64 7046697327704092548, ; 109: System.Reactive.Interfaces => 0x61cae5e2717f2f84 => 48
	i64 7103753931438454322, ; 110: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 82
	i64 7141577505875122296, ; 111: System.Runtime.InteropServices.WindowsRuntime.dll => 0x631bfae7659b5878 => 13
	i64 7270811800166795866, ; 112: System.Linq => 0x64e71ccf51a90a5a => 139
	i64 7338192458477945005, ; 113: System.Reflection => 0x65d67f295d0740ad => 136
	i64 7348123982286201829, ; 114: System.Memory.Data.dll => 0x65f9c7d471b2a3e5 => 43
	i64 7488575175965059935, ; 115: System.Xml.Linq.dll => 0x67ecc3724534ab5f => 56
	i64 7489048572193775167, ; 116: System.ObjectModel => 0x67ee71ff6b419e3f => 141
	i64 7565058294581776089, ; 117: P4WHandheld.Android.dll => 0x68fc7c7001ab4ed9 => 0
	i64 7576191739629449958, ; 118: Microsoft.AspNet.SignalR.Client => 0x69240a3f2edcb2e6 => 24
	i64 7635363394907363464, ; 119: Xamarin.Forms.Core.dll => 0x69f6428dc4795888 => 112
	i64 7637365915383206639, ; 120: Xamarin.Essentials.dll => 0x69fd5fd5e61792ef => 111
	i64 7654504624184590948, ; 121: System.Net.Http => 0x6a3a4366801b8264 => 14
	i64 7735176074855944702, ; 122: Microsoft.CSharp => 0x6b58dda848e391fe => 27
	i64 7820441508502274321, ; 123: System.Data => 0x6c87ca1e14ff8111 => 123
	i64 7836164640616011524, ; 124: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 67
	i64 8044118961405839122, ; 125: System.ComponentModel.Composition => 0x6fa2739369944712 => 129
	i64 8064050204834738623, ; 126: System.Collections.dll => 0x6fe942efa61731bf => 6
	i64 8083354569033831015, ; 127: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 86
	i64 8087206902342787202, ; 128: System.Diagnostics.DiagnosticSource => 0x703b87d46f3aa082 => 41
	i64 8101777744205214367, ; 129: Xamarin.Android.Support.Annotations => 0x706f4beeec84729f => 57
	i64 8103644804370223335, ; 130: System.Data.DataSetExtensions.dll => 0x7075ee03be6d50e7 => 125
	i64 8167236081217502503, ; 131: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 21
	i64 8185542183669246576, ; 132: System.Collections => 0x7198e33f4794aa70 => 6
	i64 8290740647658429042, ; 133: System.Runtime.Extensions => 0x730ea0b15c929a72 => 137
	i64 8400357532724379117, ; 134: Xamarin.AndroidX.Navigation.UI.dll => 0x749410ab44503ded => 98
	i64 8470680757392014285, ; 135: System.Reactive.Linq => 0x758de744ee167bcd => 49
	i64 8510386122694925816, ; 136: MobileDevice => 0x761af716b0a51df8 => 28
	i64 8537064613166331693, ; 137: Plugin.SimpleAudioPlayer.dll => 0x7679bf08cc56372d => 35
	i64 8562358305470590539, ; 138: System.Reactive.PlatformServices.dll => 0x76d39b837530ae4b => 50
	i64 8601935802264776013, ; 139: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 105
	i64 8626175481042262068, ; 140: Java.Interop => 0x77b654e585b55834 => 21
	i64 8638972117149407195, ; 141: Microsoft.CSharp.dll => 0x77e3cb5e8b31d7db => 27
	i64 8639588376636138208, ; 142: Xamarin.AndroidX.Navigation.Runtime => 0x77e5fbdaa2fda2e0 => 97
	i64 8684531736582871431, ; 143: System.IO.Compression.FileSystem => 0x7885a79a0fa0d987 => 128
	i64 8725526185868997716, ; 144: System.Diagnostics.DiagnosticSource.dll => 0x79174bd613173454 => 41
	i64 9009927183260475773, ; 145: Plugin.LatestVersion.dll => 0x7d09b1095a60617d => 32
	i64 9020037700568894461, ; 146: System.Reactive.Core => 0x7d2d9c7f9b0a4bfd => 47
	i64 9117941539726723070, ; 147: DataTransferObjects => 0x7e896f857e3087fe => 2
	i64 9312692141327339315, ; 148: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 110
	i64 9324707631942237306, ; 149: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 68
	i64 9419392115832876195, ; 150: System.Reactive.PlatformServices => 0x82b8673928556ca3 => 50
	i64 9584643793929893533, ; 151: System.IO.dll => 0x85037ebfbbd7f69d => 133
	i64 9659729154652888475, ; 152: System.Text.RegularExpressions => 0x860e407c9991dd9b => 143
	i64 9662334977499516867, ; 153: System.Numerics.dll => 0x8617827802b0cfc3 => 45
	i64 9678050649315576968, ; 154: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 75
	i64 9711637524876806384, ; 155: Xamarin.AndroidX.Media.dll => 0x86c6aadfd9a2c8f0 => 94
	i64 9808709177481450983, ; 156: Mono.Android.dll => 0x881f890734e555e7 => 29
	i64 9834056768316610435, ; 157: System.Transactions.dll => 0x8879968718899783 => 124
	i64 9998632235833408227, ; 158: Mono.Security => 0x8ac2470b209ebae3 => 138
	i64 10038780035334861115, ; 159: System.Net.Http.dll => 0x8b50e941206af13b => 14
	i64 10229024438826829339, ; 160: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 78
	i64 10303118382221642606, ; 161: Plugin.SimpleAudioPlayer.Abstractions.dll => 0x8efc0794931e4b6e => 34
	i64 10360651442923773544, ; 162: System.Text.Encoding => 0x8fc86d98211c1e68 => 142
	i64 10430153318873392755, ; 163: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 76
	i64 10447083246144586668, ; 164: Microsoft.Bcl.AsyncInterfaces.dll => 0x90fb7edc816203ac => 26
	i64 10566960649245365243, ; 165: System.Globalization.dll => 0x92a562b96dcd13fb => 11
	i64 10714184849103829812, ; 166: System.Runtime.Extensions.dll => 0x94b06e5aa4b4bb34 => 137
	i64 10847732767863316357, ; 167: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 69
	i64 10964653383833615866, ; 168: System.Diagnostics.Tracing => 0x982a4628ccaffdfa => 135
	i64 11023048688141570732, ; 169: System.Core => 0x98f9bc61168392ac => 40
	i64 11037814507248023548, ; 170: System.Xml => 0x992e31d0412bf7fc => 55
	i64 11162124722117608902, ; 171: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 109
	i64 11340910727871153756, ; 172: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 77
	i64 11376461258732682436, ; 173: Xamarin.Android.Support.Compat => 0x9de14f3d5fc13cc4 => 58
	i64 11392833485892708388, ; 174: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 100
	i64 11485890710487134646, ; 175: System.Runtime.InteropServices => 0x9f6614bf0f8b71b6 => 134
	i64 11529969570048099689, ; 176: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 109
	i64 11578238080964724296, ; 177: Xamarin.AndroidX.Legacy.Support.V4 => 0xa0ae2a30c4cd8648 => 85
	i64 11580057168383206117, ; 178: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 65
	i64 11597940890313164233, ; 179: netstandard => 0xa0f429ca8d1805c9 => 1
	i64 11672361001936329215, ; 180: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 82
	i64 11683710219442713716, ; 181: ZXingNetMobile => 0xa224e08aa87bf474 => 122
	i64 11743665907891708234, ; 182: System.Threading.Tasks => 0xa2f9e1ec30c0214a => 9
	i64 11825423055497389874, ; 183: DataTransferObjects.dll => 0xa41c579d7da2e732 => 2
	i64 12006736334756399793, ; 184: SignaturePad => 0xa6a07f1500ee4ab1 => 37
	i64 12036099219279441448, ; 185: ZXing.Net.Mobile.Forms => 0xa708d0784e81ee28 => 120
	i64 12102847907131387746, ; 186: System.Buffers => 0xa7f5f40c43256f62 => 39
	i64 12137774235383566651, ; 187: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 107
	i64 12145679461940342714, ; 188: System.Text.Json => 0xa88e1f1ebcb62fba => 54
	i64 12220938475036508228, ; 189: Matcha.BackgroundService.Droid.dll => 0xa9997ecb32748044 => 23
	i64 12307931450793293627, ; 190: Microsoft.Azure.Amqp.dll => 0xaace8e71bc64773b => 25
	i64 12414299427252656003, ; 191: Xamarin.Android.Support.Compat.dll => 0xac48738e28bad783 => 58
	i64 12450197211230333945, ; 192: SignaturePad.dll => 0xacc7fc664ef16bf9 => 37
	i64 12451044538927396471, ; 193: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 81
	i64 12466513435562512481, ; 194: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 92
	i64 12487638416075308985, ; 195: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 79
	i64 12538491095302438457, ; 196: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 73
	i64 12550732019250633519, ; 197: System.IO.Compression => 0xae2d28465e8e1b2f => 127
	i64 12629983860853673214, ; 198: ZXing.Net.Mobile.Forms.Android.dll => 0xaf46b767a9198cfe => 119
	i64 12700543734426720211, ; 199: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 74
	i64 12708238894395270091, ; 200: System.IO => 0xb05cbbf17d3ba3cb => 133
	i64 12952608645614506925, ; 201: Xamarin.Android.Support.Core.Utils => 0xb3c0e8eff48193ad => 60
	i64 12963446364377008305, ; 202: System.Drawing.Common.dll => 0xb3e769c8fd8548b1 => 126
	i64 13032622942553887878, ; 203: Matcha.BackgroundService => 0xb4dd2d843687d486 => 22
	i64 13358059602087096138, ; 204: Xamarin.Android.Support.Fragment.dll => 0xb9615c6f1ee5af4a => 61
	i64 13370592475155966277, ; 205: System.Runtime.Serialization => 0xb98de304062ea945 => 3
	i64 13401370062847626945, ; 206: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 107
	i64 13454009404024712428, ; 207: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 117
	i64 13491513212026656886, ; 208: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 70
	i64 13521613184248251399, ; 209: MobileDevice.dll => 0xbba66b8ced5b0407 => 28
	i64 13572454107664307259, ; 210: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 101
	i64 13630467124726050699, ; 211: Azure.Core.Amqp => 0xbd2925a1f3efaf8b => 15
	i64 13643785327914841093, ; 212: Plugin.Media.dll => 0xbd587677c60cf405 => 33
	i64 13647894001087880694, ; 213: System.Data.dll => 0xbd670f48cb071df6 => 123
	i64 13702626353344114072, ; 214: System.Diagnostics.Tools.dll => 0xbe29821198fb6d98 => 10
	i64 13852575513600495870, ; 215: ImageCircle.Forms.Plugin.dll => 0xc03e3c09186e90fe => 20
	i64 13959074834287824816, ; 216: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 81
	i64 13967638549803255703, ; 217: Xamarin.Forms.Platform.Android => 0xc1d70541e0134797 => 113
	i64 14124974489674258913, ; 218: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 73
	i64 14125464355221830302, ; 219: System.Threading.dll => 0xc407bafdbc707a9e => 7
	i64 14172845254133543601, ; 220: Xamarin.AndroidX.MultiDex => 0xc4b00faaed35f2b1 => 95
	i64 14261073672896646636, ; 221: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 100
	i64 14276308012117507949, ; 222: Plugin.LatestVersion => 0xc61fa27f7653bf6d => 32
	i64 14327695147300244862, ; 223: System.Reflection.dll => 0xc6d632d338eb4d7e => 136
	i64 14400856865250966808, ; 224: Xamarin.Android.Support.Core.UI => 0xc7da1f051a877d18 => 59
	i64 14438260825521943376, ; 225: RestSharp.dll => 0xc85f01b93fac7350 => 36
	i64 14486659737292545672, ; 226: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 88
	i64 14551742072151931844, ; 227: System.Text.Encodings.Web.dll => 0xc9f22c50f1b8fbc4 => 53
	i64 14644440854989303794, ; 228: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 93
	i64 14693486184709846151, ; 229: Plugin.SimpleAudioPlayer => 0xcbe9bfd5e7bd7487 => 35
	i64 14792063746108907174, ; 230: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 117
	i64 14852515768018889994, ; 231: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 77
	i64 14954388675289411854, ; 232: ZXing.Net.Mobile.Forms.dll => 0xcf88a944b7bff10e => 120
	i64 14987728460634540364, ; 233: System.IO.Compression.dll => 0xcfff1ba06622494c => 127
	i64 14988210264188246988, ; 234: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 79
	i64 15076659072870671916, ; 235: System.ObjectModel.dll => 0xd13b0d8c1620662c => 141
	i64 15133485256822086103, ; 236: System.Linq.dll => 0xd204f0a9127dd9d7 => 139
	i64 15296283831573226172, ; 237: Azure.Messaging.ServiceBus.dll => 0xd447511a047922bc => 17
	i64 15370334346939861994, ; 238: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 76
	i64 15383240894167415497, ; 239: System.Memory.Data => 0xd57c4016df1c7ac9 => 43
	i64 15457813392950723921, ; 240: Xamarin.Android.Support.Media.Compat => 0xd6852f61c31a8551 => 62
	i64 15526743539506359484, ; 241: System.Text.Encoding.dll => 0xd77a12fc26de2cbc => 142
	i64 15582737692548360875, ; 242: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 91
	i64 15609085926864131306, ; 243: System.dll => 0xd89e9cf3334914ea => 42
	i64 15777549416145007739, ; 244: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 103
	i64 15810740023422282496, ; 245: Xamarin.Forms.Xaml => 0xdb6b08484c22eb00 => 115
	i64 15817206913877585035, ; 246: System.Threading.Tasks.dll => 0xdb8201e29086ac8b => 9
	i64 15851975962649584118, ; 247: zxing.portable.dll => 0xdbfd882691c261f6 => 121
	i64 15963349826457351533, ; 248: System.Threading.Tasks.Extensions => 0xdd893616f748b56d => 131
	i64 16107354805249926211, ; 249: ZXingNetMobile.dll => 0xdf88d1dade1a6443 => 122
	i64 16119456071779071829, ; 250: FastAndroidCamera.dll => 0xdfb3cfe48ae7b755 => 18
	i64 16154507427712707110, ; 251: System => 0xe03056ea4e39aa26 => 42
	i64 16266187189082433806, ; 252: System.Reactive.Core.dll => 0xe1bd1b110744a90e => 47
	i64 16273242250288839465, ; 253: Microsoft.Azure.Amqp => 0xe1d62b9b78583b29 => 25
	i64 16526376532108668976, ; 254: ZXing.Net.Mobile.Forms.Android => 0xe5597be53cb07030 => 119
	i64 16565028646146589191, ; 255: System.ComponentModel.Composition.dll => 0xe5e2cdc9d3bcc207 => 129
	i64 16677317093839702854, ; 256: Xamarin.AndroidX.Navigation.UI => 0xe771bb8960dd8b46 => 98
	i64 16822611501064131242, ; 257: System.Data.DataSetExtensions => 0xe975ec07bb5412aa => 125
	i64 16833383113903931215, ; 258: mscorlib => 0xe99c30c1484d7f4f => 30
	i64 16866861824412579935, ; 259: System.Runtime.InteropServices.WindowsRuntime => 0xea132176ffb5785f => 13
	i64 16890310621557459193, ; 260: System.Text.RegularExpressions.dll => 0xea66700587f088f9 => 143
	i64 16932527889823454152, ; 261: Xamarin.Android.Support.Core.Utils.dll => 0xeafc6c67465253c8 => 60
	i64 16945858842201062480, ; 262: Azure.Core => 0xeb2bc8d57f4e7c50 => 16
	i64 17024911836938395553, ; 263: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 66
	i64 17031351772568316411, ; 264: Xamarin.AndroidX.Navigation.Common.dll => 0xec5b843380a769fb => 96
	i64 17037200463775726619, ; 265: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 84
	i64 17151170952569239713, ; 266: RestSharp => 0xee05331c4de338a1 => 36
	i64 17194278927140186426, ; 267: P4WHandheld.Android => 0xee9e5995c52ce93a => 0
	i64 17333249706306540043, ; 268: System.Diagnostics.Tracing.dll => 0xf08c12c5bb8b920b => 135
	i64 17428701562824544279, ; 269: Xamarin.Android.Support.Core.UI.dll => 0xf1df2fbaec73d017 => 59
	i64 17475612250781254388, ; 270: Matcha.BackgroundService.dll => 0xf285d8c0984802f4 => 22
	i64 17544493274320527064, ; 271: Xamarin.AndroidX.AsyncLayoutInflater => 0xf37a8fada41aded8 => 71
	i64 17627500474728259406, ; 272: System.Globalization => 0xf4a176498a351f4e => 11
	i64 17685921127322830888, ; 273: System.Diagnostics.Debug.dll => 0xf571038fafa74828 => 8
	i64 17704177640604968747, ; 274: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 92
	i64 17710060891934109755, ; 275: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 90
	i64 17712670374920797664, ; 276: System.Runtime.InteropServices.dll => 0xf5d00bdc38bd3de0 => 134
	i64 17838668724098252521, ; 277: System.Buffers.dll => 0xf78faeb0f5bf3ee9 => 39
	i64 17882897186074144999, ; 278: FormsViewGroup => 0xf82cd03e3ac830e7 => 19
	i64 17892495832318972303, ; 279: Xamarin.Forms.Xaml.dll => 0xf84eea293687918f => 115
	i64 17928294245072900555, ; 280: System.IO.Compression.FileSystem.dll => 0xf8ce18a0b24011cb => 128
	i64 18025913125965088385, ; 281: System.Threading => 0xfa28e87b91334681 => 7
	i64 18116111925905154859, ; 282: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 70
	i64 18121036031235206392, ; 283: Xamarin.AndroidX.Navigation.Common => 0xfb7ada42d3d42cf8 => 96
	i64 18129453464017766560, ; 284: System.ServiceModel.Internals.dll => 0xfb98c1df1ec108a0 => 132
	i64 18245806341561545090, ; 285: System.Collections.Concurrent.dll => 0xfd3620327d587182 => 5
	i64 18305135509493619199, ; 286: Xamarin.AndroidX.Navigation.Runtime.dll => 0xfe08e7c2d8c199ff => 97
	i64 18380184030268848184 ; 287: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 108
], align 16
@assembly_image_cache_indices = local_unnamed_addr constant [288 x i32] [
	i32 29, i32 140, i32 74, i32 102, i32 103, i32 20, i32 89, i32 126, ; 0..7
	i32 83, i32 12, i32 80, i32 124, i32 114, i32 138, i32 16, i32 116, ; 8..15
	i32 69, i32 61, i32 3, i32 38, i32 34, i32 67, i32 91, i32 84, ; 16..23
	i32 31, i32 44, i32 68, i32 102, i32 26, i32 65, i32 63, i32 90, ; 24..31
	i32 131, i32 31, i32 24, i32 95, i32 72, i32 80, i32 130, i32 5, ; 32..39
	i32 87, i32 53, i32 51, i32 75, i32 106, i32 52, i32 64, i32 55, ; 40..47
	i32 30, i32 99, i32 51, i32 112, i32 116, i32 62, i32 86, i32 66, ; 48..55
	i32 46, i32 104, i32 45, i32 52, i32 1, i32 101, i32 10, i32 44, ; 56..63
	i32 12, i32 140, i32 121, i32 118, i32 132, i32 108, i32 71, i32 64, ; 64..71
	i32 104, i32 4, i32 4, i32 113, i32 33, i32 111, i32 93, i32 18, ; 72..79
	i32 49, i32 23, i32 94, i32 106, i32 105, i32 8, i32 40, i32 17, ; 80..87
	i32 110, i32 57, i32 118, i32 99, i32 38, i32 88, i32 114, i32 19, ; 88..95
	i32 15, i32 54, i32 89, i32 87, i32 72, i32 78, i32 63, i32 130, ; 96..103
	i32 83, i32 48, i32 56, i32 46, i32 85, i32 48, i32 82, i32 13, ; 104..111
	i32 139, i32 136, i32 43, i32 56, i32 141, i32 0, i32 24, i32 112, ; 112..119
	i32 111, i32 14, i32 27, i32 123, i32 67, i32 129, i32 6, i32 86, ; 120..127
	i32 41, i32 57, i32 125, i32 21, i32 6, i32 137, i32 98, i32 49, ; 128..135
	i32 28, i32 35, i32 50, i32 105, i32 21, i32 27, i32 97, i32 128, ; 136..143
	i32 41, i32 32, i32 47, i32 2, i32 110, i32 68, i32 50, i32 133, ; 144..151
	i32 143, i32 45, i32 75, i32 94, i32 29, i32 124, i32 138, i32 14, ; 152..159
	i32 78, i32 34, i32 142, i32 76, i32 26, i32 11, i32 137, i32 69, ; 160..167
	i32 135, i32 40, i32 55, i32 109, i32 77, i32 58, i32 100, i32 134, ; 168..175
	i32 109, i32 85, i32 65, i32 1, i32 82, i32 122, i32 9, i32 2, ; 176..183
	i32 37, i32 120, i32 39, i32 107, i32 54, i32 23, i32 25, i32 58, ; 184..191
	i32 37, i32 81, i32 92, i32 79, i32 73, i32 127, i32 119, i32 74, ; 192..199
	i32 133, i32 60, i32 126, i32 22, i32 61, i32 3, i32 107, i32 117, ; 200..207
	i32 70, i32 28, i32 101, i32 15, i32 33, i32 123, i32 10, i32 20, ; 208..215
	i32 81, i32 113, i32 73, i32 7, i32 95, i32 100, i32 32, i32 136, ; 216..223
	i32 59, i32 36, i32 88, i32 53, i32 93, i32 35, i32 117, i32 77, ; 224..231
	i32 120, i32 127, i32 79, i32 141, i32 139, i32 17, i32 76, i32 43, ; 232..239
	i32 62, i32 142, i32 91, i32 42, i32 103, i32 115, i32 9, i32 121, ; 240..247
	i32 131, i32 122, i32 18, i32 42, i32 47, i32 25, i32 119, i32 129, ; 248..255
	i32 98, i32 125, i32 30, i32 13, i32 143, i32 60, i32 16, i32 66, ; 256..263
	i32 96, i32 84, i32 36, i32 0, i32 135, i32 59, i32 22, i32 71, ; 264..271
	i32 11, i32 8, i32 92, i32 90, i32 134, i32 39, i32 19, i32 115, ; 272..279
	i32 128, i32 7, i32 70, i32 96, i32 132, i32 5, i32 97, i32 108 ; 288..287
], align 16

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 8; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 8

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 8
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 8
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 16; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="x86-64" "target-features"="+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="x86-64" "target-features"="+cx16,+cx8,+fxsr,+mmx,+popcnt,+sse,+sse2,+sse3,+sse4.1,+sse4.2,+ssse3,+x87" "tune-cpu"="generic" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1}
!llvm.ident = !{!2}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
