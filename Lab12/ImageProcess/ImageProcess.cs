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
    }
}