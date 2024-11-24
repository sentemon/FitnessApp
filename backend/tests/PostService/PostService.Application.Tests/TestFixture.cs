using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PostService.Application.Commands.AddComment;
using PostService.Application.Commands.AddLike;
using PostService.Application.Commands.AddPost;
using PostService.Application.Commands.DeleteComment;
using PostService.Application.Commands.DeleteLike;
using PostService.Application.Commands.DeletePost;
using PostService.Application.Commands.UpdatePost;
using PostService.Application.Queries.GetAllComments;
using PostService.Application.Queries.GetAllLikes;
using PostService.Application.Queries.GetAllPosts;
using PostService.Application.Queries.GetPost;
using PostService.Domain.Entities;
using PostService.Persistence;
using Testcontainers.PostgreSql;

namespace PostService.Application.Tests;

public class TestFixture
{
    private readonly TestStartup _testStartup = new();
    public readonly PostDbContext PostDbContextFixture;
    
    public readonly AddPostCommandHandler AddPostCommandHandler;
    public readonly UpdatePostCommandHandler UpdatePostCommandHandler;
    public readonly DeletePostCommandHandler DeletePostCommandHandler;
    public readonly AddCommentCommandHandler AddCommentCommandHandler;
    public readonly DeleteCommentCommandHandler DeleteCommentCommandHandler;
    public readonly AddLikeCommandHandler AddLikeCommandHandler;
    public readonly DeleteLikeCommandHandler DeleteLikeCommandHandler;

    public readonly GetPostQueryHandler GetPostQueryHandler;
    public readonly GetAllPostsQueryHandler GetAllPostsQueryHandler;
    public readonly GetAllCommentsQueryHandler GetAllCommentsQueryHandler;
    public readonly GetAllLikesQueryHandler GetAllLikesQueryHandler;
    
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    public User ExistingUser { get; }

    public TestFixture()
    {
        _postgreSqlContainer.StartAsync().Wait();

        var connectionString = _postgreSqlContainer.GetConnectionString();
        var serviceProvider = _testStartup.Initialize(connectionString);

        PostDbContextFixture = serviceProvider.GetRequiredService<PostDbContext>();
        
        ApplyMigrations();

        AddPostCommandHandler = serviceProvider.GetRequiredService<AddPostCommandHandler>();
        UpdatePostCommandHandler = serviceProvider.GetRequiredService<UpdatePostCommandHandler>();
        DeletePostCommandHandler = serviceProvider.GetRequiredService<DeletePostCommandHandler>();
        AddCommentCommandHandler = serviceProvider.GetRequiredService<AddCommentCommandHandler>();
        DeleteCommentCommandHandler = serviceProvider.GetRequiredService<DeleteCommentCommandHandler>();
        AddLikeCommandHandler = serviceProvider.GetRequiredService<AddLikeCommandHandler>();
        DeleteLikeCommandHandler = serviceProvider.GetRequiredService<DeleteLikeCommandHandler>();

        GetPostQueryHandler = serviceProvider.GetRequiredService<GetPostQueryHandler>();
        GetAllPostsQueryHandler = serviceProvider.GetRequiredService<GetAllPostsQueryHandler>();
        GetAllCommentsQueryHandler = serviceProvider.GetRequiredService<GetAllCommentsQueryHandler>();
        GetAllLikesQueryHandler = serviceProvider.GetRequiredService<GetAllLikesQueryHandler>();
        
        ExistingUser = CreateExistingUser();
    }
    
    private void ApplyMigrations()
    {
        using (var scope = PostDbContextFixture.Database.BeginTransaction())
        {
            try
            {
                PostDbContextFixture.Database.Migrate();
                scope.Commit();
            }
            catch (Exception)
            {
                scope.Rollback();
                throw;
            }
        }
    }

    private User CreateExistingUser()
    {
        var user = new User(
            Guid.NewGuid(),
            "First Name",
            "Last Name",
            "Username",
            "");
        
        PostDbContextFixture.Users.Add(user);
        PostDbContextFixture.SaveChanges();

        return user;
    }
}
