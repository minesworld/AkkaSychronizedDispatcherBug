using Akka.Util;
using AkkaSychronizedDispatcherBug;
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
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TestInWindowDirectSet
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow(int index)
        {
            this.InitializeComponent();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            Microsoft.UI.Windowing.AppWindow appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

            if (appWindow != null)
            {
                appWindow.Move(new PointInt32(40 + index * 600, 40));
                appWindow.Resize(new SizeInt32(600, 200));
            }

            System.Diagnostics.Debug.WriteLine("before");

            if (SynchronizationContext.Current == null)
            {
                System.Diagnostics.Debug.WriteLine("no context!");
                Debugger.Break();
            }

            // create a TimeSourceActor and CounterDisplayActor which uses this instance to update the Counted value via setter
            Creator.CreateActorsDisplayingCounted_(CountedTextBlock);

            System.Diagnostics.Debug.WriteLine("after");
        }
    }
}
