using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace accessController
{
    internal class Packet
    {
        //Packet Length
        public static int WGPacketSize = 64; 
        //Type
        public static int Type = 0x17;   //Type

        //Access Controller' (DEVICE) Port
        public static int ControllerPort = 60000;
        //Special logo to prevent issues
        public static long SpecialFlag = 0x55AAAA55;     
        //Function ID
        public int functionID;                           
        //Deceive Serial Number(Controller) four bytes, nine dec number
        public long iDevSn;                              
        //Access Controller' IP Address
        public string IP;                                

        //56 bytes of data [including sequenceId]
        public byte[] data = new byte[56];               
        //Receive Data buffer
        public byte[] recv = new byte[WGPacketSize];     

        public Packet()
        {
            Reset();
        }

        //Data reset
        public void Reset() 
        {
            for (int i = 0; i < 56; i++)
            {
                data[i] = 0;
            }
        }

        private static long sequenceId;

        //Generates a 64-byte short package
        public byte[] ToByte() 
        {
            byte[] buff = new byte[WGPacketSize];
            sequenceId++;

            buff[0] = (byte)Type;
            buff[1] = (byte)functionID;
            Array.Copy(System.BitConverter.GetBytes(iDevSn), 0, buff, 4, 4);
            Array.Copy(data, 0, buff, 8, data.Length);
            Array.Copy(System.BitConverter.GetBytes(sequenceId), 0, buff, 40, 4);
            return buff;
        }

        private readonly WG3000_COMM.Core.wgMjController controller = new WG3000_COMM.Core.wgMjController();

        //send command ,receive return command
        public int Run()  
        {
            byte[] buff = ToByte();

            int tries = 3;
            int errcnt = 0;
            controller.IP = IP;
            controller.PORT = ControllerPort;
            do
            {
                if (controller.ShortPacketSend(buff, ref recv) < 0)
                {
                    return -1;
                }
                else
                {
                    //sequenceId
                    long sequenceIdReceived = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        long lng = recv[40 + i];
                        sequenceIdReceived += (lng << (8 * i));
                    }

                    if ((recv[0] == Type)                       
                        && (recv[1] == functionID)              
                        && (sequenceIdReceived == sequenceId))  
                    {
                        return 1;
                    }
                    else
                    {
                        errcnt++;
                    }
                }
            } while (tries-- > 0); 

            return -1;
        }
        public static long SequenceIdSent()//
        {
            return sequenceId; // The last issue of the serial number(xid)
        }


        public void Close()
        {
            controller.Dispose();
        }
    }
}
