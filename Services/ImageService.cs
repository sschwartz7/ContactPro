using ContactPro.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContactPro.Services
{
    public class ImageService : IImageService
    {
        private readonly string? _defaultImage = "/img/SharkSillouette.png";
        public string? ConvertByteArrayToFile(byte[]? fileData, string? extension)
        {
            try
            {
                if (fileData == null)
                {
                    return _defaultImage;
                }
                string? imageBase64Data = Convert.ToBase64String(fileData);
                imageBase64Data = string.Format($"data:{extension};base64,{imageBase64Data}");
                
                return imageBase64Data;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<byte[]> ConvertFileToByteArrayAsync(IFormFile? file)
        {
            try
            {
                using MemoryStream memoryStream = new MemoryStream();//using cleans up after itself
                await file!.CopyToAsync(memoryStream);
                byte[] byteFile = memoryStream.ToArray();
                memoryStream.Close();//actively closes memory stream
                return byteFile;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
