namespace BackEnd.DTOS.Review
{
    public class ReviewRequest
    {
        public string Data { get; set; }
        public IFormFileCollection? VedioFiles { get; set; }
        public string ProductListID { get; set; }
        public int Score { get; set; }
        public string ScoreText { get; set; }
        public string CustomerID { get; set; }
        public IFormFileCollection? ImageFiles { get; set; }
    }
}
