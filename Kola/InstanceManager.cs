using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO.Pipes;
using System.IO;

namespace Kola
{

    public class InstanceArgsEventArgs : EventArgs
    {
        public string[] Args { get; private set; }
        public InstanceArgsEventArgs(string[] args)
        {
            Args = args;
        }
    }
    public class InstanceManager
    {
        public event EventHandler<InstanceArgsEventArgs> ReceiveArgs;
        public bool IsServer { get; private set; }
        public bool IsOpen { get; private set; }

        private string id;
        private Mutex mutex;

        const string separator = ";";
        public InstanceManager(string uniqueID)
        {
            id = uniqueID;
            mutex = new Mutex(false, uniqueID);
            IsOpen = false;
        }
        ~InstanceManager()
        {
            mutex.Dispose();

        }
        public void Start(string[] args)
        {
            IsServer = mutex.WaitOne(0);
            

            if(IsServer)
            {
                IsOpen = true;
                StartServer();
                ReceiveArgs?.Invoke(this, new InstanceArgsEventArgs(args));
            }
            else
            {
                sendToServer(args);
            }
        }

        public void Close()
        {
            if(IsServer)
            {
                mutex.ReleaseMutex();
            }
            IsOpen = false;
        }

        private async void StartServer()
        {
            using (NamedPipeServerStream pipe = new NamedPipeServerStream(id, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous))
            {
                using (StreamReader reader = new StreamReader(pipe))
                {
                    while (IsOpen)
                    {
                        if(!pipe.IsConnected)
                        {
                            await pipe.WaitForConnectionAsync();
                        }
                        if(!reader.EndOfStream)
                        {
                            string s = await reader.ReadLineAsync();
                            ReceiveArgs?.Invoke(this, new InstanceArgsEventArgs(s.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries)));
                        }
                        else
                        {
                            pipe.Disconnect();
                        }
                    }
                }
            }
        }

        private void sendToServer(string[] args)
        {
            using(NamedPipeClientStream pipe = new NamedPipeClientStream(".", id, PipeDirection.Out))
            {
                pipe.Connect();
                using (StreamWriter writer = new StreamWriter(pipe))
                {
                    writer.WriteLine(string.Join(separator, args));
                    pipe.WaitForPipeDrain();
                }
            }

        }
    }
}
