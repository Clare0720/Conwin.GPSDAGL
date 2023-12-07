using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Conwin.Framework.Log4net;
using Conwin.Framework.Log4net.Extendsions;

namespace Conwin.GPSDAGL.Services.Common
{
    public class PDFHelper
    {
        /// <summary>
        /// 將Html文字转换为PDF文档
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns>返回文件ID</returns>
        public bool ConvertHtmlTextToPDFStr(string htmlText, string filename)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return false;
            }   
            bool result = true;
            Document doc = new Document(PageSize.A4);
            try
            {
                string path = ("~" + UrlCollection.XingZhengWenShuFile).MapPath();
                if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + filename + ".pdf";
                byte[] data = Encoding.UTF8.GetBytes(htmlText);
                using (MemoryStream msInput = new MemoryStream(data))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                    PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                    doc.Open();
                    XMLWorkerHelper.GetInstance()
                        .ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new FontFactory());
                    PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                    writer.SetOpenAction(action);
                }

            }
            catch (Exception ex)
            {
                result = false;
                LogHelper.Fatal(string.Format("生成PDF文档异常：{0}", ex.ToString()));
            }
            finally
            {
                doc.Close();
            }

            return result;
        }
        /// <summary>
        /// 將Html文字转换为PDF文档
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns>返回文件ID</returns>
        public bool ConvertHtmlTextToPDFStrByWkHtmlToPdf(string htmlText, string filename)
        {
            if (string.IsNullOrEmpty(htmlText))
            {
                return false;
            }
            bool result = true;
            Document doc = new Document(PageSize.A4);
            try
            {
                string path = ("~" + UrlCollection.XingZhengWenShuFile).MapPath();
                if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "\\" + filename + ".pdf";
                WkHtmlToPdfHelper.HtmlTextConvertToPdf(htmlText, path);

            }
            catch (Exception ex)
            {
                result = false;
                LogHelper.Fatal(string.Format("生成PDF文档异常：{0}", ex.ToString()));
            }
            finally
            {
                doc.Close();
            }

            return result;
        }
        /// <summary>
        /// 將Html文字转换为PDF文档
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns>返回文件ID</returns>
        public byte[] ConvertHtmlTextToPDFStrByWkHtmlToPdfReturnFile(string htmlText, string filename, out string filePath, string customizeStyle = "")
        {
            byte[] result = null;
            filePath = null;
            Document doc = new Document(PageSize.A4);
            try
            {
                filePath = ("~" + UrlCollection.XingZhengWenShuFile).MapPath();
                if (Directory.Exists(filePath) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath = filePath + "\\" + filename;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                if (WkHtmlToPdfHelper.HtmlTextConvertToPdf(htmlText, filePath, customizeStyle))
                {
                    result = WkHtmlToPdfHelper.GetFileData(filePath);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                result = null;
                LogHelper.Fatal(string.Format("生成PDF文档异常：{0}", ex.ToString()));
            }
            finally
            {
                doc.Close();
            }

            return result;
        }
        /// <summary>
        /// 將Html文字转换为PDF文档
        /// </summary>
        /// <param name="htmlText"></param>
        /// <returns></returns>
        public byte[] ConvertHtmlTextToPDF(string htmlText)
        {
            byte[] result = null;
            if (string.IsNullOrEmpty(htmlText))
            {
                return result;
            }
            Document doc = new Document(PageSize.A4);
            MemoryStream msOutput = new MemoryStream();
            try
            {
                byte[] data = Encoding.UTF8.GetBytes(htmlText);
                using (MemoryStream msInput = new MemoryStream(data))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, msOutput);
                    PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                    doc.Open();
                    XMLWorkerHelper.GetInstance()
                        .ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new FontFactory());
                    PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                    writer.SetOpenAction(action);
                }

            }
            catch (Exception ex)
            {
                result = null;
                LogHelper.Fatal(string.Format("生成PDF文档异常：{0}", ex.ToString()));
            }
            finally
            {
                doc.Close();
                msOutput.Close();
                result = msOutput.ToArray();
            }

            return result;
        }
    }

    public class FontFactory : FontFactoryImp
    {
        private static readonly string arialFontPath = "~\\Config\\Fonts\\STFANGSO.ttf".MapPath();//仿宋 常规

        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
            bool cached)
        {
            BaseFont baseFont = BaseFont.CreateFont(arialFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }

}