using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Musference.Logic
{
    public interface ICloudinaryhandler
    {
        public Task<string> GetFileStringAndUpload(IFormFile file, string type = "");
    }
    public class Cloudinaryhandler : ICloudinaryhandler
    {
        public readonly Cloudinary _cloudinaryh;

        public Cloudinaryhandler(Cloudinary cloudinary)
        {
            _cloudinaryh = cloudinary;
        }

        public async Task<string> GetFileStringAndUpload(IFormFile file, string type = "")
        {
            if (file == null)
                return string.Empty;
            RawUploadResult uploadResult;

            if (type == "image")
            {
                uploadResult = await _cloudinaryh.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                }).ConfigureAwait(false);
            }
            else
            {
                uploadResult = await _cloudinaryh.UploadAsync("video", null,
                   new FileDescription(file.FileName, file.OpenReadStream())).ConfigureAwait(false);

            }
            return uploadResult.Url.ToString();
        }
    }
}
