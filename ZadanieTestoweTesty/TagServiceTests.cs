using AutoMapper;
using CoreEx.Abstractions;
using MediportaZadanieRekrutacyjne.Data;
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieTestoweTesty
{
    public class TagServiceTests
    {
        [Fact]
        public async Task CreateTag_ValidTag_AddsTagToDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var mockStackExchangeService = new Mock<IStackExchangeService>();
           
            // Create a mock for IMapper
            var mockMapper = new Mock<IMapper>();

          
          
            using (var context = new DataDbContext(options))
            {

                var service = new TagRepo(context, mockStackExchangeService.Object, mockMapper.Object);
                var tag = new TagItem { Name = "TestTag", Share = 50 };

                // Act
                await service.CreateTag(tag);

                // Assert
                var createdTag = await context.Tags.FirstOrDefaultAsync(t => t.Name == "TestTag");
                Assert.NotNull(createdTag);
                Assert.Equal("TestTag", createdTag.Name);
                Assert.Equal(50, createdTag.Share);
            }
        }

        [Fact]
        public async Task GetTagsAsync_ReturnsListOfTags()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var mockStackExchangeService = new Mock<IStackExchangeService>();

            // Create a mock for IMapper
            var mockMapper = new Mock<IMapper>();

            using (var context = new DataDbContext(options))
            {
                var service = new TagRepo(context, mockStackExchangeService.Object,mockMapper.Object);
                await context.Tags.AddRangeAsync(new List<TagItem>
            {
                new TagItem { Name = "Tag1", Share = 20 },
                new TagItem { Name = "Tag2", Share = 30 },
                new TagItem { Name = "Tag3", Share = 40 }
            });
                await context.SaveChangesAsync();

                // Act
                var tags = await service.GetTagsAsync();

                // Assert
                Assert.NotNull(tags);
                Assert.Equal(3, tags.Count);
                Assert.Contains(tags, t => t.Name == "Tag1");
                Assert.Contains(tags, t => t.Name == "Tag2");
                Assert.Contains(tags, t => t.Name == "Tag3");
            }
        }

    }
}
