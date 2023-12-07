using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections;
using Gma.QrCodeNet.Encoding;
using System.IO;
using Gma.QrCodeNet.Encoding.Windows.Controls;
using System.Drawing.Imaging;
using Conwin.Framework.Log4net.Extendsions;

namespace Conwin.GPSDAGL.Services.Common
{
    public class BarCodeHelper
    {
        /// <summary>
        /// 获取二维码方法
        /// </summary>
        /// <param name="msg">存储数据</param>
        /// <param name="size">尺寸</param>
        /// <returns></returns>
        public static void GetQRCode(System.Web.HttpResponse response, string msg, int size)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = new QrCode();

            qrEncoder.TryEncode(msg, out qrCode);

            Renderer renderer = new Renderer(size, Brushes.Black, Brushes.White);

            MemoryStream ms = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);
            //return ms.ToArray();

            response.Clear();
            response.ContentType = "image/jpeg";
            response.BinaryWrite(ms.ToArray());
            response.End();

        }

        /// <summary>
        /// 获取二维码方法
        /// </summary>
        /// <param name="msg">存储数据</param>
        /// <param name="size">尺寸</param>
        /// <returns></returns>
        public static void GetQRCode(System.Web.HttpResponse response, string msg, int size, Color color, bool border)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = new QrCode();

            qrEncoder.TryEncode(msg, out qrCode);
            SolidBrush brush = new SolidBrush(color);
            Renderer renderer = new Renderer(size, brush, Brushes.White);

            MemoryStream ms = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);

            Bitmap bm = new Bitmap(ms);
            Graphics g = Graphics.FromImage(bm);

            if (border)
            {
                Pen pen = new Pen(brush);
                g.DrawLine(pen, new Point(0, 0), new Point(bm.Width - 1, 0));
                g.DrawLine(pen, new Point(0, 0), new Point(0, bm.Height - 1));
                g.DrawLine(pen, new Point(bm.Width - 1, 0), new Point(bm.Width - 1, bm.Height - 1));
                g.DrawLine(pen, new Point(0, bm.Height - 1), new Point(bm.Width - 1, bm.Height - 1));
            }
            else
            {
                g.DrawLine(Pens.White, new Point(bm.Width - 1, 0), new Point(bm.Width - 1, bm.Height - 1));
                g.DrawLine(Pens.White, new Point(0, bm.Height - 1), new Point(bm.Width - 1, bm.Height - 1));
            }

            g.Save();

            //return ms.ToArray();
            MemoryStream ms2 = new MemoryStream();
            bm.Save(ms2, ImageFormat.Jpeg);
            response.Clear();
            response.ContentType = "image/jpeg";
            response.BinaryWrite(ms2.ToArray());
            ms.Close();
            ms2.Close();
            response.End();

        }

        /// <summary>
        /// 获取无边框二维码
        /// </summary>
        /// <param name="msg">存储数据</param>
        /// <param name="size">尺寸</param>
        /// <returns></returns>
        public static void GetQRCodeNoBorder(System.Web.HttpResponse response, string msg, int size)
        {
            QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
            QrCode qrCode = new QrCode();

            qrEncoder.TryEncode(msg, out qrCode);

            Renderer renderer = new Renderer(size, Brushes.Black, Brushes.White);

            MemoryStream ms = new MemoryStream();
            renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);
            //return ms.ToArray();
            Bitmap objBitmap = null;
            Graphics g = null;
            try
            {
                objBitmap = new Bitmap(Image.FromStream(ms));
                g = Graphics.FromImage(objBitmap);
                Pen pen = Pens.White;
                g.DrawLine(pen, objBitmap.Width - 1, 0, objBitmap.Height - 1, objBitmap.Width - 1);
                g.DrawLine(pen, 0, objBitmap.Height - 1, objBitmap.Height - 1, objBitmap.Width - 1);
                g.Save();
                response.Clear();
                response.ContentType = "image/jpeg";
                objBitmap.Save(response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                //response.BinaryWrite(ms.ToArray());
                response.End();
            }
            finally
            {
                if (null != ms)
                {
                    ms.Dispose();
                }
                if (null != objBitmap)
                    objBitmap.Dispose();
                if (null != g)
                    g.Dispose();
            }



        }

        /// <summary>
        /// 获取二维码方法
        /// </summary>
        /// <param name="msg">存储数据</param>
        /// <param name="size">尺寸</param>
        /// <returns>Base64String</returns>
        public static string GetQRCode(string content, int size)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                Renderer renderer = new Renderer(size, Brushes.Black, Brushes.White);
                renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);
                return Convert.ToBase64String(ms.ToArray(), Base64FormattingOptions.None);
            }
            catch
            {
                ms.Dispose();
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取二维码方法
        /// </summary>
        /// <param name="msg">存储数据</param>
        /// <param name="size">尺寸</param>
        /// <returns>Base64String</returns>
        public static byte[] GetQRCodeByte(string content, int size)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap objBitmap = null;
            Graphics g = null;
            try
            {
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                Renderer renderer = new Renderer(size, Brushes.Black, Brushes.White);
                renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);

                objBitmap = new Bitmap(Image.FromStream(ms));
                g = Graphics.FromImage(objBitmap);
                Pen pen = Pens.White;
                g.DrawLine(pen, objBitmap.Width - 2, 0, objBitmap.Height - 2, objBitmap.Width - 2);
                g.DrawLine(pen, 0, objBitmap.Height - 2, objBitmap.Height - 2, objBitmap.Width - 2);
                g.Save();
                objBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
            catch
            {
                ms.Dispose();
                return null ;
            }
        }

        public static string WriteQRCode(string content, int size)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap objBitmap = null;
            Graphics g = null;
            string path = string.Empty;
            try
            {
                path = ("~" + UrlCollection.XingZhengWenShuFile).MapPath();
                if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(path);
                }
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                Renderer renderer = new Renderer(size, Brushes.Black, Brushes.White);
                renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);
                objBitmap = new Bitmap(Image.FromStream(ms));
                g = Graphics.FromImage(objBitmap);
                Pen pen = Pens.White;
                g.DrawLine(pen, objBitmap.Width - 1, 0, objBitmap.Height - 1, objBitmap.Width - 1);
                g.DrawLine(pen, 0, objBitmap.Height - 1, objBitmap.Height - 1, objBitmap.Width - 1);
                g.Save();
                path = path + "\\" + Guid.NewGuid().ToString() + ".png";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                objBitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch(Exception ex)
            {
                string ss = ex.Message;
                path = string.Empty;
                ms.Dispose();
            }
            finally
            {
                if (null != ms)
                {
                    ms.Dispose();
                }
                if (null != objBitmap)
                    objBitmap.Dispose();
                if (null != g)
                    g.Dispose();
            }
            return path;
        }

        public static string WriteQRCode(string content, int size, string filename)
        {
            MemoryStream ms = new MemoryStream();
            Bitmap objBitmap = null;
            Graphics g = null;
            string path = string.Empty;
            try
            {
                path = ("~" + UrlCollection.XingZhengWenShuFile).MapPath();
                if (Directory.Exists(path) == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(path);
                }
                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.M);
                QrCode qrCode = new QrCode();
                qrEncoder.TryEncode(content, out qrCode);
                Renderer renderer = new Renderer(size, Brushes.Black, Brushes.White);
                renderer.WriteToStream(qrCode.Matrix, ms, ImageFormat.Jpeg);
                objBitmap = new Bitmap(Image.FromStream(ms));
                g = Graphics.FromImage(objBitmap);
                Pen pen = Pens.White;
                g.DrawLine(pen, objBitmap.Width - 1, 0, objBitmap.Height - 1, objBitmap.Width - 1);
                g.DrawLine(pen, 0, objBitmap.Height - 1, objBitmap.Height - 1, objBitmap.Width - 1);
                g.Save();
                path = path + "\\" + filename + ".png";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                objBitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }
            catch
            {
                path = string.Empty;
                ms.Dispose();
            }
            finally
            {
                if (null != ms)
                {
                    ms.Dispose();
                }
                if (null != objBitmap)
                    objBitmap.Dispose();
                if (null != g)
                    g.Dispose();
            }
            return path;
        }
    }
}
