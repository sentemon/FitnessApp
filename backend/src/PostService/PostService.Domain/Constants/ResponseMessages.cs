namespace PostService.Domain.Constants;

public static class ResponseMessages
{
    public const string YouSuccessfullyDeletedPost = "You successfully deleted post.";
    public const string YouSuccessfullyDeletedComment = "You successfully deleted comment.";
    public const string YouSuccessfullyDeletedLike = "You successfully deleted like.";

    public const string UserIdIsNull = "User Id is null.";
    
    public const string PostNotFound = "Post not found.";
    public const string CommentNotFound = "Comment not found.";
    public const string LikeNotFound = "Like not found.";
    public const string UserNotFound = "User not found";
    public const string UserHasNotLikedThisPostYet = "User has not liked this post yet.";
    public const string UserHasAlreadyLikedThisPost = "User has already liked this post.";
    public const string YouDoNotHavePermissionToUpdateThisPost = "You do not have permission to update this post.";
    public const string YouDoNotHavePermissionToDeleteThisPost = "You do not have permission to delete this post.";
    public const string YouDoNotHavePermissionToDeleteThisComment = "You do not have permission to delete this comment.";

    public const string InvalidFileState = "Invalid file state for the given content type.";
}