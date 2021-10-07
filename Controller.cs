using System;
using System.Collections.Generic;

namespace accessController
{
    public class CommunicationResponse
    {
        public bool Successfull;
        public string Info;
        public string Error;
        public dynamic value = null;
    }

    public class DeviceController : Helpers
    {


        public string ControllerIP { get; set; }
        public long ControllerSN { get; set; }
        public static System.Text.Encoding StringEncoder { get; }

        private readonly Packet pkt;



        public DeviceController(string IpAddress, long SerialNumber)
        {
            ControllerIP = IpAddress;
            ControllerSN = SerialNumber;
            pkt = new Packet(ControllerIP, ControllerSN);
           
            try
            {
                pkt.functionID = 0x20;
                int ret = pkt.Run();
                if (ret > 0)
                {
                    
                    //Add smth
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        public CommunicationResponse SearchControllers()
        {
            List<string> controllers = new();
            CommunicationResponse response = new();

            try
            {
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
                response.Info = $"Found {controllers.Count} Controllers";
                response.Successfull = true;
                response.value = controllers;
                return response;
            }
            catch (Exception Err)
            {
                response.Error = Err.Message;
                response.Info = Err.Message;
                response.Successfull = false;
                return response;
            }
        }


        public CommunicationResponse ReadDateTime()
        {
            CommunicationResponse response = new();
            try
            {


                pkt.Reset();
                pkt.functionID = 0x32;
                int ret = pkt.Run();
                if (ret > 0)
                {
                    string controllerTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                    pkt.recv[8], pkt.recv[9], pkt.recv[10], pkt.recv[11], pkt.recv[12], pkt.recv[13], pkt.recv[14]);
                    response.Successfull = true;
                    response.Info = $"Successfully read the controller time {controllerTime}";
                    response.value = controllerTime;
                    return response;
                }
                response.Successfull = false;
                response.Info = "Could not read Date and time";
                response.Error = $"Check your device communication by cmd : ping {ControllerIP}";
                return response;

            }
            catch (Exception Err)
            {
                response.Error = Err.Message;
                response.Info = Err.Message;
                response.Successfull = false;
                return response;

            }


        }

        public CommunicationResponse SyncDateTime()
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x30;
                DateTime ptm = DateTime.Now;
                pkt.data[0] = (byte)GetHex((ptm.Year - ptm.Year % 100) / 100);
                pkt.data[1] = (byte)GetHex((ptm.Year) % 100); //st.GetMonth()); 
                pkt.data[2] = (byte)GetHex(ptm.Month);
                pkt.data[3] = (byte)GetHex(ptm.Day);
                pkt.data[4] = (byte)GetHex(ptm.Hour);
                pkt.data[5] = (byte)GetHex(ptm.Minute);
                pkt.data[6] = (byte)GetHex(ptm.Second);
                int ret = pkt.Run();
                if (ret > 0)
                {
                    bool bSame = true;
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
                        response.Info = $"Successfully set the date and time to {ptm}";
                        response.Successfull = true;
                        response.value = ptm;
                        response.Error = null;
                        return response;

                    }
                }
                response.Info = "Failed to set date and time";
                response.Error = "Please check the communication with your device";
                response.Successfull = true;
                return response;

            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Successfull = false;
                response.Error = Err.Source;
                return response;

            }

        }


