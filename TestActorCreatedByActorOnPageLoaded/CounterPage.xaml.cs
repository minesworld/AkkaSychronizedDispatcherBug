using Akka.Actor;
using AkkaSychronizedDispatcherBug;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TestActorCreatedByActorOnPageLoaded
{
    [ObservableObject]
    public partial class CounterPage : Page, ICounted
    {
        [ObservableProperty]
        private string counted = "no Ticks";

        public CounterPage()
        {
            this.InitializeComponent();

            this.Loaded += CreateActor;
        }

        private void CreateActor(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("before");

            if (SynchronizationContext.Current == null)
            {
                System.Diagnostics.Debug.WriteLine("no context!");
                Debugger.Break();
            }

            // create a TimeSourceActor and CounterDisplayActor which uses this instance to update the Counted value via setter
            Creator.CreatorActorRef.Tell(new CreateActorDisplayingCountedMessage
            {
                DispatcherQueue = DispatcherQueue,
                Countable = this
            }, ActorRefs.NoSender);

            System.Diagnostics.Debug.WriteLine("after");
        }
    }
}
