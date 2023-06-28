using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Tags.Queries.SumaryTags;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.IntegrationTests.Tags.Queries;

using static Testing;
public class SummaryTagsTests : BaseTestFixture
{

    public async Task SetupTags()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });
        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New item"
        });
        var itemId2 = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New item 2"
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
        await AddAsync(new Tag
        {
            ItemId = itemId2,
            Name = "test"
        });
    }

    [Test]
    public async Task ShouldReturnSummaryTags()
    {
        await SetupTags();

        var result = await SendAsync(new SummaryTagsQuery());

        result.Should().HaveCount(2);
        var tag = result.FirstOrDefault(e => e.Name == "test");
        tag.Should().NotBeNull();
        tag.Count.Should().Be(2); 
    }


}
