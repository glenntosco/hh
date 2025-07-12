using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Plugin.SimpleAudioPlayer;
using Pro4Soft.DataTransferObjects;
using Pro4Soft.DataTransferObjects.Configuration;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.MobileDevice.Business;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;
using Xamarin.Essentials;
using Xamarin.Forms;
using Contact = Pro4Soft.DataTransferObjects.Dto.Collaboration.Contact;
using Menu = Pro4Soft.DataTransferObjects.Dto.Generic.Menu;

namespace Pro4Soft.MobileDevice.Plumbing
{
    public class Context: LoginDetails
    {
        public Dictionary<Guid, ImageSource> UserAvatars = new Dictionary<Guid, ImageSource>();

        public ISimpleAudioPlayer NewTaskSound { get; }
        public ISimpleAudioPlayer TaskRemovedSound { get; }
        public ISimpleAudioPlayer NewMessageSound { get; }

        public Context()
        {
            NewTaskSound = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            NewTaskSound.Load("new_task.mp3");

            TaskRemovedSound = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            TaskRemovedSound.Load("task_removed.mp3");

            NewMessageSound = CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
            NewMessageSound.Load("new_message.mp3");
        }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_LicensePlates_AllowLpnOnTheFloor))]
        public bool AllowLpnOnFloor { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Fulfillment_Picking_AllowShorting))]
        public bool AllowPickTicketPickShort { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Production_Picking_AllowShorting))]
        public bool AllowProdOrderPickShort { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Production_Picking_AllowSkipping))]
        public bool AllowProdOrderPickSkip { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Production_Handheld_PrintLabels))]
        public bool ProdOrderPrintLabels { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Production_Handheld_CollectWeightAndDimensions))]
        public bool ProdOrderCollectDims { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Receiving_Handheld_CollectBarcode))]
        public bool ProdOrderCollectBarcode { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Fulfillment_Picking_AllowSkipping))]
        public bool AllowPickTicketPickSkip { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Fulfillment_Picking_EmptyBinPrompt))]
        public bool EmptyBinPrompt { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceProductScan))]
        public bool ForceProductScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceBinLpnScan))]
        public bool ForceBinLpnScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceDockDoorScan))]
        public bool ForceDockDoorScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForcePoScan))]
        public bool ForcePoScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceCustomerReturnScan))]
        public bool ForceCustomerReturnScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForcePickTicketScan))]
        public bool ForcePickTicketScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceTruckLoadBolScan))]
        public bool ForceTruckLoadBolScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceWoScan))]
        public bool ForceWoScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ForceProductionOrderScan))]
        public bool ForceProdOrderScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_AllowQuantityScan))]
        public bool AllowQtyScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_AllowDateScan))]
        public bool AllowDateScan { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Returns_Handheld_SuggestPutawayBin))]
        public bool IsSuggestPutawayReturns { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Receiving_Handheld_SuggestPutawayBin))]
        public bool IsSuggestPutawayReceiving { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_SuggestProductMoveBin))]
        public bool IsSuggestDirectMoveBin { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Handheld_ProductLocationOnMove))]
        public bool IsDisplayLocationOnMove { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Receiving_Handheld_ShowExpectedQuantity))]
        public bool PromptExpectedQuantityOnReceiving { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Fulfillment_Picking_FullPackPickingOrderQuantityLimit))]
        public int FullPackPickingOrderQuantityLimit { get; set; }

        [ConfigValue(ConfigValue = nameof(ConfigConstants.Business_Fulfillment_Picking_FullPackPickingQuantityLimit))]
        public int FullPackPickingQuantityLimit { get; set; }

        private string _tenant;
        public string Tenant
        {
            get
            {
                if (_tenant != null) 
                    return _tenant;
                var file = Path.Combine(FileSystem.AppDataDirectory, "tenant.txt");
                if (!File.Exists(file))
                    return null;
                _tenant = Utils.ReadTextFile(Path.Combine(FileSystem.AppDataDirectory, "tenant.txt")).Trim();
                return _tenant;
            }
            set
            {
                var file = Path.Combine(FileSystem.AppDataDirectory, "tenant.txt");
                if (string.IsNullOrWhiteSpace(value))
                {
                    if(File.Exists(file))
                        File.Delete(file);
                    _tenant = null;
                }
                else
                {
                    Utils.WriteTextFile(file, value);
                    _tenant = value;
                }
            }
        }
        
        public Dictionary<string, string> Translations { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> DirtyTranslations { get; set; } = new Dictionary<string, string>();

        public async Task Login(string username, string password)
        {
            try
            {
                Main.SetLoading();
                Translations = null;
                Singleton<Web>.Instance.AuthenticationToken = await Singleton<Web>.Instance.PostInvokeAsync($"api/Auth/Login?source={AuditSubType.LoginHandheld}", new
                {
                    username,
                    password,
                    Tenant
                }, true, false);

                //Translations
                if (DirtyTranslations.Any())
                {
                    await Singleton<Web>.Instance.PostInvokeAsync("api/Lang/CreateOrUpdate", DirtyTranslations);
                    DirtyTranslations.Clear();
                }

                Translations = await Singleton<Web>.Instance.GetInvokeAsync<Dictionary<string, string>>($"api/Lang/GetTokens", true);

                //GetCurrent and Menu
                var data = await Singleton<Web>.Instance.GetInvokeAsync<LoginDetails>($"api/Auth/GetCurrent?source=handheld", true);
                if (!data.AgreementSigned)
                    throw new ExceptionLocalized("License Agreement have not been signed, Handheld is disabled!");
                PropMapper<LoginDetails, Context>.CopyTo(data, this);

                //await new GeoLocationBackgroundWorker(() => null).StartJob();

                Main.SetLoggedUsername();

                var hhMenu = Pro4Soft.DataTransferObjects.Dto.Generic.Menu.GetById(Singleton<Context>.Instance.Menu, "Handheld");

                static void RecursiveParentBuild(Menu root)
                {
                    foreach (var child in root.Children)
                    {
                        child.Parent = root;
                        RecursiveParentBuild(child);
                    }
                }

                if (hhMenu == null)
                    throw new ExceptionLocalized("Handheld module has not been setup for this user");

                hhMenu.Children.ForEach(RecursiveParentBuild);
                RootMenu = hhMenu.Children;
                MenuChildren.Clear();
                foreach (var menu in hhMenu.Children)
                    MenuChildren.Add(menu);
                MenuParent = null;
                Main.SetChatVisible(NewMessages > 0);

                //Apply configs
                var configs = typeof(Context).GetProperties().Select(c => new
                {
                    Property = c,
                    Attribute = Attribute.GetCustomAttribute(c, typeof(ConfigValueAttribute)) as ConfigValueAttribute
                }).Where(c => c.Attribute != null).ToList();

                var configValues = await Singleton<Web>.Instance.PostInvokeAsync<List<ConfigEntry>>($"data/configs", configs.Select(c => c.Attribute.ConfigValue), true);
                configs.ForEach(c =>
                {
                    var config = configValues.SingleOrDefault(c1 => c.Attribute.ConfigValue == c1.Name);
                    if (config == null)
                        return;

                    switch (c.Property.PropertyType.Name)
                    {
                        case nameof(Boolean):
                            c.Property.SetValue(this, config.BoolValue);
                            break;
                        case nameof(String):
                            c.Property.SetValue(this, config.StringValue);
                            break;
                        case nameof(Double):
                            c.Property.SetValue(this, config.DoubleValue);
                            break;
                        case nameof(Int32):
                            c.Property.SetValue(this, config.IntValue);
                            break;
                    }
                });

                //Tasks
                Singleton<WebEventListener>.Instance.Start();
                Singleton<WebEventListener>.Instance.Subscribe(RaiseEventConstants.TasksChanged, async payload =>
                {
                    var serverTsks = await Singleton<Web>.Instance.GetInvokeAsync<List<UserTask>>("hh/lookup/GetMyTasks", true);
                    Tasks.Clear();
                    foreach (var task in serverTsks)
                        Tasks.Add(task);
                    Main.DispatchThread.BeginInvokeOnMainThread(Main.SetTaskVisibility);
                    if (payload)
                        Singleton<Context>.Instance.NewTaskSound.Play();
                    else
                        Singleton<Context>.Instance.TaskRemovedSound.Play();
                });
                var serverTasks = await Singleton<Web>.Instance.GetInvokeAsync<List<UserTask>>("hh/lookup/GetMyTasks", true);
                foreach (var task in serverTasks)
                    Tasks.Add(task);
                Main.SetTaskVisibility();

                //Chat
                Singleton<Context>.Instance.ChatMessages.Clear();
                Singleton<WebEventListener>.Instance.Subscribe(RaiseEventConstants.UserChatMessage, payload =>
                {
                    Singleton<Context>.Instance.AddUserMessage(new UserMessage
                    {
                        MessageId = payload.MessageId,
                        ToUserId = payload.ToUserId,
                        Message = payload.Message,
                        FromUserId = payload.FromUserId,
                        FromUsername = payload.FromUsername,
                        Timestamp = payload.Timestamp,
                        ToUsername = payload.ToUsername,
                    }, message =>
                    {
                        if (message.FromUserId == Singleton<Context>.Instance.UserId)
                            return;
                        Singleton<Context>.Instance.NewMessageSound.Play();
                        Main.SetChatVisible(true);
                    });
                });

                Singleton<WebEventListener>.Instance.Subscribe(RaiseEventConstants.ScreenRequested, async payload =>
                {
                    await UploadScreen();
                });
            }
            finally
            {
                Main.SetFinishedLoading();
            }
        }

        public async Task Logout()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Singleton<Web>.Instance.AuthenticationToken))
                    return;

                Main.SetLoading();
                Singleton<WebEventListener>.Instance.Stop();
                if (DirtyTranslations.Any())
                {
                    try { await Singleton<Web>.Instance.PostInvokeAsync("api/Lang/CreateOrUpdate", DirtyTranslations, true); }
                    catch {}
                }
                await Singleton<Web>.Instance.GetInvokeAsync($"api/Auth/Logout", true);
            }
            catch{}
            finally
            {
                Singleton<Web>.Instance.AuthenticationToken = null;
                Translations?.Clear();
                DirtyTranslations?.Clear();
                RootMenu?.Clear();
                MenuChildren?.Clear();
                MenuParent = null;
                Contacts?.Clear();
                ChatMessages?.Clear();
                Tasks?.Clear();
                TrackGeoLocation = null;
                Main.SetFinishedLoading();
            }
        }

        public async Task UploadScreen()
        {
            try
            {
                var screenshot = await Screenshot.CaptureAsync();
                await Singleton<Web>.Instance.UploadStream($"hh/userSession/UpdateScreen", await screenshot.OpenReadAsync(), true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #region Menu management
        public List<Menu> RootMenu { get; set; } = new List<Menu>();
        public List<Menu> MenuChildren { get; set; } = new List<Menu>();
        public Menu MenuParent { get; set; }
        public Lazy<List<ViewDescriptor>> AvailableControllers = new Lazy<List<ViewDescriptor>>(() =>
            typeof(BaseViewController).Assembly.GetTypes()
                .Select(c =>
                {
                    var attr = c.GetCustomAttributes(typeof(ViewControllerAttribute)).SingleOrDefault() as ViewControllerAttribute;
                    if (attr == null)
                        return null;
                    return new ViewDescriptor
                    {
                        ViewType = attr.ViewType,
                        ControllerType = c,
                        StateName = attr.StateName
                    };
                })
                .Where(c => c != null)
                .ToList()
        );
        #endregion

        #region Chat management
        public ObservableCollection<Contact> Contacts { get; } = new ObservableCollection<Contact>();
        public ConcurrentDictionary<Guid, ObservableCollection<UserMessage>> ChatMessages = new ConcurrentDictionary<Guid, ObservableCollection<UserMessage>>();
        public void AddUserMessage(UserMessage message, Action<UserMessage> syncCallback = null)
        {
            var key = message.FromUserId;
            if (UserId == key)
                key = message.ToUserId;
            if (!ChatMessages.ContainsKey(key))
                ChatMessages[key] = new ObservableCollection<UserMessage>();
            if (ChatMessages[key].Any(c => c.MessageId == message.MessageId))
                return;

            if (syncCallback == null)
            {
                ChatMessages[key].Add(message);
                ChatMessages[key].Sort(col => col.OrderBy(c => c.Timestamp));
            }
            else
            {
                Main.DispatchThread.BeginInvokeOnMainThread(() =>
                {
                    ChatMessages[key].Add(message);
                    ChatMessages[key].Sort(c => c.OrderBy(c1 => c1.Timestamp));
                    syncCallback.Invoke(message);
                });
            }
        }
        #endregion

        #region Task management
        public ObservableCollection<UserTask> Tasks { get; } = new ObservableCollection<UserTask>();
        public void AddTask(UserTask task, Action<UserTask> syncCallback = null)
        {
            if (Tasks.Any(c => c.Id == task.Id))
                return;

            if (syncCallback == null)
            {
                Tasks.Add(task);
                Tasks.Sort(col => col.OrderBy(c => c.Priority).ThenBy(c => c.DateCreated));
            }
            else
            {
                Main.DispatchThread.BeginInvokeOnMainThread(() =>
                {
                    Tasks.Add(task);
                    Tasks.Sort(col => col.OrderBy(c => c.Priority).ThenBy(c => c.DateCreated));
                    syncCallback.Invoke(task);
                });
            }
        }

        public void RemoveTask(UserTask task, Action<UserTask> syncCallback = null)
        {
            if (!Tasks.Contains(task))
                return;

            if (syncCallback == null)
            {
                Tasks.Remove(task);
            }
            else
            {
                Main.DispatchThread.BeginInvokeOnMainThread(() =>
                {
                    Tasks.Remove(task);
                    syncCallback.Invoke(task);
                });
            }
        }
        #endregion
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigValueAttribute : Attribute
    {
        public string ConfigValue { get; set; }
    }
}