        public CommunicationResponse GetTotalPremissions()
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x58;
                pkt.iDevSn = ControllerSN;
                int ret = pkt.Run();
                if (ret > 0)
                {
                    var PremissionNumber = ByteToLong(pkt.recv, 8, 4);
                    response.Info = $"Number of total Premissions : {PremissionNumber}";
                    response.Successfull = true;
                    response.value = PremissionNumber;
                    return response;
                }
                else
                {

                    response.Info = "FAILED to get number of premissions";
                    response.Successfull = false;
                    response.Error = "Please check your communication with device";
                    return response;
                }
            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = false;
                return response;
            }

        }

        public CommunicationResponse DeleteSinglePremission(long cardNumber)
        {
            CommunicationResponse response = new();

            try
            {
                pkt.Reset();
                pkt.functionID = 0x52;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, cardNumber);

                int ret = pkt.Run();
                if (ret > 0)
                {
                    response.Info = "Successfully deleted premission";
                    response.Successfull = true;
                    return response;
                }
                response.Info = "Failed to delete premission";
                response.Error = "Please check your communication with device";
                response.Successfull = false;
                return response;
            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;

            }

        }

        public CommunicationResponse DeleteAllPremissions()
        {
            CommunicationResponse response = new();

            try
            {
                pkt.Reset();
                pkt.functionID = 0x54;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, 0x55AAAA55);

                int ret = pkt.Run();
                if (ret > 0)
                {
                    if (pkt.recv[8] == 1)
                    {

                        response.Info = "Successfully Deleted all premissions";
                        response.Successfull = true;

                    };
                }
                response.Info = "Failed to delete all premissions";
                response.Successfull = false;
                response.Error = "Please check your communication with device";
                return response;


            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;
            }

        }

        public CommunicationResponse ModifyName(long cardNr, string name)
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x60;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, cardNr);
                StringToBytes(ref pkt.data, 4, name);
                int ret = pkt.Run();
                if (ret > 0)
                {
                    response.Successfull = true;
                    response.Info = $"Name is successfully set to {name}";
                    response.value = name;
                    return response;
                }
                response.Info = $"Failed to set name as {name}";
                response.Successfull = false;
                response.Error = "Please check your communication with device";
                return response;
            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;
            }


        }

        public CommunicationResponse ReadNameByCard(long cardNo)
        {
            CommunicationResponse response = new();
            try
            {

                pkt.Reset();
                pkt.functionID = 0x64;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, cardNo);
                if (pkt.Run() > 0)
                {
                    ;

                    string name = BytesToString(pkt.recv, 12, 28);
                    response.Info = $"Name of card number {cardNo} is registred as - {name}";
                    response.Successfull = true;
                    response.value = name;
                    return response;
                }
                response.Info = $"Failed to get the name of card number {cardNo}";
                response.Successfull = false;
                response.Error = "Please check your communication with device";
                return response;
            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;
            }
        }
        public CommunicationResponse ReadNameByIndex(long index)
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x66;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, index);
                int ret = pkt.Run();
                if (ret > 0)
                {
                    string name = BytesToString(pkt.recv, 12, 28);

                    response.Info = $"Name of index number {index} is registred as - {name}";
                    response.Successfull = true;
                    response.value = name;
                    return response;
                }
                response.Info = $"Failed to get the name of card number {index}";
                response.Successfull = false;
                response.Error = "Please check your communication with device";
                return response;
            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;
            }


        }

        public CommunicationResponse GetPremissionByIndex(long index)
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x5C;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, index);

                int ret = pkt.Run();
                if (ret > 0)
                {
                    long cardNOOfPrivilegeToGet = ByteToLong(pkt.recv, 8, 4);
                    if (4294967295 == cardNOOfPrivilegeToGet)
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
                return response;
            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;
            }

        }

        public void GetPremissionByCard(long cardNumber)
        {
            pkt.Reset();
            pkt.functionID = 0x5A;
            pkt.iDevSn = ControllerSN;
            LongToBytes(ref pkt.data, 0, cardNumber);

            int ret = pkt.Run();

            if (ret > 0)
            {

                long cardNOOfPrivilegeToGet = 0;
                cardNOOfPrivilegeToGet = ByteToLong(pkt.recv, 8, 4);
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
            }
        }
        public CommunicationResponse GivePremission(long cardNumber, bool[] doors, DateTime date) //add endDate
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x50;
                pkt.iDevSn = ControllerSN;
                LongToBytes(ref pkt.data, 0, cardNumber);
                DateTime Today = DateTime.Now;
                pkt.data[4] = (byte)GetHex((Today.Year - Today.Year % 100) / 100);
                pkt.data[5] = (byte)GetHex((Today.Year) % 100);
                pkt.data[6] = (byte)GetHex(Today.Month);
                pkt.data[7] = (byte)GetHex(Today.Day);
                //SET END DATE BASED ON DATETIME;
                pkt.data[8] = (byte)GetHex((date.Year - date.Year % 100) / 100);
                pkt.data[9] = (byte)GetHex((date.Year) % 100);
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
                        response.Info = $"Successfully set the premission until {date}";
                        response.Successfull = true;
                    }
                }
                response.Info = $"Failed to give premission to card number {cardNumber}";
                response.Successfull = false;
                response.Error = "Please check your communication with device";
                return response;



            }
            catch (Exception Err)
            {
                response.Info = Err.Message;
                response.Error = Err.Source;
                response.Successfull = true;
                return response;
            }

        }

        public CommunicationResponse OpenDoor(int doorNumber)
        {
            CommunicationResponse response = new();
            try
            {
                pkt.Reset();
                pkt.functionID = 0x40;
                pkt.data[0] = (byte)(doorNumber & 0xff); //2013-11-03 20:56:33
                int ret = pkt.Run();
                if (ret > 0)
                {
                    if (pkt.recv[8] == 1)
                    {
                        response.Info = $"Successfully opened door  number : {doorNumber}";
                        response.Successfull = true;
                        return response;
                    }
                    response.Info = $"Failed to open door  number : {doorNumber}";
                    response.Successfull = false;
                    response.Error = "Something went wrong please try again";
                    return response;
                }
                else
                {
                    response.Info = $"Failed to open door  number : {doorNumber}";
                    response.Successfull = false;
                    response.Error = "Check device communication";
                    return response;
                }
            }catch(Exception Err)
            {
                response.Info = $"Failed to open door  number : {doorNumber}";
                response.Successfull = false;
                response.Error = Err.Message;
                return response;

            }

        }


        public CommunicationResponse SetDoorDelay(int delay, int doorNumber)
        {
            CommunicationResponse response = new();

            try
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
                            response.Info = $"Sucessfully set delay of {delay} to the door number {doorNumber}";
                            response.Successfull = true;
                            response.value = delay;
                            return response;
                        }
                        else
                        {
                            response.Info= $"Failed to set delay of {delay} to the door number {doorNumber}";
                            response.Error = "Error at Packet data[2]";
                            response.Successfull = false;
                            return response;
                        }
                    }
                    else
                    {
                        response.Info = $"Failed to set delay of {delay} to the door number {doorNumber}";
                        response.Error = "Error at Packet data[0] failed to get door number";
                        response.Successfull = false;
                        return response;
                    }
                }
                else
                {
                    response.Info = $"Failed to set delay of {delay} to the door number {doorNumber}";
                    response.Error = "Error setting the delay";
                    response.Successfull = false;
                    return response;
                }

            }
            catch (Exception Err)
            {
                response.Info = $"Failed to open door  number : {doorNumber}";
                response.Successfull = false;
                response.Error = Err.Message;
                return response;

            }
           
        }

        public List<string> GetAllRecordSwipes()
        {
            List<string> allRecords = new();

            pkt.Reset();
            pkt.functionID = 0xB4;
            int ret = pkt.Run();
            int success = 0;
            if (ret > 0)
            {
                long recordIndexGotToRead = 0x0;
                recordIndexGotToRead = ByteToLong(pkt.recv, 8, 4);
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
                          
                            pkt.Reset();
                            pkt.functionID = 0xB0;
                            int recordIndexToGet = 0;
                            LongToBytes(ref pkt.data, 0, recordIndexToGet);

                            ret = pkt.Run();
                            success = 0;
                            if (ret > 0)
                            {
                                recordIndexGotToRead = (int)ByteToLong(pkt.recv, 8, 4);
                                recordIndexToGetStart = recordIndexGotToRead;
                                continue;
                            }
                            success = 0;
                            break;
                        }
                        recordIndexValidGet = recordIndexToGetStart;

                        allRecords.Add(ReturnRecordInfo(pkt.recv)); 
                    }
                    else
                    {
                       

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

        public CommunicationResponse GetLastSwipedCard()
        {
            CommunicationResponse response = new();

            try
            {
                pkt.Reset();
                pkt.functionID = 0xB0;
                long recordIndexToGet = 0xffffffff;
                LongToBytes(ref pkt.data, 0, recordIndexToGet);
                int ret = pkt.Run();
                if (ret > 0)
                {
                    long cardNum = ByteToLong(pkt.recv, 16, 4);
                    response.Info = $"Successfully got the latest swipe card : {cardNum}";
                    response.Successfull = true;
                    response.value = cardNum;
                    return response;
                }
                response.Info = "Failed to get the latest swipe card";
                response.Successfull = false;
                response.value = null;
                response.Error = "Check device communication";
                return response;

            }
            catch (Exception Err)
            {
                response.Info = "Failed to get the latest swipe card";
                response.Successfull = false;
                response.Error = Err.Message;
                return response;

            }
        }
    }
}
