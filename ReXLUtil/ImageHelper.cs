using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

public static class ImageHelper
{
    public static async Task<byte[]> ResizeImageToTargetSizeAsync(byte[] inputImageBytes, int targetSizeInKB)
    {
        int targetSizeInBytes = targetSizeInKB * 1024;
        if (inputImageBytes == null || inputImageBytes.Length <= targetSizeInBytes)
            return inputImageBytes;

        using var inputStream = new MemoryStream(inputImageBytes);
        using var image = await Image.LoadAsync<Rgba32>(inputStream);

        int width = image.Width;
        int height = image.Height;

        for (int quality = 90; quality >= 30; quality -= 5)
        {
            using var ms = new MemoryStream();

            var clone = image.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            }));

            await clone.SaveAsJpegAsync(ms, new JpegEncoder
            {
                Quality = quality
            });

            if (ms.Length <= targetSizeInBytes)
                return ms.ToArray();

            // Optional: reduce dimensions slightly each round
            width = (int)(width * 0.9);
            height = (int)(height * 0.9);
        }

        throw new Exception("Unable to compress image to the target size.");
    }
}
