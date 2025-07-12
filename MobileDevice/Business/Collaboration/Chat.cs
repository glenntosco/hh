using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;

namespace Pro4Soft.MobileDevice.Business.Collaboration
{
    [ViewController(typeof(ChatView))]
    public class Chat : BaseViewController
    {
        public override string Title => "Chat";

        private ChatView View => CustomView as ChatView;

        public Contact Contact { get; set; }

        protected override async Task Init()
        {
            View.OnSendMessage = async msg =>
            {
                await Singleton<Web>.Instance.PostInvokeAsync($"api/UserMessageApi/SendMessage", new UserMessage
                {
                    ToUserId = Contact.Id,
                    Message = msg
                });
            };
            var messages = await Singleton<Web>.Instance.GetInvokeAsync<List<UserMessage>>($"api/UserMessageApi/GetMessages?fromUserId={Contact.Id}");
            foreach (var message in messages)
                Singleton<Context>.Instance.AddUserMessage(message);
            View.ToContact = Contact;
            Main.SetChatVisible(false);
        }

        public override async void OnClosing()
        {
            try
            {
                await Singleton<Web>.Instance.GetInvokeAsync($"api/UserMessageApi/SetRead?contactId={Contact.Id}");
            }
            catch { }
        }
    }
}