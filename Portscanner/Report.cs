using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Portscanner
{
    class Report
    {
        

        public static void WriteConnectionLogEntry(bool isEndpointHostname, bool isEndpointIPAddress, string ipAddressString, string ipAddress, string protocol, int port)
        {
            string reportFilePath = Directory.GetCurrentDirectory();
            reportFilePath = reportFilePath + "\\report.txt";

            Console.WriteLine(reportFilePath);


            using (StreamWriter streamWriter = new StreamWriter(reportFilePath))
            {
                if (isEndpointHostname == true)
                {
                    streamWriter.WriteLine(DateTime.UtcNow + " " + ipAddressString + "( " + ipAddress + " )" + " " + protocol + " " + port + " is open");
                }
                else if (isEndpointIPAddress == true)
                {
                    streamWriter.WriteLine(DateTime.UtcNow + " " + ipAddressString + " " + protocol + " " + port + " is open");

                }

            }


        }
    }
}
