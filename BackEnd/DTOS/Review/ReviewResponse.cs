namespace BackEnd.DTOS.Review
{
    public class ReviewResponse
    {
        public string ID { get; set; }
        public string Data { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string? Video { get; set; }
        public int Score { get; set; }
        public string ScoreText { get; set; }
        public string CustomerID { get; set; }
        public string ProductListID { get; set; }
        static public ReviewResponse FromReview(Models.Review review)
        {
            return new ReviewResponse
            {
                ID = review.ID,
                Video = !string.IsNullOrEmpty(review.Video) ? "http://10.103.0.15/cs63/s09/reactJs/backEnd/" + "vedio/" + review.Video : "",
                Score = review.Score,
                ScoreText = review.ScoreText,
                CustomerID = review.CustomerID,
                ProductListID = review.ProductListID,
                Data = review.Data,
                Created = review.Created,
            };
        }
    }
}
