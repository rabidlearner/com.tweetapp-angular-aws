namespace com.tweetapp.api.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public Tweet Tweet { get; set; }
        public string Username { get; set; }        
        public string Message { get; set; }
    }
}
