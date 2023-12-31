﻿using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;
using Todo_App.Domain.ValueObjects;

namespace Todo_App.Application.IntegrationTests.TodoLists.Queries;

using static Testing;

public class GetTodosTests : BaseTestFixture
{
    [Test]
    public async Task ShouldReturnPriorityLevels()
    {
        await RunAsDefaultUserAsync();

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.PriorityLevels.Should().NotBeEmpty();
    }

    [Test]
    public async Task ShouldReturnAllListsAndItems()
    {
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = "Shopping",
            Colour = Colour.Blue,
            Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" }
                    }
        });

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Items.Should().HaveCount(7);
    } 

    [Test]
    public async Task ShouldReturnAllListsWithItemsExceptDeleted()
    {
        string listTitle = "Fruit Shopping";
        await RunAsDefaultUserAsync();

        await AddAsync(new TodoList
        {
            Title = listTitle,
            Colour = Colour.Blue,
            Items =
                    {
                        new TodoItem { Title = "Apples", Done = true, Deleted = false},
                        new TodoItem { Title = "Mango", Done = true, Deleted = true}
                    }
        });

        await AddAsync(new TodoList
        {
            Title = "Grocery Shopping",
            Colour = Colour.Blue,
            Items =
                    {
                        new TodoItem { Title = "Rice", Done = true }
                    },
            Deleted = true
        });

        var query = new GetTodosQuery();

        var result = await SendAsync(query);

        result.Lists.Should().HaveCount(1);
        result.Lists.First().Title.Should().Be(listTitle);
        result.Lists.First().Items.Should().HaveCount(1);
    }

    [Test]
    public async Task ShouldAcceptAnonymousUser()
    {
        var query = new GetTodosQuery();

        var action = () => SendAsync(query);

        await action.Should().NotThrowAsync<UnauthorizedAccessException>();
    }
}
