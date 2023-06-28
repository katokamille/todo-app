using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Tags.Commands.CreateTags;
using Todo_App.Application.TodoItems.Commands.CreateTodoItem;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.IntegrationTests.Tags.Commands;

using static Testing;
public class CreateTagTests : BaseTestFixture
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

        return itemId;
    }

    [Test]
    public async Task ShouldValidateEmptyTag()
    {
        int itemId = await SetupTags();
        var command = new CreateTagCommand
        {
            ItemId = itemId,
            Name = ""
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldRequireValidItemId()
    {
        int itemId = await SetupTags();
        var command = new CreateTagCommand
        {
            ItemId = itemId - 1,
            Name = "sample"
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldValidateIfExists()
    {
        int itemId = await SetupTags();
        var command = new CreateTagCommand
        {
            ItemId = itemId,
            Name = "test"
        };

        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<AlreadyExistsException>();
    }

    [Test]
    public async Task ShouldCreateTag()
    {
        var userId = await RunAsDefaultUserAsync();
        int itemId = await SetupTags();
        var command = new CreateTagCommand
        {
            ItemId = itemId,
            Name = "sample"
        };

        var tagId = await SendAsync(command);

        var tag = await FindAsync<Tag>(tagId);

        tag.Should().NotBeNull();
        tag.ItemId.Should().Be(itemId);
        tag.Name.Should().Be(command.Name);
        tag.CreatedBy.Should().Be(userId);
        tag.LastModifiedBy.Should().Be(userId);
    }
}
