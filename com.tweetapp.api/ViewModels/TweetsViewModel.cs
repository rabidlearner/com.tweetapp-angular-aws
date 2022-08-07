namespace com.tweetapp.api.ViewModels
{
    public class TweetsViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public List<RepliesViewModel> RepliesViewModels { get; set; }

    }
}
