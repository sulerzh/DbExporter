using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DbExporter.Provider.QS2000
{
    public class BitmapImageHelper
    {
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片告诉</param>
        /// <param name="data">像素数据</param>
        /// <param name="file">保存路径</param>
        public static void SaveBitmap(int width, int height, byte[] data, string file)
        {
            int bitsPerPixel = 24;
            int stride = (width * bitsPerPixel + 7) / 8;

            // Single step creation of the image
            var bmps = BitmapSource.Create(width, height, 96, 96,
                                PixelFormats.Bgr24, null, data, stride);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmps));

            using (var fs = File.OpenWrite(file))
            {
                encoder.Save(fs);
            }
        }

        /// <summary>
        /// 创建Bitmap
        /// </summary>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片告诉</param>
        /// <param name="data">像素数据</param>
        /// <param name="needRotate">是否需要旋转</param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(int width, int height, byte[] data, bool needRotate)
        {
            int bitsPerPixel = 24;
            int stride = (width * bitsPerPixel + 7) / 8;

            // Single step creation of the image
            var bmps = BitmapSource.Create(width, height, 96, 96,
                                PixelFormats.Bgr24, null, data, stride);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmps));

            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                Bitmap bmp = new Bitmap(ms);
                // 旋转
                if (needRotate)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate90FlipX);
                }

                return bmp;
            }
        }

        /// <summary>
        /// 合并并整饰数据（添加文字标题）
        /// </summary>
        /// <param name="srcMaps">待合并数组</param>
        /// <param name="bands">波段名称数组</param>
        /// <returns></returns>
        public static Bitmap FixImageMerge(List<Bitmap> srcMaps, List<string> bands)
        {
            // 创建要显示的图片对象,根据参数的个数设置宽度
            int borderWidth = 10;
            int borderHeader = 43;
            int imageWidth = 30;
            int imageHeight = 130;
            int xImagePad = 8;
            int xTextPad = 8;
            int yTextPad = 10;
            int destWidth = 240;
            int destHeight = 210;
            Bitmap destMap = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(destMap);

            // 清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.White);

            // 按顺序合并
            int offsetWidth = borderWidth;
            var borderPen = new System.Drawing.Pen(System.Drawing.Color.Black, 1);
            g.DrawRectangle(borderPen, 0, 0, destMap.Width - 1, destMap.Height - 1);
            for (int i = 0; i < srcMaps.Count; i++)
            {
                var map = srcMaps[i];
                string bandName = bands[i];
                // 绘制文字
                Font font = new Font("Segoe UI", 12, FontStyle.Regular);
                SolidBrush sbrush = new SolidBrush(System.Drawing.Color.Black);
                g.DrawString(bandName, font, sbrush, new PointF(offsetWidth + xTextPad, yTextPad));

                // 绘制边框和图片
                Rectangle rect = new Rectangle(offsetWidth, borderHeader, imageWidth, imageHeight);
                g.DrawImage(map, rect, 0, 0, map.Width, map.Height, GraphicsUnit.Pixel);
                g.DrawRectangle(borderPen, rect);
                offsetWidth += imageWidth;
                offsetWidth += xImagePad;
            }

            g.Dispose();
            return destMap;
        }

        public static Bitmap MergerImg(List<Bitmap> srcMaps, int padWidth)
        {
            int destWidth = padWidth * (srcMaps.Count + 1);
            int destHeight = 0;
            // 计算合并后的宽度和高度
            foreach (var map in srcMaps)
            {
                destWidth += map.Width;
                if (map.Height > destHeight)
                {
                    destHeight = map.Height;
                }
            }

            // 创建要显示的图片对象,根据参数的个数设置宽度
            Bitmap destMap = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage(destMap);

            // 清除画布,背景设置为白色
            g.Clear(System.Drawing.Color.LightGray);

            // 按顺序合并
            int offsetWidth = padWidth;
            foreach (var map in srcMaps)
            {
                g.DrawImage(map, offsetWidth, 0, map.Width, map.Height);
                offsetWidth += map.Width;
                offsetWidth += padWidth;
            }

            g.Dispose();
            return destMap;
        }
    }
}
