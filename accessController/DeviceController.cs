using System;
using System.Collections.Generic;

namespace accessController
{
    public class DeviceController : Helpers
    {
        public string controllerIP { get; set; }
        public long controllerSN { get; set; }

        private AccessPacket pkt;

        //       public void closeConnection()
        //     {
        //       pkt.Close();
        //    }

        public string Initialize()
        {
            pkt = new AccessPacket
            {
                iDevSn = controllerSN,
                IP = controllerIP
            };
            pkt.functionID = 0x20;
            int ret;
            ret = pkt.Run();
            if (ret > 0)
            {
                return ("Device Online (Connected)");
            }
            return ("Controller Offline (Not connected)");
        }

        public void AddOrModifyPremission(long cardNumber, bool[] doors) //add date endDate
        {
            pkt.Reset();
            pkt.functionID = 0x50;
            //0D D7 37 00 To add or modify the permissions in the card number = 0x0037D70D = 3659533 (decimal)
            LongToBytes(ref pkt.data, 0, cardNumber);
            //20 10 01 01 Start Date:  2021-01-01   (Must be greater than 2001)
            pkt.data[4] = 0x20;
            pkt.data[5] = 0x21;
            pkt.data[6] = 0x01;
            pkt.data[7] = 0x01;
            //20 29 12 31 End Date:  2029-12-31
            pkt.data[8] = 0x20;
            pkt.data[9] = 0x21;
            pkt.data[10] = 0x05;
            pkt.data[11] = 0x02;
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
        }

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

        public long GetLatestSwipe()
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

        public string setDelay(int delay, int doorNumber)
        {
            pkt.Reset();
            pkt.functionID = 0x80;
            //(Set Door 2  Online and Open door delay 3 seconds)
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

        public List<string> extraxtRecord()
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

                        //.......Do something with Records
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
                    LongToBytes(ref pkt.data, 4, AccessPacket.SpecialFlag);

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
    }

    public class Helpers
    {
        protected long byteToLong(byte[] buff, int start, int len)
        {
            long val = 0;
            for (int i = 0; i < len && i < 4; i++)
            {
                long lng = buff[i + start];
                val += (lng << (8 * i));
            }
            return val;
        }

        public void log(string x)
        {
            Console.WriteLine(x);
        }

        public void displayRecordInformation(byte[] recv)
        {
            //8-11	Record the index number
            //(=0 no record)	4	0x00000000
            int recordIndex = 0;

            //12	Record Type**********************************************
            //0=no record
            //1=swipe card record
            //2=Door, button, device start, remote door open record
            //3=alarm record	1
            //0xFF=Indicates that the record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record
            int recordType = recv[12];

            //13	Vality(0 Not allowed to pass, 1 Allowed to pass)	1
            int recordValid = recv[13];

            //14	Door Number(1,2,3,4)	1
            int recordDoorNO = recv[14];

            //15	Door In/Door Out(1 In Door, 2 Out Door)	1	0x01
            int recordInOrOut = recv[15];

            //16-19	Card Number(Type is when the swipe card is recorded)
            //Or number (other types of records)	4
            long recordCardNO = 0;
            recordCardNO = byteToLong(recv, 16, 4);

            //20-26	Swipe card time:
            //The date and time seconds (using the BCD code) See the description of the setup time section
            string recordTime = "2000-01-01 00:00:00";
            recordTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                recv[20], recv[21], recv[22], recv[23], recv[24], recv[25], recv[26]);
            //2012.12.11 10:49:59	7
            //27	Record the ResonNO(You can check the "swipe card record description. Xls" file ReasonNO)
            //Dealing with complex information	1
            int reason = recv[27];

