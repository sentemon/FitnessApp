using PostService.Api.GraphQL.Mutation;
using PostService.Api.GraphQL.Query;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddMutationType<Mutation>()
    .AddQueryType<Query>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGraphQL();

app.Run();