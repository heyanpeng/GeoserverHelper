using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;

namespace GeoserverHelper
{
    /// <summary>
    /// MyGeoserver 的摘要说明
    /// </summary>
    public class MyGeoserver : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            string handleName = context.Request["HandleName"];
            if (handleName == "submitStyleContent")
            {
                submitStyleContent(context);
            }
            else if (handleName == "helloworld")
            {
                context.Response.Write("helloworld change");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private void submitStyleContent(HttpContext context)
        {
            string WorkSpaces = context.Request["WorkSpaces"];
            string SldFileName = context.Request["SldFileName"];
            string SldStr = context.Request["SldStr"];
            try
            {
                Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                string GeoserverPath = config.AppSettings.Settings["GeoserverPath"].Value;
                createFolder(GeoserverPath + "\\data_dir");
                createFolder(GeoserverPath + "\\data_dir\\workspaces");
                createFolder(GeoserverPath + "\\data_dir\\workspaces\\" + WorkSpaces);
                createFolder(GeoserverPath + "\\data_dir\\workspaces\\" + WorkSpaces + "\\styles\\");
                deleteFile(GeoserverPath + "\\data_dir\\workspaces\\" + WorkSpaces + "\\styles\\" + SldFileName);
                FileStream fs = new FileStream(GeoserverPath + "\\data_dir\\workspaces\\" + WorkSpaces + "\\styles\\" + SldFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding("GB2312"));
                sw.WriteLine(SldStr);
                sw.Close();
            }
            catch (System.Exception ex)
            {
                context.Response.Write(ex.Message);
            }
        }

        private void createFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void deleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}