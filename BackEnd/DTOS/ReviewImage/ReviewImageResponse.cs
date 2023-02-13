namespace BackEnd.DTOS.ImageReview
{
    public class ReviewImageResponse
    {
        public string ID { get; set; }
        public string Image { get; set; }
        static public ReviewImageResponse FromReviewImage(Models.ReviewImage reviewImage)
        {
            return new ReviewImageResponse
            {
                ID = reviewImage.ID,
                Image = !string.IsNullOrEmpty(reviewImage.Image) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "images/" + reviewImage.Image : "" ,

            };
        }
    }
}
