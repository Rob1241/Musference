namespace Musference.Models.DTOs
{
    public class AddTrackDto
    {
        public string Title { get; set; }
        public string AudioFile { get; set; }
        public string LogoFile { get; set; } = "https://res.cloudinary.com/da1tlcmhr/image/upload/v1675790978/musference_cloudinary/image1_ru4u83.jpg";
    }
}
