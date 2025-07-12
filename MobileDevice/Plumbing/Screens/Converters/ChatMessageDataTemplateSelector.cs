using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;
using Pro4Soft.MobileDevice.Plumbing.Screens.Controls;
using Xamarin.Forms;

namespace Pro4Soft.MobileDevice.Plumbing.Screens.Converters
{
    public class ChatMessageDataTemplateSelector : DataTemplateSelector
    {
        public ChatMessageDataTemplateSelector()
        {
            // Retain instances!
            _incomingDataTemplate = new DataTemplate(typeof(IncomingViewCell));
            _outgoingDataTemplate = new DataTemplate(typeof(OutgoingViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is UserMessage messageVm))
                return null;
            if (messageVm.FromUserId == Singleton<Context>.Instance.UserId)
                return _outgoingDataTemplate;
            return _incomingDataTemplate;
        }

        private readonly DataTemplate _incomingDataTemplate;
        private readonly DataTemplate _outgoingDataTemplate;
    }
}
