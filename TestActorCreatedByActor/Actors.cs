using Akka.Actor;
using Akka.Configuration;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using Windows.ApplicationModel;
using Windows.System;

namespace AkkaSychronizedDispatcherBug
{
    public static class Creator
    {
        static public ActorSystem Instance { get; private set; }

        static public IActorRef CreatorActorRef { get; private set; }
        static public void CreateActorSystem()
        {
            var configPath = System.IO.Path.Combine(Package.Current.InstalledPath, "Akka.conf");
            var configText = File.ReadAllText(configPath);
            var akkaConfig = ConfigurationFactory.ParseString(configText);
            Instance = ActorSystem.Create("test", akkaConfig);

            CreatorActorRef = Instance.ActorOf(Props.Create(() => new CreatorActor()));
        }

    }


    class CreateActorDisplayingCountedMessage
    {
        public Microsoft.UI.Dispatching.DispatcherQueue DispatcherQueue { get; init; }
        public ICounted Countable { get; init; }
    }

    public class CreatorActor : ReceiveActor
    {
        override protected void PreStart()
        {
            Become(Working); 
        }

        public CreatorActor() : base()
        {
        }

        private void Working()
        {
            Receive<CreateActorDisplayingCountedMessage>(message =>
            {
                var displayRef = Context.ActorOf(Props.Create(() => new CounterDisplayActor(message.DispatcherQueue, message.Countable)));
                var timeSourceRef = Context.ActorOf(Props.Create(() => new TimeSourceActor(displayRef)));
            });
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
                counter += 1;
                ReceiverRef.Tell(counter);
            });
        }
    }

    public interface ICounted
    {
        public string Counted { set; }
    }


    public partial class CounterDisplayActor : ReceiveActor
    {
        protected Microsoft.UI.Dispatching.DispatcherQueue DispatcherQueue { get; init; }
        protected ICounted Setable { get; init; }

        override protected void PreStart()
        {
            Become(Working); // makes no difference - could be declared in WebView2Actor().
        }

        public CounterDisplayActor(Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue, ICounted setable) : base() 
        {
            DispatcherQueue = dispatcherQueue;
            Setable = setable;
        }

        private void Working()
        {
            Receive<int>(message =>
            {
                DispatcherQueue.TryEnqueue(() => Setable.Counted = $"{message} Ticks");
            });
        }
    }
}
