using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Common
{
    public class WkHtmlToPdfHelper
    {
        /// <summary>
        /// HTML文本内容转换为PDF
        /// </summary>
        /// <param name="strHtml">HTML文本内容</param>
        /// <param name="savePath">PDF文件保存的路径</param>
        /// <returns></returns>
        public static bool HtmlTextConvertToPdf(string strHtml, string savePath, string customizeStyle = "")
        {
            bool flag = false;
            try
            {
                string htmlPath = HtmlTextConvertFile(strHtml);

                flag = HtmlConvertToPdf(htmlPath, savePath, customizeStyle);
                File.Delete(htmlPath);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// HTML转换为PDF
        /// </summary>
        /// <param name="htmlPath">可以是本地路径，也可以是网络地址</param>
        /// <param name="savePath">PDF文件保存的路径</param>
        /// <returns></returns>
        public static bool HtmlConvertToPdf(string htmlPath, string savePath, string customizeStyle = "")
        {
            bool flag = false;
            CheckFilePath(savePath);

            ///这个路径为程序集的目录，因为我把应用程序 wkhtmltopdf.exe 放在了程序集同一个目录下
            string exePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "wkhtmltopdf.exe";
            if (!File.Exists(exePath))
            {
                throw new Exception("No application wkhtmltopdf.exe was found.");
            }
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = string.Format("\"{0}\"",exePath);//防止路径有空格
                processStartInfo.WorkingDirectory = Path.GetDirectoryName(exePath);
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.StandardOutputEncoding = Encoding.UTF8;
            
                processStartInfo.RedirectStandardInput = true;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
                processStartInfo.Arguments = customizeStyle + GetArguments(string.Format("\"{0}\"", htmlPath), string.Format("\"{0}\"", savePath));

                processStartInfo.ErrorDialog = true;
                processStartInfo.StandardErrorEncoding = Encoding.UTF8;
                

                Process process = new Process();
                process.StartInfo = processStartInfo;
                process.Start();

                System.IO.StreamReader outputStreamReader = process.StandardOutput;
                System.IO.StreamReader errStreamReader = process.StandardError;

                process.WaitForExit();
                if (process.HasExited)
                {
                    string output = outputStreamReader.ReadToEnd();
                    string error = errStreamReader.ReadToEnd();
                  
                }
               

                process.Close();
                process.Dispose();

                flag = true;
            }
            catch
            {
                flag = false;
            }
            return flag;
        }


        /// <summary>
        /// 获取命令行参数
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        private static string GetArguments(string htmlPath, string savePath)
        {
            if (string.IsNullOrEmpty(htmlPath))
            {
                throw new Exception("HTML local path or network address can not be empty.");
            }

            if (string.IsNullOrEmpty(savePath))
            {
                throw new Exception("The path saved by the PDF document can not be empty.");
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(" --page-height 297 ");        //页面高度100mm
            stringBuilder.Append(" --page-width 210 ");         //页面宽度100mm
            stringBuilder.Append(" --margin-top 10 ");
            stringBuilder.Append(" " + htmlPath + " ");       //本地 HTML 的文件路径或网页 HTML 的URL地址
            stringBuilder.Append(" " + savePath + " ");       //生成的 PDF 文档的保存路径
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 验证保存路径
        /// </summary>
        /// <param name="savePath"></param>
        private static void CheckFilePath(string savePath)
        {
            string ext = string.Empty;
            string path = string.Empty;
            string fileName = string.Empty;

            ext = Path.GetExtension(savePath);
            if (string.IsNullOrEmpty(ext) || ext.ToLower() != ".pdf")
            {
                throw new Exception("Extension error:This method is used to generate PDF files.");
            }

            fileName = Path.GetFileName(savePath);
            if (string.IsNullOrEmpty(fileName))
            {
                throw new Exception("File name is empty.");
            }

            try
            {
                path = savePath.Substring(0, savePath.IndexOf(fileName));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
                throw new Exception("The file path does not exist.");
            }
        }

        /// <summary>
        /// HTML文本内容转HTML文件
        /// </summary>
        /// <param name="strHtml">HTML文本内容</param>
        /// <returns>HTML文件的路径</returns>
        public static string HtmlTextConvertFile(string strHtml)
        {
            if (string.IsNullOrEmpty(strHtml))
            {
                throw new Exception("HTML text content cannot be empty.");
            }

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"html\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = path + DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(1000, 10000) + ".html";
                FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
                streamWriter.Write(strHtml);
                streamWriter.Flush();

                streamWriter.Close();
                streamWriter.Dispose();
                fileStream.Close();
                fileStream.Dispose();
                return fileName;
            }
            catch
            {
                throw new Exception("HTML text content error.");
            }
        }
        /// <summary>
        /// 将文件转换成byte[] 数组
        /// </summary>
        /// <param name="fileName">文件路径文件名称</param>
        /// <returns>byte[]</returns>
        public static byte[] GetFileData(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            try
            {
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, (int)fs.Length);

                return buffur;
            }
            catch (Exception ex)
            {
                //MessageBoxHelper.ShowPrompt(ex.Message);
                return null;
            }
            finally
            {
                if (fs != null)
                {

                    //关闭资源
                    fs.Close();
                }
            }
        }


        /// <summary>
        /// 将文件转换成byte[] 数组
        /// </summary>
        /// <param name="fileName">文件路径文件名称</param>
        /// <returns>byte[]</returns>

        public static byte[] AuthGetFileData(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                byte[] buffur = new byte[fs.Length];
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    bw.Write(buffur);
                    bw.Close();
                }
                return buffur;
            }
        }
    }
    
}