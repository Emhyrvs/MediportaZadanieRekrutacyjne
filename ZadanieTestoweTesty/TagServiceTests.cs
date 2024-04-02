using AutoMapper;
using CoreEx.Abstractions;
using MediportaZadanieRekrutacyjne.Data;
using MediportaZadanieRekrutacyjne.Models;
using MediPortaZadanieTestowe.Controllers;
using MediPortaZadanieTestowe.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZadanieTestoweTesty
{
    public class TagServiceTests
    {
        
       
        [Fact]
        public async Task CreateTag_ValidTag_AddsTagToDatabase()
        {

            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var mockStackExchangeService = new Mock<IStackExchangeService>();


            var mockMapper = new Mock<IMapper>();



            using (var context = new DataDbContext(options))
            {

                var service = new TagRepo(context, mockStackExchangeService.Object, mockMapper.Object);
                var tag = new TagItem { Name = "TestTag", Share = 50 };


                await service.CreateTag(tag);


                var createdTag = await context.Tags.FirstOrDefaultAsync(t => t.Name == "TestTag");
                Assert.NotNull(createdTag);
                Assert.Equal("TestTag", createdTag.Name);
                Assert.Equal(50, createdTag.Share);
            }
        }

        [Fact]
        public async Task GetTagsAsync_ReturnsListOfTags()
        {

            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            var mockStackExchangeService = new Mock<IStackExchangeService>();


            var mockMapper = new Mock<IMapper>();

            using (var context = new DataDbContext(options))
            {
                var service = new TagRepo(context, mockStackExchangeService.Object, mockMapper.Object);
                await context.Tags.AddRangeAsync(new List<TagItem>
            {
                new TagItem { Name = "Tag1", Share = 20 },
                new TagItem { Name = "Tag2", Share = 30 },
                new TagItem { Name = "Tag3", Share = 40 }
            });
                await context.SaveChangesAsync();


                var tags = await service.GetTagsAsync();


                Assert.NotNull(tags);
                Assert.Equal(3, tags.Count);
                Assert.Contains(tags, t => t.Name == "Tag1");
                Assert.Contains(tags, t => t.Name == "Tag2");
                Assert.Contains(tags, t => t.Name == "Tag3");
            }
        }
        [Fact]
        public async Task GetTagsFromService_ReturnsListOfTagsFromService()
        {

            var mockLogger = new Mock<ILogger>();


            var mockMapper = new Mock<IMapper>();



            var service = new StackExchangeService(mockMapper.Object, mockLogger.Object);

            var Tags = await service.GetAllTagsAsync(1, 10);
            Assert.NotNull(Tags);
            Assert.Equal(10, Tags.Count);





        }

        [Fact]
        public async Task GetTagsFromDatabase_ShouldReturnTags()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase2")
                .Options;
            var mockStackExchangeService = new Mock<IStackExchangeService>();
            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger>();

            // Arrange
            using (var context = new DataDbContext(options))
            {
                var tags = new List<TagItem>
        {
            new TagItem { Id=1, Name = "JavaScript", Count =1000 },
            new TagItem { Id=2, Name = "C#", Count = 1000 }
        };
                context.Tags.AddRange(tags);
                context.SaveChanges();
            }

            using (var context = new DataDbContext(options))
            {
                var TagsRepo = new TagRepo(context, mockStackExchangeService.Object, mockMapper.Object);
                var TagController = new TagsController(mockStackExchangeService.Object, TagsRepo, mockLogger.Object);

                // Act
                var result = await TagController.GetTagsFromDatabase();

                // Assert
                // Assert
                var actionResult = Assert.IsType<ActionResult<List<TagItem>>>(result);
                var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
                var tags = Assert.IsAssignableFrom<IEnumerable<TagItem>>(okObjectResult.Value);
                Assert.Equal(2, tags.Count());
            }
        }




    }
}
