using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Tags.Queries.GetTags;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.IntegrationTests.Tags.Queries;

using static Testing;

public class GetTagsTests : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidItemId()
    {
        var query = new GetTagsQuery();
        await FluentActions.Invoking(() => SendAsync(query)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldReturnAllTagsExceptDeleted()
    {
        await RunAsDefaultUserAsync();

        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });
        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New List"
        });

        await AddAsync(new Tag
        {
            ItemId = itemId,
            Name = "test"
        });
        await AddAsync(new Tag
        {
            ItemId = itemId,
            Name = "test 123",
            Deleted = true
        });

        var query = new GetTagsQuery
        {
            ItemId = itemId
        };

        var result = await SendAsync(query);

        result.Should().HaveCount(1);
    }
}
