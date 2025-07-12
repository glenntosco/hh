using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Pro4Soft.DataTransferObjects.Dto;
using Pro4Soft.MobileDevice.Plumbing.Infrastructure;

namespace Pro4Soft.MobileDevice.Plumbing
{
    public class WebEventListener
    {
        private static readonly object _syncObject = new object();

        private HubConnection _hub;
        private IHubProxy _proxy;

        private readonly ConcurrentDictionary<RaiseEventConstants, List<Action<dynamic>>> _subscribers = new ConcurrentDictionary<RaiseEventConstants, List<Action<dynamic>>>();

        public void Subscribe(RaiseEventConstants eventName, Action<dynamic> callback)
        {
            if (!_subscribers.ContainsKey(eventName))
            {
                _subscribers[eventName] = new List<Action<dynamic>>();
                _proxy?.On(eventName.ToString(), payload =>
                {
                    if (!_subscribers.TryGetValue(eventName, out var list))
                        return;
                    foreach (var action in list)
                        action.Invoke(payload);
                });
            }

            if(!_subscribers[eventName].Contains(callback))
                _subscribers[eventName].Add(callback);
        }

        public void Unsubscribe(RaiseEventConstants eventName, Action<dynamic> callback)
        {
            if (!_subscribers.TryGetValue(eventName, out var callbacks))
                return;
            callbacks.Remove(callback);
        }

        public void Start(bool restart = false)
        {
            if (restart && _manuallyStopped)
                return;
            _manuallyStopped = false;
            lock (_syncObject)
            {
                _hub ??= new HubConnection(Singleton<Web>.Instance.BaseUrl, true);

                if (_proxy == null)
                {
                    _proxy = _hub.CreateHubProxy("SubscriberHub") as HubProxy;
                    foreach (var key in _subscribers.Keys)
                        _proxy.On(key.ToString(), payload =>
                        {
                            if (_subscribers.TryGetValue(key, out var list))
                                list.ForEach(c => c.Invoke(payload));
                        });
                }

                _hub.Closed += () =>
                {
                    _hub = null;
                    _proxy = null;
                    Start(true);
                };

                _hub.StateChanged += s =>
                {
                    if (s.NewState != ConnectionState.Connected) 
                        return;
                    _proxy.Invoke("JoinGroup", Singleton<Context>.Instance.UserId);
                };

                if (_hub.State == ConnectionState.Disconnected)
                {
                    try
                    {
                        _hub.Start();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private bool _manuallyStopped;
        public void Stop()
        {
            _manuallyStopped = true;
            _subscribers.Clear();
            _hub?.Stop(TimeSpan.FromSeconds(5));
        }
    }
}