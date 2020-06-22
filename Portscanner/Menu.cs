using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Portscanner
{



    class Menu
    {

        private static ProtocolType protocol;
        private static int startPort = 0;
        private static string ipAddressString = string.Empty;
        private static int endPort = 0;
        private static string ipAddress = string.Empty;
        private static bool isEndpointHostname = false;
        private static bool isEndpointIPAddress = false;
        private static bool isDNSEntryInLocalDNSCache = false;


        public static void ShowIPAddressMenu()
        {
            Console.WriteLine();

            Console.WriteLine("type IP address or hostname and press enter");
            Console.WriteLine();

            ipAddressString = Console.ReadLine();

            if (IPAddress.TryParse(ipAddressString, out IPAddress iPAddress) == true)
            {
                // string is an ip
                isEndpointIPAddress = true;
            }
            else
            {
                // string is a hostname
                isEndpointHostname = true;
            }

            // tjek om dns adresse er fra lokal dns cache

            List<DNSEntry> dNSEntries = new List<DNSEntry>();

            using (Process process = new Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = "Get-DnsClientCache | fl";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();

                // Synchronously read the standard output of the spawned process.
                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();


                using (StringReader sr = new StringReader(output))
                {
                    string line;

                    int i = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (i == 9)
                        {
                            DNSEntry dNSEntry = new DNSEntry();

                            char[] trimChars = { 'D', 'a', 't', 'a', ' ', ':' };
                            string formattedLine = line.TrimStart(trimChars);

                            string trimmedLine = formattedLine.Trim();

                            dNSEntry.Data = trimmedLine;
                            dNSEntries.Add(dNSEntry);
                            Console.WriteLine(trimmedLine);

                            i = 0;
                        }
                        i++;
                    }
                }

                process.Close();
            }

            IPAddress[] iPAddresses = Dns.GetHostAddresses(ipAddressString);
            ipAddress = iPAddresses[0].ToString();

            foreach (var item in dNSEntries)
            {
                if (item.Data == ipAddress)
                {
                    isDNSEntryInLocalDNSCache = true;
                }
            }


        }

        public static void ShowProtocolMenu()
        {
            bool protocolChosen = false;

            protocol = ProtocolType.Tcp;

            Console.Clear();

            Console.WriteLine("  => tcp");
            Console.WriteLine("     udp");


            while (protocolChosen == false)
            {
                ConsoleKey consoleKey = Console.ReadKey(intercept: true).Key;

                if (consoleKey == ConsoleKey.DownArrow)
                {
                    Console.Clear();

                    Console.WriteLine("     tcp");
                    Console.WriteLine("  => udp");

                    protocol = ProtocolType.Udp;

                }
                else if (consoleKey == ConsoleKey.UpArrow)
                {
                    Console.Clear();

                    Console.WriteLine("  => tcp");
                    Console.WriteLine("     udp");

                    protocol = ProtocolType.Tcp;

                }

                if (consoleKey == ConsoleKey.Enter)
                {
                    protocolChosen = true;

                }
            }
        }

        public static void ShowStartPortMenu()
        {
            bool startPortChosen = false;

            startPort = 0;

            while (startPortChosen == false)
            {
                Console.WriteLine("enter start port");

                Console.WriteLine("example: 80");

                startPort = int.Parse(Console.ReadLine());

                if (startPort > 0 && startPort < 65535)
                {
                    startPortChosen = true;
                }
            }
        }

        public static void ShowEndPortMenu()
        {
            bool endPortChosen = false;

            endPort = 0;

            while (endPortChosen == false)
            {
                Console.WriteLine("enter end port");

                Console.WriteLine("example: 80");

                endPort = int.Parse(Console.ReadLine());

                if (endPort >= startPort && endPort > 0 && endPort < 65535)
                {
                    endPortChosen = true;
                }
            }
        }

        public static void ShowConnectionTestMenu()
        {
            string dnsEntryCacheStatus = "DNS cache status not available";

            if (isDNSEntryInLocalDNSCache == true)
            {
                dnsEntryCacheStatus = "DNS address is resolved by local cache";
            }
            else
            {
                dnsEntryCacheStatus = "DNS address is resolved by DNS server";
            }

            // lav dette om så jeg 


            // det til at virke med IP
            // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("216.58.214.14");


            // lav reverse DNS på ip på website fx google.com (hvis brugeren taster ip ind) og så brug denne som input herunder i URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://google.com/");
            WebResponse response = request.GetResponse();
            Console.Out.WriteLine(response.Headers.Get("Server"));


            Stopwatch stopwatch = new Stopwatch();

            int port = startPort;

            for (int i = startPort; i <= endPort; i++)
            {

                if (protocol == ProtocolType.Tcp)
                {
                    TcpClient tcpClient = new TcpClient();

                    stopwatch.Start();


                    if (tcpClient.ConnectAsync(ipAddressString, port).Wait(1000) == true)
                    {
                        stopwatch.Stop();                       

                        if (isEndpointHostname == true)
                        {
                            Console.WriteLine(ipAddressString + " ( " + ipAddress + " ) " + " - " + dnsEntryCacheStatus + " " + port + " is open");
                            Report.WriteConnectionLogEntry(isEndpointHostname: true, isEndpointIPAddress: false, ipAddressString: ipAddressString, ipAddress: ipAddress, protocol: "tcp", port: port);
                        }
                        else if (isEndpointIPAddress == true)
                        {
                            Console.WriteLine(ipAddressString + " " + " - " + dnsEntryCacheStatus + " "  + port + " is open");
                            Report.WriteConnectionLogEntry(isEndpointHostname: false, isEndpointIPAddress: true, ipAddressString: ipAddressString, ipAddress: ipAddress, protocol: "tcp", port: port);

                        }

                        Console.WriteLine("Time to connect: " + stopwatch.ElapsedMilliseconds + " msec");

                        stopwatch.Reset();



                    }
                    else
                    {
                        stopwatch.Stop();
                        Console.WriteLine(ipAddressString + " ( " + ipAddressString + " ) " + " " + port + " is closed");
                        Console.WriteLine("Timeout was " + stopwatch.ElapsedMilliseconds + " msec");

                        stopwatch.Reset();


                    }
                }

                if (protocol == ProtocolType.Udp)
                {

                    UdpClient udpClient = new UdpClient();

                    try
                    {
                        stopwatch.Start();
                        udpClient.Connect(ipAddressString, port);
                        stopwatch.Stop();

                        Console.WriteLine(ipAddressString + " " + port + " is open");

                        Console.WriteLine("Time to connect: " + stopwatch.ElapsedMilliseconds + " msec");

                        stopwatch.Reset();

                    }
                    catch (Exception)
                    {
                        stopwatch.Stop();
                        Console.WriteLine(ipAddressString + " " + port + " is closed");
                        Console.WriteLine("Timeout was " + stopwatch.ElapsedMilliseconds + " msec");

                        stopwatch.Reset();

                    }
                }

                port++;

            }
        }
    }
}
