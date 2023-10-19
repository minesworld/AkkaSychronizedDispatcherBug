﻿using Akka.Actor;
using Akka.Configuration;
using Microsoft.UI.Xaml;
using System;
using System.IO;
using Windows.ApplicationModel;

namespace AkkaSychronizedDispatcherBug
{
    public static class Creator
    {
        static public ActorSystem Instance { get; private set; }
        static public void CreateActorSystem()
        {
            var configPath = System.IO.Path.Combine(Package.Current.InstalledPath, "Akka.conf");
            var configText = File.ReadAllText(configPath);
            var akkaConfig = ConfigurationFactory.ParseString(configText);
            Instance = ActorSystem.Create("test", akkaConfig);
        }

        static public void CreateActorsDisplayingCounted_(ICounted countable)
        {
            var displayRef = Instance.ActorOf(Props.Create(() => new CounterDisplayActor(countable)).WithDispatcher("synchronized-dispatcher"));
            var timeSourceRef = Instance.ActorOf(Props.Create(() => new TimeSourceActor(displayRef)));
        }
    }
    

    public class TimeSourceActor : ReceiveActor, IWithTimers
    {
        private class Tick
        {
            static public Tick Instance { get; private set; } = new Tick();
        }

        public ITimerScheduler Timers { get; set; }

        protected int counter = 0;

        protected IActorRef ReceiverRef { get; set; }

        override protected void PreStart()
        {
            Become(Working); // makes no difference - could be declared in WebView2Actor().

            Timers.StartPeriodicTimer("tick", Tick.Instance, TimeSpan.FromSeconds(1));
        }

        public TimeSourceActor(IActorRef receiverRef) : base()
        {
            ReceiverRef = receiverRef;
        }

        private void Working()
        {
            Receive<Tick>(message =>
            {
                try
                {
                    counter += 1;
                    ReceiverRef.Tell(counter);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"before: {ex}");
                }
            });
        }
    }

    public interface ICounted
    {
        public string Counted { set; }
    }


    public partial class CounterDisplayActor : ReceiveActor
    {
        protected ICounted Setable { get; init; }

        override protected void PreStart()
        {
            Become(Working); // makes no difference - could be declared in WebView2Actor().
        }

        public CounterDisplayActor(ICounted setable) : base() {
            Setable = setable;
        }

        private void Working()
        {
            Receive<int>(message =>
            {
                Setable.Counted = $"{message} Ticks";
            });
        }
    }
}
