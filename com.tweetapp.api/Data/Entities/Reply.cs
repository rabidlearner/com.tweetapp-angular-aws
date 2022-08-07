namespace com.tweetapp.api.Data.Entities
{
    public class Reply
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
        public string username{ get; set; }        
        public string Message { get; set; }
    }
}
