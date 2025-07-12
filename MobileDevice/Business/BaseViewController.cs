using System;
using System.Threading.Tasks;
using Pro4Soft.DataTransferObjects.Dto.Collaboration;
using Pro4Soft.DataTransferObjects.Dto.Generic;
using Pro4Soft.MobileDevice.Plumbing;
using Pro4Soft.MobileDevice.Plumbing.Screens;

namespace Pro4Soft.MobileDevice.Business
{
    public abstract class BaseViewController
    {
        public Menu MenuItem { get; set; }
        public abstract string Title { get; }

        public UserTask AssignedTask { protected get; set; }
        public virtual BaseContentView CustomView { get; set; }

        public virtual async Task InitBase()
        {
            try
            {
                await Init();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        protected abstract Task Init();
        
        public virtual void OnClosing()
        {

        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ViewControllerAttribute : Attribute
    {
        public string StateName { get; }
        public Type ViewType { get; set; }

        public ViewControllerAttribute():this(null, typeof(ScanScreenView))
        {
        }

        public ViewControllerAttribute(Type viewType) : this(null, viewType)
        {
        }

        public ViewControllerAttribute(string stateName):this(stateName, typeof(ScanScreenView))
        {

        }

        public ViewControllerAttribute(string stateName, Type viewType)
        {
            StateName = stateName;
            ViewType = viewType;
        }
    }
}
