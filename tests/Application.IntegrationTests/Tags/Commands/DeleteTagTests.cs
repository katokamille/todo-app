using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Tags.Commands.CreateTags;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.IntegrationTests.Tags.Commands;

using static Testing;
public class DeleteTagTests : BaseTestFixture
{
    public async Task<int> SetupTags()
    {
        var listId = await SendAsync(new CreateTodoListCommand
        {
            Title = "New List"
        });
        var itemId = await SendAsync(new CreateTodoItemCommand
        {
            ListId = listId,
            Title = "New List"
        });

        return itemId;
    }

    [Test]
    public async Task ShouldValidateEmptyTag()
    {
        int itemId = await SetupTags();

        var tagId = await SendAsync(new CreateTagCommand
        {
            ItemId = itemId,
            Name = "sample"
        });

        var command = new DeleteTagCommand(tagId - 1);

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTag()
    {
        int itemId = await SetupTags();

        var tagId = await SendAsync(new CreateTagCommand
        {
            ItemId = itemId,
            Name = "sample"
        });

        var command = new DeleteTagCommand(tagId);
        await SendAsync(command);

        var tag = await FindAsync<Tag>(tagId);

        tag.Deleted.Should().BeTrue();
    }
}
