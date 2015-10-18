using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.IO;
using System.Management;
using System.Net;
using System.Security.Cryptography;


namespace RegRainbowEcommerce
{
    class Util
    {  
        public static string GetAuthKey(string prodName, string hardwareID)
        {
            string s = "";
            int startIndex = 0;
            do
            {
                s = s + Strings.Asc(prodName.Substring(startIndex, 1)).ToString();
                startIndex++;
            }
            while (startIndex <= 3);
            long num4 = long.Parse(s);
            long num = long.Parse(hardwareID) ^ num4;
            if (num < 0x3b9aca00L)
            {
                num += 0x3b9aca00L;
            }
            return num.ToString();
        }

      
 

        public static int GetKeyFromRemoteServer(string regURL)
        {
            string s = new StreamReader(WebRequest.Create(regURL).GetResponse().GetResponseStream(), true).ReadToEnd().ToLower();
            int index = s.IndexOf("answer:");
            if (index != -1)
            {
                s = s.Substring(index + 7, 1);
                int result = 0;
                if (int.TryParse(s, out result))
                {
                    return result;
                }
            }
            return -1;
        }

        public static string GetLocalIP()
        {
            string str2 = "";
            IPAddress[] addressList = Dns.GetHostByName(Environment.MachineName).AddressList;
            if (addressList.Length > 0)
            {
                str2 = addressList[0].ToString();
            }
            return str2;
        }

        internal static string GetMACAddress()
        {
            ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            string str2 = string.Empty;
            foreach (ManagementObject obj2 in instances)
            {
                if (str2.Equals(string.Empty))
                {
                    if (Conversions.ToBoolean(obj2["IPEnabled"]))
                    {
                        str2 = obj2["MacAddress"].ToString();
                    }
                    obj2.Dispose();
                }
                str2 = str2.Replace(":", string.Empty);
            }
            return str2;
        }

        public static string GetMD5Data(string inputString)
        {
            byte[] buffer = null;
            string str;
            byte[] bytes = null;
            try
            {
                StringBuilder builder = new StringBuilder();
                bytes = Encoding.UTF8.GetBytes(inputString);
                buffer = new MD5CryptoServiceProvider().ComputeHash(bytes);
                int num2 = buffer.Length - 1;
                for (int i = 0; i <= num2; i++)
                {
                    builder.Append(buffer[i].ToString("x2"));
                }
                str = builder.ToString();
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                str = null;
                ProjectData.ClearProjectError();
            }
            return str;
        }

        public static string GetMotherBoardID()
        {
            string str2 = string.Empty;
            SelectQuery query = new SelectQuery("Win32_BaseBoard");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject obj2 in searcher.Get())
            {
                str2 = obj2["SerialNumber"].ToString();
            }
            return str2;
        }

        public static string GetProcessorId()
        {
            string str2 = string.Empty;
            SelectQuery query = new SelectQuery("Win32_processor");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (ManagementObject obj2 in searcher.Get())
            {
                str2 = obj2["processorId"].ToString();
            }
            return str2;
        }

        public static object GetProdKey(string prodName, string serialNo, string authKey)
        {
            string str = "";
            return ((((str + authKey.Substring(0, 5)) + "-" + prodName.Substring(2, 3)) + "-" + serialNo) + "-" + authKey.Substring(5, 5));
        }

        public static string GetRealIP()
        {
            string str3;
            string requestUriString = "http://checkrealip.com";
            try
            {
                str3 = new StreamReader(WebRequest.Create(requestUriString).GetResponse().GetResponseStream(), true).ReadToEnd();
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
                return "";
            }
            str3 = str3.ToLower();
            int index = str3.IndexOf("address:");
            if (index == -1)
            {
                return "";
            }
            str3 = str3.Substring(index + 8);
            index = str3.IndexOf("\r\n");
            if (index == -1)
            {
                return "";
            }
            return str3.Substring(0, index);
        }

        public static string GetSysInfoHash()
        {
            string inputString = "";
            inputString = string.Format("{0}{1}{2}{3}", new object[] { GetMACAddress(), GetMotherBoardID(), GetProcessorId(), GetVolumeSerial("C") }) + "I-FREELANCER";
            string str3 = "";
            if (GetMD5Data(inputString) != null)
            {
                str3 = GetMD5Data(inputString);
            }
            return str3;
        }

        public static string GetSystemIdNumber()
        {
            long num = Convert.ToInt64(GetVolumeSerial("C"), 0x10);
            string str4 = string.Format("{0}{1}{2}", GetMACAddress(), GetMotherBoardID(), GetProcessorId()).ToUpper();
            int num2 = 0;
            int length = str4.Length;
            while (num2 < length)
            {
                char ch = str4[num2];
                num += Strings.Asc(ch);
                num2++;
            }
            num = num % 0x2540be400L;
            if (num < 0x3b9aca00L)
            {
                num += 0x3b9aca00L;
            }
            return num.ToString();
        }

        public static string GetSystemIdNumberX()
        {
            long num = Convert.ToInt64(GetVolumeSerial("C"), 0x10);
            string str4 = string.Format("{0}{1}{2}", GetMACAddress(), GetMotherBoardID(), GetProcessorId()).ToUpper();
            int num2 = 0;
            int length = str4.Length;
            while (num2 < length)
            {
                char ch = str4[num2];
                num += Strings.Asc(ch);
                num2++;
            }
            num = num % 0x2540be400L;
            if (num < 0x3b9aca00L)
            {
                num += 0x3b9aca00L;
            }
            return string.Format("{0:X}", num);
        }

        public static string GetVersion(string URL)
        {
            string str2 = "";
            try
            {
                str2 = new StreamReader(WebRequest.Create(URL).GetResponse().GetResponseStream(), true).ReadToEnd();
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                ProjectData.ClearProjectError();
            }
            return str2;
        }

        public static string GetVolumeSerial(string strDriveLetter = "C")
        {
            ManagementObject obj2 = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}:\"", strDriveLetter));
            obj2.Get();
            return obj2["VolumeSerialNumber"].ToString();
        }

        public static string ValidateSN(string ProdSN)
        {
            string[] strArray = ProdSN.Split(new char[] { '-' });
            string str = strArray[0].ToUpper();
            string str2 = strArray[1].ToUpper();
            string s = GetMD5Data(str2 + str);
            string str3 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).ToUpper().Substring(0, 5);
            s = GetMD5Data(str3 + str2 + str);
            string str4 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).ToUpper().Substring(0, 5);
            s = GetMD5Data(str4 + str3 + str2 + str);
            string str5 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s)).ToUpper().Substring(0, 5);
            return ((str + "-" + str2 + "-" + str3 + "-" + str4 + "-" + str5).Replace("I", "1").Replace("O", "0").Replace("U", "V").Replace("Z", "2")  );
        }



    }
}
