namespace com.tweetapp.api.Data.Entities
{
    public class Tweet
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
    }
}
