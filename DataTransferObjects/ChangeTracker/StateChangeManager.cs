using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pro4Soft.Common.ChangeTracker
{
    public class StateChangeManager
    {
        public List<IChangeSubscriber> Subscribers { get; } = new List<IChangeSubscriber>();

        public void RegisterEventSubscribers(IChangeSubscriber subscriber)
        {
            if(!Subscribers.Contains(subscriber))
                Subscribers.Add(subscriber);
        }

        public async Task PublishEvent(object sender, ObjectChangeEvent changeEvent, object entityObject, bool beforeSave)
        {
            var exceptions = new List<Exception>();
            foreach (var subscriber in Subscribers.OrderByDescending(c => c.Precedence))
            {
                try
                {
                    await subscriber.HandleChange(sender, changeEvent, entityObject, beforeSave);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Any())
                throw new AggregateException("Subscribers failed", exceptions);
        }
    }
}