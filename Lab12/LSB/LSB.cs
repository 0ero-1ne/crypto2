using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;

namespace Lab12.LSB
{
    public static class LSB
    {
        public static void Encrypt(string filename, string message, bool even = true)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var messageBits = string.Join("", messageBytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));

            var (pixels, width, height) = ImageProcess.ImageProcess.GetImageBytes(filename);

            if (pixels.Length * 3 / 2 < messageBits.Length * 8)
            {
                throw new Exception("Too small image for your message");
            }

            var counter = 0;

            for (int i = even ? 0 : 1; i < pixels.Length; i += 2)
            {
                pixels[i].R = GetEncryptedPixelChannelByte(pixels[i].R, messageBits[counter++]);
                if (counter == messageBytes.Length * 8) break;
                pixels[i].G = GetEncryptedPixelChannelByte(pixels[i].G, messageBits[counter++]);
                if (counter == messageBytes.Length * 8) break;
                pixels[i].B = GetEncryptedPixelChannelByte(pixels[i].B, messageBits[counter++]);
                if (counter == messageBytes.Length * 8) break;
            }

            var fileInfo = new FileInfo(filename);
            var filenameWOExtension = Path.GetFileNameWithoutExtension(filename);
            var newFilename = fileInfo.DirectoryName + "\\" + filenameWOExtension + "-encrypted.png";

            ExifProfile profile = new();
            profile.SetValue(ExifTag.ImageNumber, (ushort)messageBytes.Length);
            profile.SetValue(ExifTag.Copyright, even.ToString());

            ImageProcess.ImageProcess.SaveImage(pixels, width, height, newFilename, profile);
        }

        public static string Decrypt(string filename)
        {
            var (pixels, _, _) = ImageProcess.ImageProcess.GetImageBytes(filename);
            var profile = ImageProcess.ImageProcess.GetExifProfile(filename);

            IExifValue<uint>? messageLengthValue = null;
            IExifValue<string>? isEvenValue = null;
            profile?.TryGetValue(ExifTag.ImageNumber, out messageLengthValue);
            profile?.TryGetValue(ExifTag.Copyright, out isEvenValue);

            uint messageLength = messageLengthValue == null ? 0 : (uint)messageLengthValue.GetValue()!;
            bool isEven = isEvenValue == null || (string)isEvenValue!.GetValue()! == "True";

            if (messageLength == 0) return "There is no any message";

            var counter = 0;
            var binaryData = "";
            

            byte[] bytes = new byte[messageLength];
            
            for (int i = isEven ? 0 : 1; i < pixels.Length; i += 2) {
                binaryData += pixels[i].R & 1;
                if (++counter == messageLength * 8) break;
                binaryData += pixels[i].G & 1;
                if (++counter == messageLength * 8) break;
                binaryData += pixels[i].B & 1;
                if (++counter == messageLength * 8) break;
            }

            for(int i = 0; i < messageLength; i++)
            {
                bytes[i] = Convert.ToByte(binaryData.Substring(8 * i, 8), 2);
            }

            return Encoding.UTF8.GetString(bytes);
        }

        private static byte GetEncryptedPixelChannelByte(byte pixelChannelByte, char messageBit)
        {
            int lastChannelBit = pixelChannelByte & 1;

            if (lastChannelBit.ToString() == messageBit.ToString()) {
                return pixelChannelByte;
            }

            return (byte)(pixelChannelByte ^ 1);
        }
    }
}