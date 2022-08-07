using com.tweetapp.api.Models;

namespace com.tweetapp.api.Repo.IRepo
{
    public interface IRepliesRepo
    {
        Task<bool> PostReply(Reply reply);
        Task<List<Reply>> GetReplies(int TewwtId);
        Task DeleteReplies(int id);
    }
}
