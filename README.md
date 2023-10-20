Having `App.xaml.cs` open **two** windows and debugging it will create an **'Access violation'** while starting without debugging **does not**...

Starting with or without debugger attached works fine if **only one** window is created.


The solution contains different projects all behaving the same way:

- TestInWindow
  + `TextBox` bound to a property of `Window` created using `CommunityToolkit.MVVM` . The property value is updated from an `Actor` dispatched synchronized.
- TestInWindowDirectSet
  + `TestBox.Text` set from an `Actor` dispatched synchronized.
- TestViaDispatcherQueue
  + `TextBox` bound to a property of `Window` created using `CommunityToolkit.MVVM` . The property value is updated from an unsynchronized actor via the Windows `DispatcherQueue.TryEnqueue`

All projects above create the Actors within the Windows creation method after this.InitializeComponent()

- TestActorCreatedByActor
  + `TextBox` bound to a property of `Window` created using `CommunityToolkit.MVVM` . The property value is updated from an unsynchronized actor via the Windows `DispatcherQueue.TryEnqueue`
  + the `Actor` is created from another already running actor
- TestActorCreatedByActorOnPageLoaded
  + same as TestActorCreatedByActor but the `TextBox` is on its own Page. The message to create the `Actor` updating its value is create after the `Page` is loaded

The debug output is:
*before* *after* the Actor is created / sending a message to create
*no context!* if `SynchronizationContext.Current == null` before creating the Actor / sending a message to create



```
'TestInWindow.exe' (CoreCLR: DefaultDomain): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Private.CoreLib.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\TestInWindow.dll'. Symbols loaded.
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Runtime.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\WinRT.Runtime.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Runtime.InteropServices.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Collections.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Collections.Concurrent.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Threading.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\Microsoft.WinUI.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Runtime.CompilerServices.Unsafe.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Private.Uri.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.ObjectModel.dll'. Symbols loaded.
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.ComponentModel.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Numerics.Vectors.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\Microsoft.Windows.SDK.NET.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Security.Cryptography.Algorithms.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Security.Cryptography.Primitives.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Linq.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Memory.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\Microsoft.InteractiveExperiences.Projection.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Linq.Expressions.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Reflection.Emit.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'Snippets'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\Akka.dll'. Symbols loaded.
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Collections.Immutable.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Text.RegularExpressions.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Console.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Reflection.Emit.ILGeneration.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Reflection.Emit.Lightweight.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Reflection.Primitives.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Threading.Thread.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\Newtonsoft.Json.dll'. Symbols loaded.
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\netstandard.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Runtime.Serialization.Formatters.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\Microsoft.Extensions.ObjectPool.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Diagnostics.TraceSource.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'E:\vstmp\AkkaSynchronizedDispatcherBug\TestInWindow\bin\x86\Debug\net6.0-windows10.0.19041.0\win10-x86\AppX\System.Threading.ThreadPool.dll'. 
'TestInWindow.exe' (CoreCLR: clrhost): Loaded 'Anonymously Hosted DynamicMethods Assembly'. 
The program '[55328] TestInWindow.exe' has exited with code 3221225477 (0xc0000005) 'Access violation'.
```