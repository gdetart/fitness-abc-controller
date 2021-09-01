namespace accessController
{

    public class Helpers
    {
        //Convert bytes to long 
        //Used when reciving data from the controller device
        public long byteToLong(byte[] buff, int start, int len)
        {
            long val = 0;
            for (int i = 0; i < len && i < 4; i++)
            {
                long lng = buff[i + start];
                val += (lng << (8 * i));
            }
            return val;
        }

        public void log (string x){
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
            else if (recordType == 1) // Displays data whose record type is card number
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
                    "1","SwipePass","Swipe",
                    "2","SwipePass","Swipe Close",
                    "3","SwipePass","Swipe Open",
                    "4","SwipePass","Swipe Limited Times",
                    "5","SwipeNOPass","Denied Access: PC Control",
                    "6","SwipeNOPass","Denied Access: No PRIVILEGE",
                    "7","SwipeNOPass","Denied Access: Wrong PASSWORD",
                    "8","SwipeNOPass","Denied Access: AntiBack",
                    "9","SwipeNOPass","Denied Access: More Cards",
                    "10","SwipeNOPass","Denied Access: First Card Open",
                    "11","SwipeNOPass","Denied Access: Door Set NC",
                    "12","SwipeNOPass","Denied Access: InterLock",
                    "13","SwipeNOPass","Denied Access: Limited Times",
                    "14","SwipeNOPass","Denied Access: Limited Person Indoor",
                    "15","SwipeNOPass","Denied Access: Invalid Timezone",
                    "16","SwipeNOPass","Denied Access: In Order",
                    "17","SwipeNOPass","Denied Access: SWIPE GAP LIMIT",
                    "18","SwipeNOPass","Denied Access",
                    "19","SwipeNOPass","Denied Access: Limited Times",
                    "20","ValidEvent","Push Button",
                    "21","ValidEvent","Push Button Open",
                    "22","ValidEvent","Push Button Close",
                    "23","ValidEvent","Door Open",
                    "24","ValidEvent","Door Closed",
                    "25","ValidEvent","Super Password Open Door",
                    "26","ValidEvent","Super Password Open",
                    "27","ValidEvent","Super Password Close",
                    "28","Warn","Controller Power On",
                    "29","Warn","Controller Reset",
                    "30","Warn","Push Button Invalid: Disable",
                    "31","Warn","Push Button Invalid: Forced Lock",
                    "32","Warn","Push Button Invalid: Not On Line",
                    "33","Warn","Push Button Invalid: InterLock",
                    "34","Warn","Threat",
                    "35","Warn","Threat Open",
                    "36","Warn","Threat Close",
                    "37","Warn","Open too long",
                    "38","Warn","Forced Open",
                    "39","Warn","Fire",
                    "40","Warn","Forced Close",
                    "41","Warn","Guard Against Theft",
                    "42","Warn","7*24Hour Zone",
                    "43","Warn","Emergency Call",
                    "44","RemoteOpen","Remote Open Door",
                    "45","RemoteOpen","Remote Open Door By USB Reader",
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

        public void stringToBytes(ref byte[] outBytes, int startIndex, string name)
        {
            byte[] nameBytes = System.Text.Encoding.UTF8.GetBytes(name);
            if (nameBytes.Length > 28)
            {
                Array.Copy(nameBytes, 0, outBytes, startIndex, 28);
                Array.Copy(nameBytes, 28, outBytes, 40, 4);
            }
            else
            {
                Array.Copy(nameBytes, 0, outBytes, startIndex, nameBytes.Length);
            }

        }
        public string bytesToString (byte[] bytes,int startIndex,int length)
        {
            byte[] nameBytes= new byte[32];
            for(int i = 0; i < nameBytes.Length; i++)
            {
                nameBytes[i] = 0x00;
            }
            Array.Copy(bytes,startIndex, nameBytes,0, 28);
            return System.Text.Encoding.UTF8.GetString(nameBytes);
        }
    }

}