            //0=no record
            //1=swipe card record
            //2=Door, button, device start, remote door open record
            //3=alarm record	1
            //0xFF=Indicates that the record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record
            if (recordType == 0)
            {
                log(string.Format("Index Bit={0}  no record", recordIndex));
            }
            else if (recordType == 0xff)
            {
                log(" The record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record");
            }
            else if (recordType == 1) //2015-06-10 08:49:31 Displays data whose record type is card number
            {
                //Card Number
                log(string.Format("Index Bit={0}  ", recordIndex));
                log(string.Format("  Card Number = {0}", recordCardNO));
                log(string.Format("  Door Number = {0}", recordDoorNO));
                log(string.Format("  IN/OUT = {0}", recordInOrOut == 1 ? "Door In" : "Door Out"));
                log(string.Format("  Valid = {0}", recordValid == 1 ? "Pass" : "No Pass"));
                log(string.Format("  Time = {0}", recordTime));
                log(string.Format("  Description = {0}", getReasonDetailEnglish(reason)));
            }
            else if (recordType == 2)
            {
                //Other processing
                //Door, button, device start, remote door open record
                log(string.Format("Index Bit={0}  No swipe card record", recordIndex));
                log(string.Format("  Serial number = {0}", recordCardNO));
                log(string.Format("  Door Number = {0}", recordDoorNO));
                log(string.Format("  Time = {0}", recordTime));
                log(string.Format("  Description = {0}", getReasonDetailEnglish(reason)));
            }
            else if (recordType == 3)
            {
                //Other processing
                //alarm record
                log(string.Format("Index Bit={0}  alarm record", recordIndex));
                log(string.Format("  Serial number = {0}", recordCardNO));
                log(string.Format("  Door Number = {0}", recordDoorNO));
                log(string.Format("  Time = {0}", recordTime));
                log(string.Format("  Description = {0}", getReasonDetailEnglish(reason)));
            }
        }

        public string returnRecordInfo(byte[] recv)
        {
            //8-11	Record the index number
            //(=0 no record)	4	0x00000000
            int recordIndex = 0;

            //12	Record Type**********************************************
            //0=no record
            //1=swipe card record
            //2=Door, button, device start, remote door open record
            //3=alarm record	1
            //0xFF=Indicates that the record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record
            int recordType = recv[12];

            //13	Vality(0 Not allowed to pass, 1 Allowed to pass)	1
            int recordValid = recv[13];

            //14	Door Number(1,2,3,4)	1
            int recordDoorNO = recv[14];

            //15	Door In/Door Out(1 In Door, 2 Out Door)	1	0x01
            int recordInOrOut = recv[15];

            //16-19	Card Number(Type is when the swipe card is recorded)
            //Or number (other types of records)	4
            long recordCardNO = 0;
            recordCardNO = byteToLong(recv, 16, 4);

            //20-26	Swipe card time:
            //The date and time seconds (using the BCD code) See the description of the setup time section
            string recordTime = "2000-01-01 00:00:00";
            recordTime = string.Format("{0:X2}{1:X2}-{2:X2}-{3:X2} {4:X2}:{5:X2}:{6:X2}",
                recv[20], recv[21], recv[22], recv[23], recv[24], recv[25], recv[26]);
            //2012.12.11 10:49:59	7
            //27	Record the ResonNO(You can check the "swipe card record description. Xls" file ReasonNO)
            //Dealing with complex information	1
            int reason = recv[27];
            List<string> records = new List<string> { };
            //0=no record
            //1=swipe card record
            //2=Door, button, device start, remote door open record
            //3=alarm record	1
            //0xFF=Indicates that the record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record
            if (recordType == 0)
            {
                return string.Format("Index Bit={0}  no record", recordIndex);
            }
            else if (recordType == 0xff)
            {
                return (" The record of the specified index bit has been overwritten. Please use index 0 to retrieve the index value of the earliest record");
            }
            else if (recordType == 1) //2015-06-10 08:49:31 Displays data whose record type is card number
            {
                //Card Number
                return (string.Format("Index Bit={0},Card Number = {1}, Door Number = {2},IN/OUT = {3}, Valid = {4}, Time = {5}, Description = {6}  ", recordIndex, recordCardNO, recordDoorNO, recordInOrOut, recordValid, recordTime, getReasonDetailEnglish(reason)));
            }
            else if (recordType == 2)
            {
                //Other processing
                //Door, button, device start, remote door open record
                return (string.Format("Index Bit={0}  No swipe card record,  Serial number = {1},  Door Number = {2} Time = {3}  Description = {4}", recordIndex, recordCardNO, recordDoorNO, recordTime, getReasonDetailEnglish(reason)));
            }
            else if (recordType == 3)
            {
                //Other processing
                //alarm record
                return (string.Format("Index Bit={0}  alarm record,Card number = {0}, Door Number = {0}, Time = {0},Description = {0}", recordIndex, recordCardNO, recordDoorNO, recordTime, getReasonDetailEnglish(reason)));
            }
            else
            {
                return "";
            }
        }

