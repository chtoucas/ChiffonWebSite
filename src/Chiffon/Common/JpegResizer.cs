namespace Chiffon.Common
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using Narvalo;

    public static class JpegResizer
    {
        static ImageCodecInfo Encoder_;

        public static void Resize(string inFile, string outFile, double maxWidth, double maxHeight, long level)
        {
            Requires.NotNullOrEmpty(inFile, "inFile");
            Requires.NotNullOrEmpty(outFile, "outFile");

            using (var outStream = new FileStream(outFile, FileMode.Create, FileAccess.Write, FileShare.None)) {
                // Load via stream rather than Image.FromFile to release the file handle immediately.
                using (var stream = new FileStream(inFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    using (var inImage = Image.FromStream(stream)) {
                        Resize(inImage, outStream, maxWidth, maxHeight, level);
                    }
                }
            }
        }

        public static void Resize(Image inImage, string outFile, double maxWidth, double maxHeight, long level)
        {
            Requires.NotNull(inImage, "inImage");
            Requires.NotNullOrEmpty(outFile, "outFile");

            using (var outStream = new FileStream(outFile, FileMode.Create, FileAccess.Write, FileShare.None)) {
                Resize(inImage, outStream, maxWidth, maxHeight, level);
            }
        }

        public static void Resize(Image inImage, Stream outStream, double maxWidth, double maxHeight, long level)
        {
            Requires.NotNull(inImage, "inImage");
            Requires.NotNull(outStream, "outStream");

            // Load via stream rather than Image.FromFile to release the file handle immediately.
            //using (var stream = new FileStream(inFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
            //    using (var inImage = Image.FromStream(stream)) {
            double imageWidth = inImage.Width;
            double imageHeight = inImage.Height;

            double aspectRatio = imageWidth / imageHeight;
            double boxRatio = maxWidth / maxHeight;
            double scaleFactor;

            if (boxRatio > aspectRatio) {
                // Use height, since that is the most restrictive dimension of box.
                scaleFactor = maxHeight / imageHeight;
            }
            else {
                scaleFactor = maxWidth / imageWidth;
            }

            double width = imageWidth * scaleFactor;
            double height = imageHeight * scaleFactor;

            using (var bitmap = new Bitmap((int)width, (int)height)) {
                using (var gx = Graphics.FromImage(bitmap)) {
                    gx.SmoothingMode = SmoothingMode.HighQuality;
                    gx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gx.CompositingQuality = CompositingQuality.HighQuality;
                    gx.DrawImage(inImage, 0, 0, bitmap.Width, bitmap.Height);

                    //if (inImage.RawFormat.Guid == ImageFormat.Jpeg.Guid) {
                    if (Encoder_ == null) {
                        ImageCodecInfo[] ici = ImageCodecInfo.GetImageDecoders();

                        foreach (var info in ici) {
                            if (info.FormatID == ImageFormat.Jpeg.Guid) {
                                Encoder_ = info;
                                break;
                            }
                        }
                    }

                    if (Encoder_ != null) {
                        using (EncoderParameters ep = new EncoderParameters(1)) {
                            ep.Param[0] = new EncoderParameter(Encoder.Quality, level);
                            bitmap.Save(outStream, Encoder_, ep);
                        }
                    }
                    else {
                        bitmap.Save(outStream, inImage.RawFormat);
                    }
                }
            }
            //    }
            //}
        }
    }
}
