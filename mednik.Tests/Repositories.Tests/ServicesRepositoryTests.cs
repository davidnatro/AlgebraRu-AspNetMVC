using mednik.Controllers;
using mednik.Data;
using mednik.Data.Repositories.Services;
using mednik.Models;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace mednik.Tests.Repositories.Tests;

public class ServicesRepositoryTests
{
    [Fact]
    public async Task GetAllAsyncTest()
    {
        var mockRepository = new Mock<IServicesRepository>();
        mockRepository.Setup(method => method.GetAllAsync()).ReturnsAsync(GetAllAsync);

        var controller = new ServicesController(mockRepository.Object);
    }

    private static IEnumerable<Services> GetAllAsync()
        => new List<Services>()
        {
            new Services() {Id = Guid.NewGuid(), Name = "Name1", Link = "Link1"},
            new Services() {Id = Guid.NewGuid(), Name = "Name2", Link = "Link2"},
            new Services() {Id = Guid.NewGuid(), Name = "Name3", Link = "Link3"},
            new Services() {Id = Guid.NewGuid(), Name = "Name4", Link = "Link4"},
            new Services() {Id = Guid.NewGuid(), Name = "Name5", Link = "Link5"},
        };
}