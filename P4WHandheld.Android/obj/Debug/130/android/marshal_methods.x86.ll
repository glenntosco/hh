; ModuleID = 'obj\Debug\130\android\marshal_methods.x86.ll'
source_filename = "obj\Debug\130\android\marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android"


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
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [288 x i32] [
	i32 32687329, ; 0: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 89
	i32 34715100, ; 1: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 117
	i32 39109920, ; 2: Newtonsoft.Json.dll => 0x254c520 => 31
	i32 57263871, ; 3: Xamarin.Forms.Core.dll => 0x369c6ff => 112
	i32 101534019, ; 4: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 103
	i32 102832730, ; 5: Plugin.SimpleAudioPlayer => 0x6211a5a => 35
	i32 117431740, ; 6: System.Runtime.InteropServices => 0x6ffddbc => 134
	i32 120558881, ; 7: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 103
	i32 165246403, ; 8: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 74
	i32 166922606, ; 9: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 58
	i32 172012715, ; 10: FastAndroidCamera.dll => 0xa40b4ab => 18
	i32 182336117, ; 11: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 104
	i32 205061960, ; 12: System.ComponentModel => 0xc38ff48 => 12
	i32 209399409, ; 13: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 72
	i32 219130465, ; 14: Xamarin.Android.Support.v4 => 0xd0faa61 => 63
	i32 220171995, ; 15: System.Diagnostics.Debug => 0xd1f8edb => 8
	i32 230216969, ; 16: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 84
	i32 230752869, ; 17: Microsoft.CSharp.dll => 0xdc10265 => 27
	i32 231814094, ; 18: System.Globalization => 0xdd133ce => 11
	i32 232815796, ; 19: System.Web.Services => 0xde07cb4 => 130
	i32 247043169, ; 20: MobileDevice => 0xeb99461 => 28
	i32 278686392, ; 21: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 88
	i32 280482487, ; 22: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 82
	i32 318968648, ; 23: Xamarin.AndroidX.Activity.dll => 0x13031348 => 64
	i32 321597661, ; 24: System.Numerics => 0x132b30dd => 45
	i32 334355562, ; 25: ZXing.Net.Mobile.Forms.dll => 0x13eddc6a => 120
	i32 342366114, ; 26: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 86
	i32 385762202, ; 27: System.Memory.dll => 0x16fe439a => 44
	i32 389971796, ; 28: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 59
	i32 442521989, ; 29: Xamarin.Essentials => 0x1a605985 => 111
	i32 442565967, ; 30: System.Collections => 0x1a61054f => 6
	i32 447382559, ; 31: P4WHandheld.Android.dll => 0x1aaa841f => 0
	i32 450948140, ; 32: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 81
	i32 465846621, ; 33: mscorlib => 0x1bc4415d => 30
	i32 469710990, ; 34: System.dll => 0x1bff388e => 42
	i32 476646585, ; 35: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 82
	i32 484677488, ; 36: MobileDevice.dll => 0x1ce39770 => 28
	i32 486930444, ; 37: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 93
	i32 498788369, ; 38: System.ObjectModel => 0x1dbae811 => 141
	i32 514659665, ; 39: Xamarin.Android.Support.Compat => 0x1ead1551 => 58
	i32 526420162, ; 40: System.Transactions.dll => 0x1f6088c2 => 124
	i32 545304856, ; 41: System.Runtime.Extensions => 0x2080b118 => 137
	i32 548916678, ; 42: Microsoft.Bcl.AsyncInterfaces => 0x20b7cdc6 => 26
	i32 561442103, ; 43: Microsoft.Azure.Amqp.dll => 0x2176ed37 => 25
	i32 569127776, ; 44: Microsoft.Azure.Amqp => 0x21ec3360 => 25
	i32 605376203, ; 45: System.IO.Compression.FileSystem => 0x24154ecb => 128
	i32 627609679, ; 46: Xamarin.AndroidX.CustomView => 0x2568904f => 78
	i32 662205335, ; 47: System.Text.Encodings.Web.dll => 0x27787397 => 53
	i32 663517072, ; 48: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 108
	i32 666292255, ; 49: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 69
	i32 672442732, ; 50: System.Collections.Concurrent => 0x2814a96c => 5
	i32 690569205, ; 51: System.Xml.Linq.dll => 0x29293ff5 => 56
	i32 692692150, ; 52: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 57
	i32 775507847, ; 53: System.IO.Compression => 0x2e394f87 => 127
	i32 802720955, ; 54: SignaturePad => 0x2fd88cbb => 37
	i32 809851609, ; 55: System.Drawing.Common.dll => 0x30455ad9 => 126
	i32 831745141, ; 56: System.Reactive.Linq => 0x31936c75 => 49
	i32 843511501, ; 57: Xamarin.AndroidX.Print => 0x3246f6cd => 100
	i32 877678880, ; 58: System.Globalization.dll => 0x34505120 => 11
	i32 882883187, ; 59: Xamarin.Android.Support.v4.dll => 0x349fba73 => 63
	i32 903406257, ; 60: SignaturePad.dll => 0x35d8e2b1 => 37
	i32 928116545, ; 61: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 117
	i32 954320159, ; 62: ZXing.Net.Mobile.Forms => 0x38e1c51f => 120
	i32 955402788, ; 63: Newtonsoft.Json => 0x38f24a24 => 31
	i32 958213972, ; 64: Xamarin.Android.Support.Media.Compat => 0x391d2f54 => 62
	i32 967690846, ; 65: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 86
	i32 974778368, ; 66: FormsViewGroup.dll => 0x3a19f000 => 19
	i32 975236339, ; 67: System.Diagnostics.Tracing => 0x3a20ecf3 => 135
	i32 987214855, ; 68: System.Diagnostics.Tools => 0x3ad7b407 => 10
	i32 992768348, ; 69: System.Collections.dll => 0x3b2c715c => 6
	i32 996170219, ; 70: Plugin.SimpleAudioPlayer.Abstractions => 0x3b6059eb => 34
	i32 1012816738, ; 71: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 102
	i32 1035644815, ; 72: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 68
	i32 1042160112, ; 73: Xamarin.Forms.Platform.dll => 0x3e1e19f0 => 114
	i32 1044663988, ; 74: System.Linq.Expressions.dll => 0x3e444eb4 => 140
	i32 1052210849, ; 75: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 90
	i32 1098259244, ; 76: System => 0x41761b2c => 42
	i32 1104002344, ; 77: Plugin.Media => 0x41cdbd28 => 33
	i32 1134191450, ; 78: ZXingNetMobile.dll => 0x439a635a => 122
	i32 1175144683, ; 79: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 106
	i32 1178241025, ; 80: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 97
	i32 1204270330, ; 81: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 69
	i32 1267360935, ; 82: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 107
	i32 1293217323, ; 83: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 80
	i32 1324164729, ; 84: System.Linq => 0x4eed2679 => 139
	i32 1353320821, ; 85: Plugin.LatestVersion => 0x50aa0975 => 32
	i32 1364015309, ; 86: System.IO => 0x514d38cd => 133
	i32 1365406463, ; 87: System.ServiceModel.Internals.dll => 0x516272ff => 132
	i32 1376866003, ; 88: Xamarin.AndroidX.SavedState => 0x52114ed3 => 102
	i32 1379779777, ; 89: System.Resources.ResourceManager => 0x523dc4c1 => 4
	i32 1395857551, ; 90: Xamarin.AndroidX.Media.dll => 0x5333188f => 94
	i32 1406073936, ; 91: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 75
	i32 1411638395, ; 92: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 51
	i32 1445445088, ; 93: Xamarin.Android.Support.Fragment => 0x5627bde0 => 61
	i32 1453312822, ; 94: System.Diagnostics.Tools.dll => 0x569fcb36 => 10
	i32 1457743152, ; 95: System.Runtime.Extensions.dll => 0x56e36530 => 137
	i32 1460219004, ; 96: Xamarin.Forms.Xaml => 0x57092c7c => 115
	i32 1462112819, ; 97: System.IO.Compression.dll => 0x57261233 => 127
	i32 1469204771, ; 98: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 67
	i32 1528512243, ; 99: Matcha.BackgroundService => 0x5b1b3ef3 => 22
	i32 1543031311, ; 100: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 143
	i32 1548710267, ; 101: Matcha.BackgroundService.dll => 0x5c4f717b => 22
	i32 1550213776, ; 102: P4WHandheld.Android => 0x5c666290 => 0
	i32 1571005899, ; 103: zxing.portable => 0x5da3a5cb => 121
	i32 1574652163, ; 104: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 60
	i32 1582372066, ; 105: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 79
	i32 1592978981, ; 106: System.Runtime.Serialization.dll => 0x5ef2ee25 => 3
	i32 1622152042, ; 107: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 92
	i32 1624863272, ; 108: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 110
	i32 1626988262, ; 109: Plugin.SimpleAudioPlayer.Abstractions.dll => 0x60f9dee6 => 34
	i32 1632079564, ; 110: Microsoft.AspNet.SignalR.Client.dll => 0x61478ecc => 24
	i32 1636350590, ; 111: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 77
	i32 1639515021, ; 112: System.Net.Http.dll => 0x61b9038d => 14
	i32 1639986890, ; 113: System.Text.RegularExpressions => 0x61c036ca => 143
	i32 1657153582, ; 114: System.Runtime => 0x62c6282e => 52
	i32 1658251792, ; 115: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 116
	i32 1701541528, ; 116: System.Diagnostics.Debug.dll => 0x656b7698 => 8
	i32 1726116996, ; 117: System.Reflection.dll => 0x66e27484 => 136
	i32 1729485958, ; 118: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 73
	i32 1766324549, ; 119: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 104
	i32 1776026572, ; 120: System.Core.dll => 0x69dc03cc => 40
	i32 1788241197, ; 121: Xamarin.AndroidX.Fragment => 0x6a96652d => 81
	i32 1796167890, ; 122: Microsoft.Bcl.AsyncInterfaces.dll => 0x6b0f58d2 => 26
	i32 1808609942, ; 123: Xamarin.AndroidX.Loader => 0x6bcd3296 => 92
	i32 1813201214, ; 124: Xamarin.Google.Android.Material => 0x6c13413e => 116
	i32 1818569960, ; 125: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 98
	i32 1858542181, ; 126: System.Linq.Expressions => 0x6ec71a65 => 140
	i32 1867746548, ; 127: Xamarin.Essentials.dll => 0x6f538cf4 => 111
	i32 1878053835, ; 128: Xamarin.Forms.Xaml.dll => 0x6ff0d3cb => 115
	i32 1885316902, ; 129: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 70
	i32 1900610850, ; 130: System.Resources.ResourceManager.dll => 0x71490522 => 4
	i32 1904184254, ; 131: FastAndroidCamera => 0x717f8bbe => 18
	i32 1904755420, ; 132: System.Runtime.InteropServices.WindowsRuntime.dll => 0x718842dc => 13
	i32 1919157823, ; 133: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 95
	i32 1928379650, ; 134: DataTransferObjects.dll => 0x72f0bd02 => 2
	i32 1948011946, ; 135: Plugin.LatestVersion.dll => 0x741c4daa => 32
	i32 1991544456, ; 136: SignaturePad.Forms.dll => 0x76b48e88 => 38
	i32 2011961780, ; 137: System.Buffers.dll => 0x77ec19b4 => 39
	i32 2019465201, ; 138: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 90
	i32 2048185678, ; 139: Plugin.Media.dll => 0x7a14d54e => 33
	i32 2055257422, ; 140: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 87
	i32 2079903147, ; 141: System.Runtime.dll => 0x7bf8cdab => 52
	i32 2090596640, ; 142: System.Numerics.Vectors => 0x7c9bf920 => 46
	i32 2097448633, ; 143: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 83
	i32 2126786730, ; 144: Xamarin.Forms.Platform.Android => 0x7ec430aa => 113
	i32 2154747847, ; 145: Azure.Core.Amqp => 0x806ed7c7 => 15
	i32 2166116741, ; 146: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 60
	i32 2193016926, ; 147: System.ObjectModel.dll => 0x82b6c85e => 141
	i32 2201231467, ; 148: System.Net.Http => 0x8334206b => 14
	i32 2217644978, ; 149: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 106
	i32 2240986525, ; 150: Microsoft.AspNet.SignalR.Client => 0x8592bd9d => 24
	i32 2244775296, ; 151: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 93
	i32 2256548716, ; 152: Xamarin.AndroidX.MultiDex => 0x8680336c => 95
	i32 2261435625, ; 153: Xamarin.AndroidX.Legacy.Support.V4.dll => 0x86cac4e9 => 85
	i32 2279755925, ; 154: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 101
	i32 2287417322, ; 155: Plugin.SimpleAudioPlayer.dll => 0x885737ea => 35
	i32 2292630417, ; 156: System.Reactive.PlatformServices.dll => 0x88a6c391 => 50
	i32 2303054758, ; 157: System.Reactive.Interfaces.dll => 0x8945d3a6 => 48
	i32 2315684594, ; 158: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 65
	i32 2319330481, ; 159: Matcha.BackgroundService.Droid.dll => 0x8a3e2cb1 => 23
	i32 2321843949, ; 160: Matcha.BackgroundService.Droid => 0x8a6486ed => 23
	i32 2329204181, ; 161: zxing.portable.dll => 0x8ad4d5d5 => 121
	i32 2330457430, ; 162: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 59
	i32 2341995103, ; 163: ZXingNetMobile => 0x8b98025f => 122
	i32 2373288475, ; 164: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 61
	i32 2409053734, ; 165: Xamarin.AndroidX.Preference.dll => 0x8f973e26 => 99
	i32 2431243866, ; 166: ZXing.Net.Mobile.Core.dll => 0x90e9d65a => 118
	i32 2443007334, ; 167: Azure.Messaging.ServiceBus => 0x919d5566 => 17
	i32 2454642406, ; 168: System.Text.Encoding.dll => 0x924edee6 => 142
	i32 2471215200, ; 169: ImageCircle.Forms.Plugin => 0x934bc060 => 20
	i32 2471841756, ; 170: netstandard.dll => 0x93554fdc => 1
	i32 2475788418, ; 171: Java.Interop.dll => 0x93918882 => 21
	i32 2482213323, ; 172: ZXing.Net.Mobile.Forms.Android => 0x93f391cb => 119
	i32 2501346920, ; 173: System.Data.DataSetExtensions => 0x95178668 => 125
	i32 2505896520, ; 174: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 89
	i32 2562349572, ; 175: Microsoft.CSharp => 0x98ba5a04 => 27
	i32 2570120770, ; 176: System.Text.Encodings.Web => 0x9930ee42 => 53
	i32 2581819634, ; 177: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 107
	i32 2620871830, ; 178: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 77
	i32 2628210652, ; 179: System.Memory.Data => 0x9ca74fdc => 43
	i32 2633051222, ; 180: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 88
	i32 2640706905, ; 181: Azure.Core => 0x9d65fd59 => 16
	i32 2693849962, ; 182: System.IO.dll => 0xa090e36a => 133
	i32 2715334215, ; 183: System.Threading.Tasks.dll => 0xa1d8b647 => 9
	i32 2732626843, ; 184: Xamarin.AndroidX.Activity => 0xa2e0939b => 64
	i32 2737747696, ; 185: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 67
	i32 2766581644, ; 186: Xamarin.Forms.Core => 0xa4e6af8c => 112
	i32 2778768386, ; 187: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 109
	i32 2810250172, ; 188: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 75
	i32 2819470561, ; 189: System.Xml.dll => 0xa80db4e1 => 55
	i32 2853208004, ; 190: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 109
	i32 2855708567, ; 191: Xamarin.AndroidX.Transition => 0xaa36a797 => 105
	i32 2901442782, ; 192: System.Reflection => 0xacf080de => 136
	i32 2903344695, ; 193: System.ComponentModel.Composition => 0xad0d8637 => 129
	i32 2905242038, ; 194: mscorlib.dll => 0xad2a79b6 => 30
	i32 2916838712, ; 195: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 110
	i32 2919462931, ; 196: System.Numerics.Vectors.dll => 0xae037813 => 46
	i32 2921128767, ; 197: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 66
	i32 2959614098, ; 198: System.ComponentModel.dll => 0xb0682092 => 12
	i32 2978675010, ; 199: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 80
	i32 3012597153, ; 200: DataTransferObjects => 0xb39095a1 => 2
	i32 3024354802, ; 201: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 84
	i32 3033605958, ; 202: System.Memory.Data.dll => 0xb4d12746 => 43
	i32 3044182254, ; 203: FormsViewGroup => 0xb57288ee => 19
	i32 3057625584, ; 204: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 96
	i32 3075834255, ; 205: System.Threading.Tasks => 0xb755818f => 9
	i32 3092211740, ; 206: Xamarin.Android.Support.Media.Compat.dll => 0xb84f681c => 62
	i32 3111772706, ; 207: System.Runtime.Serialization => 0xb979e222 => 3
	i32 3124832203, ; 208: System.Threading.Tasks.Extensions => 0xba4127cb => 131
	i32 3147165239, ; 209: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 135
	i32 3204380047, ; 210: System.Data.dll => 0xbefef58f => 123
	i32 3211777861, ; 211: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 79
	i32 3214898845, ; 212: Azure.Messaging.ServiceBus.dll => 0xbf9f769d => 17
	i32 3220365878, ; 213: System.Threading => 0xbff2e236 => 7
	i32 3247949154, ; 214: Mono.Security => 0xc197c562 => 138
	i32 3249260365, ; 215: RestSharp.dll => 0xc1abc74d => 36
	i32 3258312781, ; 216: Xamarin.AndroidX.CardView => 0xc235e84d => 73
	i32 3265893370, ; 217: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 131
	i32 3267021929, ; 218: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 71
	i32 3282591531, ; 219: System.Reactive.Interfaces => 0xc3a85f2b => 48
	i32 3299363146, ; 220: System.Text.Encoding => 0xc4a8494a => 142
	i32 3300173928, ; 221: System.Reactive.Core => 0xc4b4a868 => 47
	i32 3317135071, ; 222: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 78
	i32 3317144872, ; 223: System.Data => 0xc5b79d28 => 123
	i32 3340431453, ; 224: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 70
	i32 3346324047, ; 225: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 97
	i32 3353484488, ; 226: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 83
	i32 3358260929, ; 227: System.Text.Json => 0xc82afec1 => 54
	i32 3362522851, ; 228: Xamarin.AndroidX.Core => 0xc86c06e3 => 76
	i32 3366347497, ; 229: Java.Interop => 0xc8a662e9 => 21
	i32 3374999561, ; 230: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 101
	i32 3395150330, ; 231: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 51
	i32 3404865022, ; 232: System.ServiceModel.Internals => 0xcaf21dfe => 132
	i32 3425410982, ; 233: System.Reactive.Core.dll => 0xcc2b9fa6 => 47
	i32 3429136800, ; 234: System.Xml => 0xcc6479a0 => 55
	i32 3430777524, ; 235: netstandard => 0xcc7d82b4 => 1
	i32 3439690031, ; 236: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 57
	i32 3476120550, ; 237: Mono.Android => 0xcf3163e6 => 29
	i32 3483112796, ; 238: ImageCircle.Forms.Plugin.dll => 0xcf9c155c => 20
	i32 3485117614, ; 239: System.Text.Json.dll => 0xcfbaacae => 54
	i32 3486566296, ; 240: System.Transactions => 0xcfd0c798 => 124
	i32 3501239056, ; 241: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 71
	i32 3509114376, ; 242: System.Xml.Linq => 0xd128d608 => 56
	i32 3536029504, ; 243: Xamarin.Forms.Platform.Android.dll => 0xd2c38740 => 113
	i32 3561949811, ; 244: Azure.Core.dll => 0xd44f0a73 => 16
	i32 3567349600, ; 245: System.ComponentModel.Composition.dll => 0xd4a16f60 => 129
	i32 3608519521, ; 246: System.Linq.dll => 0xd715a361 => 139
	i32 3618140916, ; 247: Xamarin.AndroidX.Preference => 0xd7a872f4 => 99
	i32 3627220390, ; 248: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 100
	i32 3632359727, ; 249: Xamarin.Forms.Platform => 0xd881692f => 114
	i32 3633644679, ; 250: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 66
	i32 3641597786, ; 251: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 87
	i32 3672681054, ; 252: Mono.Android.dll => 0xdae8aa5e => 29
	i32 3676310014, ; 253: System.Web.Services.dll => 0xdb2009fe => 130
	i32 3682565725, ; 254: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 72
	i32 3684933406, ; 255: System.Runtime.InteropServices.WindowsRuntime => 0xdba39f1e => 13
	i32 3689375977, ; 256: System.Drawing.Common => 0xdbe768e9 => 126
	i32 3718780102, ; 257: Xamarin.AndroidX.Annotation => 0xdda814c6 => 65
	i32 3724971120, ; 258: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 96
	i32 3748608112, ; 259: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 41
	i32 3758932259, ; 260: Xamarin.AndroidX.Legacy.Support.V4 => 0xe00cc123 => 85
	i32 3760520151, ; 261: System.Reactive.Linq.dll => 0xe024fbd7 => 49
	i32 3786282454, ; 262: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 74
	i32 3816437471, ; 263: RestSharp => 0xe37a36df => 36
	i32 3822602673, ; 264: Xamarin.AndroidX.Media => 0xe3d849b1 => 94
	i32 3829621856, ; 265: System.Numerics.dll => 0xe4436460 => 45
	i32 3835113687, ; 266: System.Reactive.PlatformServices => 0xe49730d7 => 50
	i32 3847036339, ; 267: ZXing.Net.Mobile.Forms.Android.dll => 0xe54d1db3 => 119
	i32 3849253459, ; 268: System.Runtime.InteropServices.dll => 0xe56ef253 => 134
	i32 3885922214, ; 269: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 105
	i32 3896106733, ; 270: System.Collections.Concurrent.dll => 0xe839deed => 5
	i32 3896760992, ; 271: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 76
	i32 3920810846, ; 272: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 128
	i32 3921031405, ; 273: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 108
	i32 3931092270, ; 274: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 98
	i32 3945713374, ; 275: System.Data.DataSetExtensions.dll => 0xeb2ecede => 125
	i32 3955647286, ; 276: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 68
	i32 3988778722, ; 277: Azure.Core.Amqp.dll => 0xedbfeee2 => 15
	i32 4025784931, ; 278: System.Memory => 0xeff49a63 => 44
	i32 4071430779, ; 279: SignaturePad.Forms => 0xf2ad1a7b => 38
	i32 4073602200, ; 280: System.Threading.dll => 0xf2ce3c98 => 7
	i32 4105002889, ; 281: Mono.Security.dll => 0xf4ad5f89 => 138
	i32 4151237749, ; 282: System.Core => 0xf76edc75 => 40
	i32 4182413190, ; 283: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 91
	i32 4186595366, ; 284: ZXing.Net.Mobile.Core => 0xf98a6026 => 118
	i32 4213026141, ; 285: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 41
	i32 4260525087, ; 286: System.Buffers => 0xfdf2741f => 39
	i32 4292120959 ; 287: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 91
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [288 x i32] [
	i32 89, i32 117, i32 31, i32 112, i32 103, i32 35, i32 134, i32 103, ; 0..7
	i32 74, i32 58, i32 18, i32 104, i32 12, i32 72, i32 63, i32 8, ; 8..15
	i32 84, i32 27, i32 11, i32 130, i32 28, i32 88, i32 82, i32 64, ; 16..23
	i32 45, i32 120, i32 86, i32 44, i32 59, i32 111, i32 6, i32 0, ; 24..31
	i32 81, i32 30, i32 42, i32 82, i32 28, i32 93, i32 141, i32 58, ; 32..39
	i32 124, i32 137, i32 26, i32 25, i32 25, i32 128, i32 78, i32 53, ; 40..47
	i32 108, i32 69, i32 5, i32 56, i32 57, i32 127, i32 37, i32 126, ; 48..55
	i32 49, i32 100, i32 11, i32 63, i32 37, i32 117, i32 120, i32 31, ; 56..63
	i32 62, i32 86, i32 19, i32 135, i32 10, i32 6, i32 34, i32 102, ; 64..71
	i32 68, i32 114, i32 140, i32 90, i32 42, i32 33, i32 122, i32 106, ; 72..79
	i32 97, i32 69, i32 107, i32 80, i32 139, i32 32, i32 133, i32 132, ; 80..87
	i32 102, i32 4, i32 94, i32 75, i32 51, i32 61, i32 10, i32 137, ; 88..95
	i32 115, i32 127, i32 67, i32 22, i32 143, i32 22, i32 0, i32 121, ; 96..103
	i32 60, i32 79, i32 3, i32 92, i32 110, i32 34, i32 24, i32 77, ; 104..111
	i32 14, i32 143, i32 52, i32 116, i32 8, i32 136, i32 73, i32 104, ; 112..119
	i32 40, i32 81, i32 26, i32 92, i32 116, i32 98, i32 140, i32 111, ; 120..127
	i32 115, i32 70, i32 4, i32 18, i32 13, i32 95, i32 2, i32 32, ; 128..135
	i32 38, i32 39, i32 90, i32 33, i32 87, i32 52, i32 46, i32 83, ; 136..143
	i32 113, i32 15, i32 60, i32 141, i32 14, i32 106, i32 24, i32 93, ; 144..151
	i32 95, i32 85, i32 101, i32 35, i32 50, i32 48, i32 65, i32 23, ; 152..159
	i32 23, i32 121, i32 59, i32 122, i32 61, i32 99, i32 118, i32 17, ; 160..167
	i32 142, i32 20, i32 1, i32 21, i32 119, i32 125, i32 89, i32 27, ; 168..175
	i32 53, i32 107, i32 77, i32 43, i32 88, i32 16, i32 133, i32 9, ; 176..183
	i32 64, i32 67, i32 112, i32 109, i32 75, i32 55, i32 109, i32 105, ; 184..191
	i32 136, i32 129, i32 30, i32 110, i32 46, i32 66, i32 12, i32 80, ; 192..199
	i32 2, i32 84, i32 43, i32 19, i32 96, i32 9, i32 62, i32 3, ; 200..207
	i32 131, i32 135, i32 123, i32 79, i32 17, i32 7, i32 138, i32 36, ; 208..215
	i32 73, i32 131, i32 71, i32 48, i32 142, i32 47, i32 78, i32 123, ; 216..223
	i32 70, i32 97, i32 83, i32 54, i32 76, i32 21, i32 101, i32 51, ; 224..231
	i32 132, i32 47, i32 55, i32 1, i32 57, i32 29, i32 20, i32 54, ; 232..239
	i32 124, i32 71, i32 56, i32 113, i32 16, i32 129, i32 139, i32 99, ; 240..247
	i32 100, i32 114, i32 66, i32 87, i32 29, i32 130, i32 72, i32 13, ; 248..255
	i32 126, i32 65, i32 96, i32 41, i32 85, i32 49, i32 74, i32 36, ; 256..263
	i32 94, i32 45, i32 50, i32 119, i32 134, i32 105, i32 5, i32 76, ; 264..271
	i32 128, i32 108, i32 98, i32 125, i32 68, i32 15, i32 44, i32 38, ; 272..279
	i32 7, i32 138, i32 40, i32 91, i32 118, i32 41, i32 39, i32 91 ; 288..287
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"NumRegisterParameters", i32 0}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ 45b0e144f73b2c8747d8b5ec8cbd3b55beca67f0"}
!llvm.linker.options = !{}
