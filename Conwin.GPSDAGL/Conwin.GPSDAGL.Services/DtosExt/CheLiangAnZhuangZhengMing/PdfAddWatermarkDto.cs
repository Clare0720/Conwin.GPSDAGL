using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.DtosExt.CheLiangAnZhuangZhengMing
{
    public class PdfAddWatermarkDto
    {
        /// <summary>
        /// PDF文件ID
        /// </summary>
        public Guid PdfFileId { get; set; }
        /// <summary>
        /// 添加水印后输出PDF文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 水印列表
        /// </summary>
        public List<WatermarkInfoDto> WatermarkList { get; set; }

        public List<QRCodeInfoDto> QRCodelFileList { get; set; }
    }
    public class WatermarkInfoDto
    {
        /// <summary>
        /// 水印图片文件ID
        /// </summary>
        public Guid? WatermarkFileID { get; set; }
        /// <summary>
        /// 需要在PDF第几页添加水印(0为第一页)
        /// </summary>
        public int PdfPage { get; set; } = 0;
        /// <summary>
        /// X起点坐标
        /// </summary>
        public int XCoordinate { get; set; }
        /// <summary>
        /// Y起点坐标
        /// </summary>
        public int YCoordinate { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }

    public class QRCodeInfoDto
    {
        /// <summary>
        /// 水印图片文件
        /// </summary>
        public byte[] FileData { get; set; }
        /// <summary>
        /// 需要在PDF第几页添加水印(0为第一页)
        /// </summary>
        public int PdfPage { get; set; } = 0;
        /// <summary>
        /// X起点坐标
        /// </summary>
        public int XCoordinate { get; set; }
        /// <summary>
        /// Y起点坐标
        /// </summary>
        public int YCoordinate { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height { get; set; }
    }

}