        public readonly string[] RecordDetails =
{
"1","SwipePass","Swipe","刷卡开门",
"2","SwipePass","Swipe Close","刷卡关",
"3","SwipePass","Swipe Open","刷卡开",
"4","SwipePass","Swipe Limited Times","刷卡开门(带限次)",
"5","SwipeNOPass","Denied Access: PC Control","刷卡禁止通过: 电脑控制",
"6","SwipeNOPass","Denied Access: No PRIVILEGE","刷卡禁止通过: 没有权限",
"7","SwipeNOPass","Denied Access: Wrong PASSWORD","刷卡禁止通过: 密码不对",
"8","SwipeNOPass","Denied Access: AntiBack","刷卡禁止通过: 反潜回",
"9","SwipeNOPass","Denied Access: More Cards","刷卡禁止通过: 多卡",
"10","SwipeNOPass","Denied Access: First Card Open","刷卡禁止通过: 首卡",
"11","SwipeNOPass","Denied Access: Door Set NC","刷卡禁止通过: 门为常闭",
"12","SwipeNOPass","Denied Access: InterLock","刷卡禁止通过: 互锁",
"13","SwipeNOPass","Denied Access: Limited Times","刷卡禁止通过: 受刷卡次数限制",
"14","SwipeNOPass","Denied Access: Limited Person Indoor","刷卡禁止通过: 门内人数限制",
"15","SwipeNOPass","Denied Access: Invalid Timezone","刷卡禁止通过: 卡过期或不在有效时段",
"16","SwipeNOPass","Denied Access: In Order","刷卡禁止通过: 按顺序进出限制",
"17","SwipeNOPass","Denied Access: SWIPE GAP LIMIT","刷卡禁止通过: 刷卡间隔约束",
"18","SwipeNOPass","Denied Access","刷卡禁止通过: 原因不明",
"19","SwipeNOPass","Denied Access: Limited Times","刷卡禁止通过: 刷卡次数限制",
"20","ValidEvent","Push Button","按钮开门",
"21","ValidEvent","Push Button Open","按钮开",
"22","ValidEvent","Push Button Close","按钮关",
"23","ValidEvent","Door Open","门打开[门磁信号]",
"24","ValidEvent","Door Closed","门关闭[门磁信号]",
"25","ValidEvent","Super Password Open Door","超级密码开门",
"26","ValidEvent","Super Password Open","超级密码开",
"27","ValidEvent","Super Password Close","超级密码关",
"28","Warn","Controller Power On","控制器上电",
"29","Warn","Controller Reset","控制器复位",
"30","Warn","Push Button Invalid: Disable","按钮不开门: 按钮禁用",
"31","Warn","Push Button Invalid: Forced Lock","按钮不开门: 强制关门",
"32","Warn","Push Button Invalid: Not On Line","按钮不开门: 门不在线",
"33","Warn","Push Button Invalid: InterLock","按钮不开门: 互锁",
"34","Warn","Threat","胁迫报警",
"35","Warn","Threat Open","胁迫报警开",
"36","Warn","Threat Close","胁迫报警关",
"37","Warn","Open too long","门长时间未关报警[合法开门后]",
"38","Warn","Forced Open","强行闯入报警",
"39","Warn","Fire","火警",
"40","Warn","Forced Close","强制关门",
"41","Warn","Guard Against Theft","防盗报警",
"42","Warn","7*24Hour Zone","烟雾煤气温度报警",
"43","Warn","Emergency Call","紧急呼救报警",
"44","RemoteOpen","Remote Open Door","操作员远程开门",
"45","RemoteOpen","Remote Open Door By USB Reader","发卡器确定发出的远程开门"
        };

        public string getReasonDetailEnglish(int Reason) //English description
        {
            if (Reason > 45)
            {
                return "";
            }
            if (Reason <= 0)
            {
                return "";
            }
            return RecordDetails[(Reason - 1) * 4 + 2]; //English information
        }

        public void LongToBytes(ref byte[] outBytes, int startIndex, long val)
        {
            Array.Copy(System.BitConverter.GetBytes(val), 0, outBytes, startIndex, 4);
        }

        public int GetHex(int val)
        {
            return ((val % 10) + (((val - (val % 10)) / 10) % 10) * 16);
        }
    }
}