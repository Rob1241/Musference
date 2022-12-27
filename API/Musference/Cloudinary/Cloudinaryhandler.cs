using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Musference.Cloudinary
{
    public class Cloudinaryhandler
    {
        private readonly CloudinaryDotNet.Cloudinary _cloudinary;

        public Cloudinaryhandler(CloudinaryDotNet.Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<string> GetFileStringAndUpload(IFormFile file, string type = "")
        {
            if (file == null)
                return string.Empty;
            RawUploadResult uploadResult;

            if (type == "image")
            {
                uploadResult = await _cloudinary.UploadAsync(new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                }).ConfigureAwait(false);
            }
            else
            {
                uploadResult = await _cloudinary.UploadAsync("video", null,
                   new FileDescription(file.FileName, file.OpenReadStream())).ConfigureAwait(false);

            }
            return uploadResult.Url.ToString();
        }
    }
}
