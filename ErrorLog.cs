using System;
using System.IO;

namespace IMKelly_EOLQA
{
    public class ErrorLog
    {
        //*******************************************************
        // Entering the error details into the error logfile into ErrorLog folder. 
        //*******************************************************

        static int FileCounter = 1;
        static string fileName = "IMKelly_EOLQA_ErrorLog_" + FileCounter + ".txt";
        string formName;
        public static void LogError(Exception ex, string formName)
        {
            string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("FormName: {0}", formName);
            message += Environment.NewLine;
            message += string.Format("Message: {0}", ex.Message);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", ex.StackTrace);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", ex.Source);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;


            // string fileName = "IMKelly_EOLQA_ErrorLog_" + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ".txt";



            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog/" + fileName);
            // string path = @"C:\temp\" + fileName;

            if (File.Exists(path))
            {
                string f1 = "";
                using (StreamReader sr = new StreamReader(path))
                {
                    f1 = sr.ReadToEnd();
                    sr.Close();
                    File.Delete(path);
                    //f1 = File.ReadAllText(path);
                }

                //string f1 = File.ReadAllText("~/ErrorLog/" + fileName);
                if ((f1.Length) / 1000 > 5000)
                {
                    // Create new File
                    FileCounter++;
                    fileName = "IMKelly_EOLQA_ErrorLog_" + FileCounter + ".txt";
                    string pathForNewLogFile = System.Web.Hosting.HostingEnvironment.MapPath("~/ErrorLog/" + fileName);
                    using (StreamWriter writer = new StreamWriter(pathForNewLogFile, true))
                    {
                        writer.WriteLine(message);
                        writer.Close();
                        File.Delete(pathForNewLogFile);

                    }
                }
                else
                {
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(message);
                        writer.Close();
                        File.Delete(path);
                    }
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                    File.Delete(path);
                }
            }
        }
    }
}