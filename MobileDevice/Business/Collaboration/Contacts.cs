using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens;
using RestSharp;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Business.Collaboration
{
    [ViewController("main.contacts", ViewType = typeof(ContactView))]
    public class Contacts : BaseViewController
    {
        public override string Title => "Messages";

        private ContactView View => CustomView as ContactView;

        protected override async Task Init()
        {
            try
            {
                View.OnTapped = async contact =>
                {
                    await Main.NavigateToController<Chat>(c =>
                    {
                        if (c is Chat chat)
                            chat.Contact = contact;
                    });
                };

                Main.SetLoading();
                var contacts = await Singleton<Web>.Instance.GetInvokeAsync<List<Contact>>($"api/UserMessageApi/GetContacts", true);
                if (Singleton<Context>.Instance.UserId != null)
                {
                    var resp = await Singleton<Web>.Instance.WebInvokeAsync($"/resource/user/{Singleton<Context>.Instance.UserId}/medium", Method.Get, null, true);
                    Singleton<Context>.Instance.UserAvatars[Singleton<Context>.Instance.UserId.Value] = ImageSource.FromStream(() => new MemoryStream(resp.RawBytes));
                }

                foreach (var contact in contacts)
                {
                    if (!Singleton<Context>.Instance.UserAvatars.ContainsKey(contact.Id))
                    {
                        var resp = await Singleton<Web>.Instance.WebInvokeAsync($"/resource/user/{contact.Id}/medium", Method.Get, null, true);
                        Singleton<Context>.Instance.UserAvatars[contact.Id] = ImageSource.FromStream(() => new MemoryStream(resp.RawBytes));
                    }

                    await Console.Out.WriteLineAsync($"{contact.Id} - {contact.Username}");
                }

                Singleton<Context>.Instance.Contacts.Clear();
                foreach (var contact in contacts)
                    Singleton<Context>.Instance.Contacts.Add(contact);
            }
            finally
            {
                Main.SetFinishedLoading();
            }
        }

        public override void OnClosing()
        {
            
        }
    }
}