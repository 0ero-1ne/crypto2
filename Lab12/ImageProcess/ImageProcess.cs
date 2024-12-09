using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.PixelFormats;

namespace Lab12.ImageProcess
{
    public class ImageProcess
    {
        public static (Rgb24[], int, int) GetImageBytes(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("No such file " + filename + "\nCheck the file path and try again");
            }

            using Image<Rgb24> image = Image.Load<Rgb24>(filename);
            var pixels = new Rgb24[image.Width * image.Height];
            image.CopyPixelDataTo(pixels);

            return (pixels, image.Width, image.Height);
        }

        // channel 0 - red, 1 - green, 2 - blue
        public static void GetLSBImageChannel(string filename, int channel)
        {
            var (pixels, width, height) = GetImageBytes(filename);

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = GetPixelByChannel(pixels[i], channel);
            }

            using Image image = Image.LoadPixelData<Rgb24>(pixels, width, height);

            var fileInfo = new FileInfo(filename);
            var filenameWOExtension = Path.GetFileNameWithoutExtension(filename);
            var newFilename = fileInfo.DirectoryName + "\\" + filenameWOExtension;
            newFilename += channel == 0 ? "-lsb_red.png" : channel == 1 ? "-lsb_green.png" : "-lsb_blue.png";

            if (File.Exists(newFilename)) File.Delete(newFilename);

            image.SaveAsPng(newFilename);
        }

        public static void GetLSBImage(string filename)
        {
            var (pixels, width, height) = GetImageBytes(filename);

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].R = (pixels[i].R & 1) == 1 ? (byte)255 : (byte)0;
                pixels[i].G = (pixels[i].G & 1) == 1 ? (byte)255 : (byte)0;
                pixels[i].B = (pixels[i].B & 1) == 1 ? (byte)255 : (byte)0;
            }

            using Image image = Image.LoadPixelData<Rgb24>(pixels, width, height);

            var fileInfo = new FileInfo(filename);
            var filenameWOExtension = Path.GetFileNameWithoutExtension(filename);
            var newFilename = fileInfo.DirectoryName + "\\" + filenameWOExtension + "-full_lsb.png";

            if (File.Exists(newFilename)) File.Delete(newFilename);

            image.SaveAsPng(newFilename);
        }

        public static ExifProfile? GetExifProfile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new Exception("No such file " + filename + "\nCheck the file path and try again");
            }

            using Image<Rgb24> image = Image.Load<Rgb24>(filename);
            return image.Metadata.ExifProfile;
        }

        public static void SaveImage(Rgb24[] pixels, int width, int height, string filename, ExifProfile profile)
        {
            if (File.Exists(filename)) File.Delete(filename);

            using Image newImage = Image.LoadPixelData<Rgb24>(pixels, width, height);
            newImage.Metadata.ExifProfile = profile;
            newImage.SaveAsPng(filename); 
        }

        private static Rgb24 GetPixelByChannel(Rgb24 pixel, int channel)
        {   
            if (channel == 0)
            {
                return (pixel.R & 1) == 1 ? new Rgb24(0, 0, 0) : new Rgb24(255, 255, 255);
            }
            
            if (channel == 1)
            {
                return (pixel.G & 1) == 1 ? new Rgb24(0, 0, 0) : new Rgb24(255, 255, 255);
            }

            if (channel == 2)
            {
                return (pixel.B & 1) == 1 ? new Rgb24(0, 0, 0) : new Rgb24(255, 255, 255);
            }

            return pixel;
        }
    }
}