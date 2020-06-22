using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace Portscanner
{
    class Program
    {
        static void Main(string[] args)
        {

        http://www.michalbialecki.com/2018/05/25/how-to-make-you-console-app-look-cool/



            // fortæl hvilken server svarer på kaldet fx hvis det er en mail server så find ud af om det er en exchange server som svarer ved at sende den kommandoer og tjekke




            // OPTIMER I KODEN DET ER SLAM KODE LIGE NU!
            // kommenter kode
            // lav kode smartere og mere decoupled

            // ekstra features:

            // tjek om host fil er indlæst

            // lav scan uden om dns cache på pc - måske kan denne bruges: https://dnsclient.michaco.net/

            // upd scan er forkert den kan ikke tjekke om en port svarer, jeg skla kunne tjekke om jeg fx kan slå nogt op i dns på 53

            // rapport fil i txt
            // multiple ip addresses to scan at the same time (multi threading) 


            // grafisk tegning over iper og porte flow af trafik
            // program skal sige om et er localhost eller anden ip
            // command line argumenter 192.168.1.0/24 tcp 80-81

            // tjekke om trafic går gennem proxy server & hvilken ip som remote pc efter proxy har & om port bliver translated // prøv at kigge i wireshark trafic for trafik mod proxy
            // tjekke lokale porte som kunne være proxy

            // cmdx program

            // tjekke om det er remote firewall som blocker 
            // traceroute
            // standard liste af services som http
            // confif fil som ligger i samme mappe som program
            // check tcp mss            
            // få lavet så jeg genbruger alt min kode fx så jeg ikke har næsten samme kode til at få start og end range
            // gør gui pæn
            // lav unit test
            // tilføj knap i menu ved hjælp af flueben til fx at vælge at logge resultat til txt fil - man skla kunne vælge felt hvor man skal skrive tekst ind ved at trykke på højre pile tast



            while (true)
            {
                Menu.ShowIPAddressMenu();

                Menu.ShowProtocolMenu();

                Menu.ShowStartPortMenu();

                Menu.ShowEndPortMenu();

                Menu.ShowConnectionTestMenu();              

                Console.ReadLine();
            }


        }

    }

    }

