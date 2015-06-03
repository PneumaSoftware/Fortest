using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Configuration;
using System.Drawing.Imaging;
using System.Drawing;

namespace VS2008.Module
{
    public class resizeImage
    {
        /// <summary>
        /// 壓縮及resize圖檔
        /// </summary>
        /// <param name="strFiles">圖檔來源</param>
        /// <param name="strFile_distinct">存檔名稱</param>
        /// <param name="_maxThumbWidth">resize 圖片寬度</param>
        /// <param name="_maxThumbHeight">resize 圖片高度 </param>
        public void thumImage(string strFiles, string strFile_distinct, int _maxThumbWidth, int _maxThumbHeight)
        {
           
            System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(strFiles);

            decimal sizeRatio = ((decimal)fullSizeImg.Height / fullSizeImg.Width);

            int thumbWidth = 0;
            int thumbHeight = 0;

            if (_maxThumbHeight != 0 && fullSizeImg.Height > fullSizeImg.Width)
            {
                sizeRatio = ((decimal)fullSizeImg.Width / fullSizeImg.Height);

                thumbHeight = _maxThumbHeight;
                thumbWidth = decimal.ToInt32(sizeRatio * thumbHeight);
            }
            else
            {
                thumbWidth = _maxThumbWidth;
                thumbHeight = decimal.ToInt32(sizeRatio * thumbWidth);
            }

            ImageCodecInfo ici = GetEncoderInfo("image/jpeg");

            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            EncoderParameter myEncoderParameter = new EncoderParameter(Encoder.Quality, long.Parse("90"));

            myEncoderParameters.Param[0] = myEncoderParameter;


            Image.GetThumbnailImageAbort dummyCallBack = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            Image thumbNailImg = fullSizeImg.GetThumbnailImage(thumbWidth, thumbHeight, dummyCallBack, IntPtr.Zero);

            if (_maxThumbWidth >= 150)
            {
                Bitmap objNewBitMap = new Bitmap(thumbWidth, thumbHeight, PixelFormat.Format32bppArgb);

                Graphics objGraphics = Graphics.FromImage(objNewBitMap);

                objGraphics.Clear(Color.Transparent);

                objGraphics.DrawImage(fullSizeImg, new Rectangle(0, 0, thumbWidth, thumbHeight));
                objNewBitMap.Save(strFile_distinct, ici, myEncoderParameters);
                objNewBitMap.Dispose();
            }
            else
            {

                //Save the thumbnail in PNG format. 
                //You may change it to a diff format with the ImageFormat property
                thumbNailImg.Save(strFile_distinct, ici, myEncoderParameters);
                thumbNailImg.Dispose();
            }
            fullSizeImg.Dispose();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mineType"></param>
        /// <returns></returns>
        private ImageCodecInfo GetEncoderInfo(string mineType)
        {

            System.Drawing.Imaging.ImageCodecInfo[] myEncoders =
                System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();

            foreach (System.Drawing.Imaging.ImageCodecInfo myEncoder in myEncoders)
                if (myEncoder.MimeType == mineType)
                    return myEncoder;
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private  bool ThumbnailCallback()
        {
            return false;
        }

    }
}
