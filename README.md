Netgao.Telephony.Workflow
=========================

Call Control Workflow Library

c# Sample Code:

    public class CallBrowser : Browser
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(CallBrowser));

        public CallBrowser(CallContext context)
            : base()
        {
            
            Context = context;
            Context.InitializeProperties();
            Context.InitializeDevices();
            Context.InitializeExtensions();
            Context.InitializeTrunks();
            Context.InitializeAttendants();
            Context.InitializeRoutes();

            Workflow = new UccWorkflow(DoWork);
 
            Platform = new CallPlatform();
            Platform.OnInit += new Call.IPlatformEvents_OnInitEventHandler(OnInit);
            Platform.OnRinging += new Call.IPlatformEvents_OnRingingEventHandler(OnRinging);
            Platform.OnDialing += new Call.IPlatformEvents_OnDialingEventHandler(OnDialing);
            Platform.OnMakeCall += new Call.IPlatformEvents_OnMakeCallEventHandler(OnMakeCall);
            Platform.OnReleaseCall += new Call.IPlatformEvents_OnReleaseCallEventHandler(OnReleaseCall);
            Platform.OnCompleted += new Call.IPlatformEvents_OnCompletedEventHandler(OnCompleted);
            Platform.Initialize(Context);

        }

        public UccWorkflow Workflow
        {
            get;
            private set;
        }

        ....

        private void OnInit(Call.ITerminal terminal)
        {
            using (TelephonyContext call = new TelephonyContext(Context.BoundedContext))
            {
                Terminal term = call.Terminals.Query().FirstOrDefault(p => p.Pad == terminal.Pad);
                if (term != null)
                {
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_ID, term.Id);
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_NAME, term.Name);
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_RECORDING, term.Recording);
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_SAVEPATH, term.SavePath);
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_SAVEFILE, term.SaveFile);
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_CLEARING, term.Clearing);
                    terminal.SetValue(Call.TERMINAL_NAME.TERMINAL_NAME_DISKSIZE, term.DiskSize);

                    Call.IAddressCollection addresses = terminal.Addresses as Call.IAddressCollection;

                    foreach (Extension agent in term.Agents)
                    {
                        Call.IAddress address = Platform.Addresses.Find(agent.Number);
                        if (address != null)
                        {
                            addresses.Add(address);
                        }
                    }

                    foreach (Extension extension in term.Extensions)
                    {
                        Call.IAddress address = Platform.Addresses.Find(extension.Number);
                        if (address != null)
                        {
                            addresses.Add(address);
                        }
                    }

                    foreach (Trunk trunk in term.Trunks)
                    {
                        Call.IAddress address = Platform.Addresses.Find(trunk.Number);
                        if (address != null)
                        {
                            addresses.Add(address);
                        }
                    }
                }
            }
        }


        private void OnMakeCall(Call.ITerminal terminal, Call.ICall call)
        {
            if (call.Automated)
            {
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    try
                    {
                        UccTerminalService service = Workflow.WorkflowRuntime.GetService<UccTerminalService>();
                        if (service != null)
                        {
                            if (call.Exclusions.Count > 0)
                            {
                                Call.ILine line = call.Exclusions[0] as Call.ILine;
                                if (line != null)
                                {
                                    Workflow.StartWorkflow(service, terminal.UniqueId, terminal.CreateArguments(), (string)line.Address.GetValue(Call.ADDRESS_NAME.ADDRESS_NAME_WORKFLOW));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(String.Format("RaiseEvent Error: OnWorkStart({0}) - {1}", call.Publisher.Terminal.Pad, ex.Message), ex);
                    }
                });
            }
        }

        private void OnReleaseCall(Call.ITerminal terminal, Call.ICall call)
        {
            if (call.Automated)
            {
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    try
                    {
                        UccTerminalService service = Workflow.WorkflowRuntime.GetService<UccTerminalService>();
                        if (service != null)
                        {
                            Workflow.CloseWorkflow(service, terminal.UniqueId);
                        }

                    }
                    catch (EventDeliveryFailedException ex)
                    {
                        logger.Error(String.Format("RaiseEvent Error: OnWorkEnd({0}) - {1}", call.Publisher.Terminal.Pad, ex.Message), ex);
                    }
                });
            }
        }

        private void OnRinging(Call.ITerminal terminal, int ringCount)
        {

            try
            {
                UccTerminalService service = Workflow.WorkflowRuntime.GetService<UccTerminalService>();
                if (service != null)
                {
                    var instance = Workflow.CreateChannel(service, terminal.UniqueId, terminal.CreateArguments());
                    if (instance != null)
                    {
                        service.RaiseStartedEvent(new RingingEventArgs(instance, ringCount));
                    }
                }
            }
            catch (EventDeliveryFailedException ex)
            {
                logger.Error(String.Format("RaiseEvent Error: OnRinging({0}, {1}) - {2}", terminal.Pad, ringCount, ex.Message), ex);
            }
        }

        private void OnDialing(Call.ITerminal terminal, string dtmfString)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    UccTerminalService service = Workflow.WorkflowRuntime.GetService<UccTerminalService>();
                    if (service != null)
                    {
                        var instance = Workflow.CreateChannel(service, terminal.UniqueId, terminal.CreateArguments());
                        if (instance != null)
                        {
                            service.RaiseStartedEvent(new DialingEventArgs(instance, dtmfString));
                        }
                    }
                }
                catch (EventDeliveryFailedException ex)
                {
                    logger.Error(String.Format("RaiseEvent Error: OnDialing({0}, \"{1}\") - {2}", terminal.Pad, dtmfString, ex.Message), ex);
                }
            });
        }

        private void OnCompleted(Call.ITerminal terminal)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    UccTerminalService service = Workflow.WorkflowRuntime.GetService<UccTerminalService>();
                    if (service != null)
                    {
                        var instance = Workflow.CreateChannel(service, terminal.UniqueId, terminal.CreateArguments());
                        if (instance != null)
                        {
                            service.RaiseCompletedEvent(new CompletedEventArgs(instance, 0));
                        }
                    }
                }
                catch (EventDeliveryFailedException ex)
                {
                    logger.Error(String.Format("RaiseEvent Error: OnCompleted({0}, {1}) - {2}", terminal.Pad, -1, ex.Message), ex);
                }
            });

        }

        private void DoWork(TerminalEventArgs e)
        {
            try
            {
                Call.ITerminal terminal = Platform.Terminals[Int32.Parse(e.Channel.Pad)];
                if (terminal != null)
                {

                    if (e is PickupEventArgs)
                    {
                        terminal.Pickup();
                    }
                    if (e is HangupEventArgs)
                    {
                        terminal.Hangup(Call.HANGUP_CAUSE.HANGUP_CAUSE_NETWORK);
                    }
               
                    if (e is ClearEventArgs)
                    {
                        terminal.Clear();
                    }

                    if (e is BlindTransferEventArgs)
                    {
                        terminal.BlindTransfer((e as BlindTransferEventArgs).CalleeId);
                    }

                    if (e is PlayEventArgs)
                    {
                        terminal.PlayVoice((e as PlayEventArgs).FileName, (e as PlayEventArgs).StopOnDTMF ?
                                    (Call.MEDIA_SOUND.MEDIA_SND_FILENAME | Call.MEDIA_SOUND.MEDIA_SND_SYNC | Call.MEDIA_SOUND.MEDIA_SND_STOPONDTMF) :
                                    (Call.MEDIA_SOUND.MEDIA_SND_FILENAME | Call.MEDIA_SOUND.MEDIA_SND_SYNC));
                    }
                    if (e is RecordEventArgs)
                    {
                        terminal.PlayVoice((e as PlayEventArgs).FileName, (e as PlayEventArgs).StopOnDTMF ?
                                    (Call.MEDIA_SOUND.MEDIA_SND_FILENAME | Call.MEDIA_SOUND.MEDIA_SND_SYNC | Call.MEDIA_SOUND.MEDIA_SND_STOPONDTMF) :
                                    (Call.MEDIA_SOUND.MEDIA_SND_FILENAME | Call.MEDIA_SOUND.MEDIA_SND_SYNC));
                    }
                }
                logger.InfoFormat("Debug Output: {0} {1}", e.ToString(), e.InstanceId);
            }
            catch (Exception ex)
            {
                logger.ErrorFormat("DoWork:{0} {1}", ex.Message, ex.StackTrace);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Platform != null)
                {
                    Platform.Shutdown();
                    IDisposable disp = Platform as IDisposable;
                    if (disp != null)
                    {
                        disp.Dispose();
                    }
                }

                if (Context != null)
                {
                    IDisposable disp = Context as IDisposable;
                    if (disp != null)
                    {
                        disp.Dispose();
                    }
                }
            }

            base.Dispose(disposing);
        }
    }

