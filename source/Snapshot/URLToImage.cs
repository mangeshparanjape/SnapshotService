using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Web; 
namespace Snapshot
{
    public class URLToImage
    {
        public string GetSnapshot(string URL,string imageType, string imageWidth, string imageHeight)
        {
            string base64Image = string.Empty;
            try
            {
                string viewPortWidth = imageWidth ;
                string viewPortHeight = imageHeight;
                string resourceTimeout = Convert.ToString(ConfigurationManager.AppSettings["ResourceTimeout"]);
                string pageRenderDelay = Convert.ToString(ConfigurationManager.AppSettings["PageRenderDelay"]);
                string logFilePath = string.Empty;
                string clipRectangleWidth = imageWidth;
                string clipRectangleHeight = imageHeight;
                string phantomJSPath = Convert.ToString(ConfigurationManager.AppSettings["PhantomJSDirectory"]);
                string imageName = System.Guid.NewGuid().ToString() + "." + imageType  ;
                string imagePath = HttpContext.Current.Server.MapPath("~/PhantomJS/Images/");
                string httpProtocol = "HTTP://";
                string httpsProtocol = "HTTPS://";

                if (!URL.Trim().ToUpper().Contains(httpProtocol) && !URL.Trim().ToUpper().Contains(httpsProtocol)) URL = httpProtocol + URL;

                ExecuteCommand(string.Format("cd \"{0}\" & phantomjs --config=config.json render.js \"{1}\" \"{2}\" {3} {4} {5} {6} {7} {8} {9}",
                                phantomJSPath,
                                URL,
                                imageName,
                                viewPortWidth,
                                viewPortHeight,
                                clipRectangleWidth,
                                clipRectangleHeight,
                                imageType.ToString().ToLower(),
                                resourceTimeout,
                                pageRenderDelay));

                if (File.Exists(imagePath + imageName))
                {
                    Image image = Image.FromFile(imagePath + imageName); 
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Convert Image to byte[]
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
              
            }
            catch
            {
                base64Image = string.Empty ;
            }
            return base64Image;
        }

        private void ExecuteCommand(string Command)
        {
            try
            {
                string workingDirectory = Convert.ToString(ConfigurationManager.AppSettings["PhantomJSDirectory"]);
                bool showWindow = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowPhantomJSWindow"]);
                int ProcessTimeout = Convert.ToInt32 (ConfigurationManager.AppSettings["ProcessTimeout"]);
                ProcessStartInfo ProcessInfo = null;
                Process Process = null;
                ProcessInfo = new ProcessStartInfo("cmd.exe", "/K " + Command + " & exit");
                ProcessInfo.CreateNoWindow = !showWindow;
                ProcessInfo.UseShellExecute = false;
                ProcessInfo.WorkingDirectory = workingDirectory;
                Process = Process.Start(ProcessInfo);
                if (! Process.WaitForExit(ProcessTimeout))
                {
                    if (!Process.HasExited)
                    {
                        try
                        {
                            if (!Process.CloseMainWindow())
                            {
                                Process.Kill();
                            }
                        }
                        catch
                        {
                            Process p =new Process();
                            p.StartInfo.FileName = "taskkill";
                            p.StartInfo.Arguments = "/PID " + Process.Id + " /f ";
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            p.Start();
                        }
                    }
                }

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
