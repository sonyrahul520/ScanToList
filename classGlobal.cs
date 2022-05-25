using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace IMKelly_ClassLibrary
{

    public class classGlobal
    {
        [ThreadStatic]
        public static SqlConnection oCon;
        public static SqlConnection oConExtension;
        public static string primaryConnectionString; // Connection string to the 123 database
        public static string securityConnectionString; // Connection string to the security database  
        public static string extensionConnectionString;
        public static string amalgumConnectionString;
        public static string fbConnectionString;
        public static void createConnection(string conStr)
        {
            oCon = new SqlConnection(conStr);
        }

        //***************************************************************************************
        // Generate check digit
        //***************************************************************************************
        public static string checkDigitGS1(string source)
        {
            bool doDouble = true;
            int total = 0;
            for (int x = 0; x < source.Length; x++)
            {
                int indexNumber = Convert.ToInt32(source.Substring(x, 1));
                int thisNumber = doDouble ? indexNumber * 3 : indexNumber;
                doDouble = !doDouble;
                total += thisNumber;
            }
            int genCheckDigit = total % 10 == 0 ? 0 : 10 - (total % 10);
            return source + genCheckDigit.ToString();
        }

        //***************************************************************************************
        // Generate check digit
        //***************************************************************************************
        public static string checkDigit(string source)
        {
            bool doDouble = false;
            int total = 0;
            for (int x = 0; x < source.Length; x++)
            {
                int indexNumber = Convert.ToInt32(source.Substring(x, 1));
                int thisNumber = doDouble ? indexNumber * 3 : indexNumber;
                doDouble = !doDouble;
                total += thisNumber;
            }
            int genCheckDigit = total % 10 == 0 ? 0 : 10 - (total % 10);
            return source + genCheckDigit.ToString();
        }
    }
}
