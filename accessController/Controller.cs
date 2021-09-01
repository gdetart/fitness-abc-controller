using System;
using System.Collections.Generic;

namespace accessController 
{
      public class Controller : Helpers
    {
        public string controllerIP;
        public long controllerSN ;
        public static System.Text.Encoding stringEncoder { get; }
        private Packet pkt;
        public Controller (string IP,long SN)
        {
            controllerIP=IP;
            controllerSN=SN;
            pkt = new Packet
            {
                iDevSn = controllerSN,
                IP = controllerIP
            };
        }
        

        public void closeConnection()
        {
            pkt.Close();
        }
        
        //Read Date and time of the controller device
        public string readDateTime()
        {
            pkt.Reset();
            pkt.functionID = 0x32;
            int ret = pkt.Run();
            if (ret > 0)
            {

                string controllerTime = "2000-01-01 00:00:00";
                controllerTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                    pkt.recv[8], pkt.recv[9], pkt.recv[10], pkt.recv[11], pkt.recv[12], pkt.recv[13], pkt.recv[14]);
                return controllerTime;
            }
            return "Failed to read Date and Time";


        }

        //Synchronise date and time between pc and controller Device
        public int SyncDateTime()
        {
            pkt.Reset();
            pkt.functionID = 0x30;

            DateTime ptm = DateTime.Now;
            pkt.data[0] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100);
            pkt.data[1] = (byte)GetHex((int)((ptm.Year) % 100)); //st.GetMonth()); 
            pkt.data[2] = (byte)GetHex(ptm.Month);
            pkt.data[3] = (byte)GetHex(ptm.Day);
            pkt.data[4] = (byte)GetHex(ptm.Hour);
            pkt.data[5] = (byte)GetHex(ptm.Minute);
            pkt.data[6] = (byte)GetHex(ptm.Second);
            int ret = pkt.Run();
            if (ret > 0)
            {
                Boolean bSame = true;
                for (int i = 0; i < 7; i++)
                {
                    if (pkt.data[i] != pkt.recv[8 + i])
                    {
                        bSame = false;
                        break;
                    }
                }
                if (bSame)
                {
                    log("1.6 Set the date and time to success...");

                }
            }
            return ret;
        }
        //Get a list of all the controllers connected in the network
        public List<string> SearchControllers()
        {
            List<string> controllers = new List<string>();
            for (int i = 0; i < 40; i++)
            {
                pkt.Reset();
                pkt.functionID = 0x94;
                pkt.Run();
                if (pkt.recv != null)
                {
                    controllers.Add(Convert.ToString(pkt.recv[8]) + "." + Convert.ToString(pkt.recv[9]) + "." + Convert.ToString(pkt.recv[10]) + "." + Convert.ToString(pkt.recv[11]));

                }

            }
            return controllers;

        }
        
        //Delete Premisions to open gates with cardNumber
        public int deleteSinglePremission(long cardNumber)
        {
            pkt.Reset();
            pkt.functionID = 0x52;
            pkt.iDevSn = controllerSN;
            LongToBytes(ref pkt.data, 0, cardNumber);

            int ret = pkt.Run();
            return ret;
        }
        //Delete all Premissions
        public void DeleteAllPremissions()
        {
            pkt.Reset();
            pkt.functionID = 0x54;
            pkt.iDevSn = controllerSN;
            LongToBytes(ref pkt.data, 0, 0x55AAAA55);

            int ret = pkt.Run();
            if (ret > 0)
            {
                if (pkt.recv[8] == 1)
                {
                    //At this time clear the success
                    log("1.13 Permission to clear (all cleared)...");
                }
            }
        }
        //Gets the premission number registred in the controller device
        public long GetTotalPremisions()
        {
            pkt.Reset();
            pkt.functionID = 0x58;
            pkt.iDevSn = controllerSN;
            int ret = pkt.Run();
            if (ret > 0)
            {
                log("successfully extracted number of premissions");
                return byteToLong(pkt.recv, 8, 4);
            }
            else
            {
                log("FAILED to get premissions");

                return 0;
            }

        }
        //Modify name by cardNumber
        public void ModifyName(long cardNr, string name)
        {
            pkt.Reset();
            pkt.functionID = 0x60;
            pkt.iDevSn = controllerSN;
            LongToBytes(ref pkt.data, 0, cardNr);
            stringToBytes(ref pkt.data, 4, name);
            pkt.Run();



        }
        
        //Read name by cardNumber
        public string ReadNameByCard(long cardNo)
        {
            pkt.Reset();
            pkt.functionID = 0x64;
            pkt.iDevSn = controllerSN;
            LongToBytes(ref pkt.data, 0, cardNo);
            pkt.Run();
            string name = bytesToString(pkt.recv, 12,28);
            return name;
        }

        //Read name by index Number
        public string ReadNameByIndex(long index)
        {
            pkt.Reset();
            pkt.functionID = 0x66;
            pkt.iDevSn = controllerSN;
            LongToBytes(ref pkt.data, 0, index);
            pkt.Run();
            string name = bytesToString(pkt.recv, 12, 28);
            return name;
        }

        //GET PREMISSION BY INDEX NUMBER
        public byte[] GetPremissionByIndex(long index)
        {
            pkt.Reset();
            pkt.functionID = 0x5C;
            pkt.iDevSn = controllerSN;
            LongToBytes(ref pkt.data, 0, index);

            int ret = pkt.Run();
            if (ret > 0)
            {

                long cardNOOfPrivilegeToGet = 0;
                cardNOOfPrivilegeToGet = byteToLong(pkt.recv, 8, 4);
                if (4294967295 == cardNOOfPrivilegeToGet) //FFFFFFFF对应于4294967295
                {
                   
                }
                else if (cardNOOfPrivilegeToGet == 0)
                {
                    //No permission information: (card number part 0)
                    
                }
                else
                {
                    //Specific authority information...
                }
                log("1.16 Gets the permission to specify the index number	 Success...");

            }
            return pkt.recv;
        }
        //Get premission by card number
        public void GetPremissionByCard(long cardNumber)
        {
            pkt.Reset();
            pkt.functionID = 0x5A;
            pkt.iDevSn = controllerSN;
            // (Check card number 0D D7 37 00 permissions)
            long cardNOOfPrivilegeToQuery = 8315505;
            LongToBytes(ref pkt.data, 0, cardNOOfPrivilegeToQuery);

            int ret = pkt.Run();

            if (ret > 0)
            {

                long cardNOOfPrivilegeToGet = 0;
                cardNOOfPrivilegeToGet = byteToLong(pkt.recv, 8, 4);
                if (cardNOOfPrivilegeToGet == 0)
                {
                    //No permission information: (card number part 0)
                    log("1.15      No permission information: (card number part 0)");
                }
                else
                {
                    //Specific authority information...
                    log("1.15     Have permission information...");
                }
                log(Convert.ToString(byteToLong(pkt.recv, 12, 4)));
            }
        }
        //Give Premission to a specific card number
        public int GivePremission(long cardNumber, bool[] doors, DateTime date) //add date endDate
        {
            pkt.Reset();
            pkt.functionID = 0x50;
            pkt.iDevSn = controllerSN;
            //0D D7 37 00 To add or modify the permissions in the card number = 0x0037D70D = 3659533 (decimal)
            LongToBytes(ref pkt.data, 0, cardNumber);
            DateTime Today = DateTime.Now;

            //20 10 01 01 Start Date:  2021-01-01   (Must be greater than 2001)
            pkt.data[4] = (byte)GetHex((Today.Year - Today.Year % 100) / 100);
            pkt.data[5] = (byte)GetHex((int)((Today.Year) % 100));
            pkt.data[6] = (byte)GetHex(Today.Month);
            pkt.data[7] = (byte)GetHex(Today.Day);
            //20 29 12 31 End Date:  2029-12-31
            pkt.data[8] = (byte)GetHex((date.Year - date.Year % 100) / 100);
            pkt.data[9] = (byte)GetHex((int)((date.Year) % 100));
            pkt.data[10] = (byte)GetHex(date.Month);
            pkt.data[11] = (byte)GetHex(date.Day);
            //01 Allows entry via door 1 [for single door, two door, four door controllers]
            pkt.data[12] = Convert.ToByte(doors[0]);
            //01 Allowed through the door 2 [effective for two-door, four-door controller]
            pkt.data[13] = Convert.ToByte(doors[1]);  //If Door 2 is disabled, it is set to 0x00
            //01 Permitted via door 3 [valid for four-door controller]
            pkt.data[14] = Convert.ToByte(doors[2]);
            //01 Allowed via door 4 [effective for four-door controller]
            pkt.data[15] = Convert.ToByte(doors[3]);

            int ret = pkt.Run();
            if (ret > 0)
            {
                if (pkt.recv[8] == 1)
                {
                    //Then the card number = 0x0037D70D = 3659533 (decimal) card, No. 1 door relay action.
                    log("1.11 Permissions are added or modified successfully...");
                }
            }
            else { log("false"); }
            return ret;
        }
        
        //Remotely open the door by door number
        public void OpenDoor(int doorNumber)
        {
            pkt.Reset();

            pkt.functionID = 0x40;

            pkt.data[0] = (byte)(doorNumber & 0xff); //2013-11-03 20:56:33
            int ret = pkt.Run();
            if (ret > 0)
            {
                if (pkt.recv[8] == 1)
                    log("Successfully opened door: " + doorNumber.ToString());
            }
            else
            {
                Console.WriteLine("SPO BAAAN O DAJ");
            }
        }

        //Set the door delay in seconds
        public string SetDoorDelay(int delay, int doorNumber)
        {
            pkt.Reset();
            pkt.functionID = 0x80;
            pkt.data[0] = Convert.ToByte(doorNumber); //Door Number
            pkt.data[1] = Convert.ToByte(3); //Door online
            pkt.data[2] = Convert.ToByte(delay); //Open the door delay

            int ret = pkt.Run();
            if (ret > 0)
            {
                if (pkt.data[0] == pkt.recv[8])

                {
                    if (pkt.data[2] == pkt.recv[10])
                    {
                        return ("Set the door control parameters	 Success...");
                    }
                    else
                    {
                        return ("error at pktdata[2]");
                    }
                }
                else
                {
                    return ("error at pkt.data[0] not good ");
                }
            }
            else
            {
                return ("Error on setDelay");
            }
        }

        //Get all the record swipes from the controller
        public List<string> GetAllRecordSwipes()
        {
            //1.9	Extract record operation
            //1. The read record index number is obtained by the 0xB4 instruction recordIndex
            //2. Get the record of the specified index number with the 0xB0 instruction Start the record from recordIndex + 1 until it is empty
            //3. Set the value of the read record index number set by the 0xB2 instruction to the last card number to be read.
            //After the above three steps, the entire extraction of records to complete the oper
            List<string> allRecords = new List<string> { };

            pkt.Reset();
            pkt.functionID = 0xB4;
            int ret = pkt.Run();
            int success = 0;
            if (ret > 0)
            {
                long recordIndexGotToRead = 0x0;
                recordIndexGotToRead = (long)byteToLong(pkt.recv, 8, 4);
                pkt.Reset();
                pkt.functionID = 0xB0;
                long recordIndexToGetStart = recordIndexGotToRead + 1;
                long recordIndexValidGet = 0;
                int cnt = 0;

                do
                {
                    LongToBytes(ref pkt.data, 0, recordIndexToGetStart);
                    ret = pkt.Run();
                    success = 0;
                    if (ret > 0)
                    {
                        success = 1;

                        //12	Record type
                        //0=no record
                        //1=Credit card record
                        //2=Door, button, device start, remote door open record
                        //3=alarm record	1
                        //0xFF=Indicates that the record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record
                        int recordType = pkt.recv[12];
                        if (recordType == 0)
                        {
                            break; //No more records
                        }
                        if (recordType == 0xff)//This index number is invalid to reset the index value
                        {
                            //Take the index bit of the earliest record
                            pkt.Reset();
                            pkt.functionID = 0xB0;
                            int recordIndexToGet = 0;
                            LongToBytes(ref pkt.data, 0, recordIndexToGet);

                            ret = pkt.Run();
                            success = 0;
                            if (ret > 0)
                            {
                                recordIndexGotToRead = (int)byteToLong(pkt.recv, 8, 4);
                                recordIndexToGetStart = recordIndexGotToRead;
                                continue;
                            }
                            success = 0;
                            break;
                        }
                        recordIndexValidGet = recordIndexToGetStart;

                        allRecords.Add(returnRecordInfo(pkt.recv)); //2015-06-09 20:01:21
                    }
                    else
                    {
                        //Extraction failed

                        break;
                    }
                    recordIndexToGetStart++;
                } while (cnt++ < 200000);
                if (success > 0)
                {
                    //Set the value of the read record index number set by the 0xB2 instruction to the last card number to be read.
                    pkt.Reset();
                    pkt.functionID = 0xB2;
                    LongToBytes(ref pkt.data, 0, recordIndexValidGet);

                    //12	Logo (to prevent false settings) 1 0x55 [fixed]
                    LongToBytes(ref pkt.data, 4, Packet.SpecialFlag);

                    ret = pkt.Run();
                    success = 0;
                    if (ret > 0)
                    {
                        if (pkt.recv[8] == 1)
                        {
                            //Completely extract successfully
                            success = 1;
                        }
                    }
                }
            }
            return allRecords;
        }

        //Get only the latest swipe 
        public long GetLastSwipe()
        {
            //Send a message (take the latest one through the index 0xffffffff)
            pkt.Reset();
            pkt.functionID = 0xB0;
            long recordIndexToGet = 0xffffffff;
            LongToBytes(ref pkt.data, 0, recordIndexToGet);
            int ret = pkt.Run();
            if (ret > 0)
            {
                log("1.7 The information for the latest record was successful...");
                //	  	The latest record of the information
                long cardNum = byteToLong(pkt.recv, 16, 4);
                return cardNum;//2015-06-09 20:01:21
            }
            else
            {
                log("error");
                return 0x0;
            }
        }
    }
}