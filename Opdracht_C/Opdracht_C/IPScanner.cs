using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Sockets;

namespace Project_Green
{
    public class IPScanner
    {
        Regex regex = new Regex("^.+(?=\\.\\d+$)");
        IPAddress[] addresses = Dns.GetHostAddresses(Dns.GetHostName());
        List<string> strings = new List<string>();

        public List<string> Getstrings()
        {
            strings.Clear();
            var address = regex.Match(addresses[0].ToString()).ToString();
            var iplist = new List<string>();
            for (int i = 2; i < 20; i++)
                iplist.Add($"{address}.{i}");

            List<Thread> threads = new List<Thread>();
            foreach (var ip in iplist)
            {
                var tws = new ThreadWithState(
                    ip,
                    new ThreadCallback(ResultCallback)
                );

                Thread t = new Thread(new ThreadStart(tws.ThreadProc));
                threads.Add(t);
                t.Start();//start thread and pass it the port            
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            return strings;
        }

        public void ResultCallback(string gr)
        {
            strings.Add(gr);
        }
    }



    // The ThreadWithState class contains the information needed for
    // a task, the method that executes the task, and a delegate
    // to call when the task is complete.
    //
    public class ThreadWithState
    {
        // State information used in the task.
        private string host;

        // Delegate used to execute the callback method when the
        // task is complete.
        private ThreadCallback callback;

        // The constructor obtains the state information and the
        // callback delegate.
        public ThreadWithState(string h, ThreadCallback callbackDelegate)
        {
            host = h;
            callback = callbackDelegate;
        }

        // The thread procedure performs the task, such as
        // formatting and printing a document, and then invokes
        // the callback delegate with the number of lines printed.
        public void ThreadProc()
        {
            Ping ping = new Ping();
            PingReply pingReply;

            pingReply = ping.Send(host, 500);

            if (pingReply != null && pingReply.Status == IPStatus.Success)
            {
                try
                {
                    var ipAddress = IPAddress.Parse(host);
                    var socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    // Connect using a timeout (5 seconds)
                    var result = socket.BeginConnect(ipAddress, 8000, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(500, true);
                    if (socket.Connected)
                    {
                        callback(host);
                        socket.EndConnect(result);
                    }
                    else
                    {
                        // NOTE, MUST CLOSE THE SOCKET
                        socket.Close();
                    }

                }
                catch { }
            }
        }
    }
    // Delegate that defines the signature for the callback method.
    //
    public delegate void ThreadCallback(string gr);
}

