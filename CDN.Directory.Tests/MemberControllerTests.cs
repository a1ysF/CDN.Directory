using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CDN.Directory.API.Controllers;
using CDN.Directory.Core.DTOs;
using CDN.Directory.Core.Entities;
using CDN.Directory.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CDN.Directory.Tests.Controllers
{
    public class MemberControllerTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        private IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Member, MemberDto>();
                cfg.CreateMap<CreateMemberDto, Member>();
                cfg.CreateMap<UpdateMemberDto, Member>();
            });
            return config.CreateMapper();
        }

        [Fact]
        public async Task CreateMember_AddsMember()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var controller = new MemberController(context, mapper);

            var dto = new CreateMemberDto
            {
                Username = "TestUser",
                Email = "test@example.com",
                PhoneNumber = "123456789"
            };

            var result = await controller.CreateMember(dto);

            var members = context.Members.ToList();
            Assert.Single(members);
            Assert.Equal("TestUser", members[0].Username);
        }

        [Fact]
        public async Task GetMembers_ReturnsList()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            context.Members.Add(new Member { Username = "User1", Email = "user1@test.com", PhoneNumber = "111" });
            context.Members.Add(new Member { Username = "User2", Email = "user2@test.com", PhoneNumber = "222" });
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var result = await controller.GetMembers();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var members = Assert.IsType<List<MemberDto>>(okResult.Value);
            Assert.Equal(2, members.Count);
        }

        [Fact]
        public async Task GetMember_ReturnsCorrectMember()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var member = new Member { Username = "User1", Email = "user1@test.com", PhoneNumber = "111" };
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var result = await controller.GetMember(member.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<MemberDto>(okResult.Value);
            Assert.Equal("User1", value.Username);
        }

        [Fact]
        public async Task GetMember_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var controller = new MemberController(context, mapper);

            var result = await controller.GetMember(999);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task UpdateMember_UpdatesData()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var member = new Member { Username = "OldName", Email = "old@example.com", PhoneNumber = "000" };
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var dto = new UpdateMemberDto { Username = "NewName", Email = "new@example.com", PhoneNumber = "111" };
            var result = await controller.UpdateMember(member.Id, dto);

            var updated = await context.Members.FindAsync(member.Id);
            Assert.Equal("NewName", updated.Username);
            Assert.Equal("new@example.com", updated.Email);
            Assert.Equal("111", updated.PhoneNumber);
        }

        [Fact]
        public async Task UpdateMember_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var controller = new MemberController(context, mapper);

            var dto = new UpdateMemberDto { Username = "X", Email = "x@x.com", PhoneNumber = "000" };
            var result = await controller.UpdateMember(999, dto);

            var actionResult = Assert.IsType<ActionResult<MemberDto>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task DeleteMember_RemovesMember()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var member = new Member { Username = "UserToDelete", Email = "delete@test.com", PhoneNumber = "999" };
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var result = await controller.DeleteMember(member.Id);

            Assert.Null(await context.Members.FindAsync(member.Id));
        }

        [Fact]
        public async Task DeleteMember_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var controller = new MemberController(context, mapper);

            var result = await controller.DeleteMember(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ArchiveMember_SetsIsArchived()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var member = new Member { Username = "UserToArchive", Email = "archive@test.com", PhoneNumber = "888", IsArchived = false };
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            await controller.ArchiveMember(member.Id);

            var archived = await context.Members.FindAsync(member.Id);
            Assert.True(archived.IsArchived);
        }

        [Fact]
        public async Task ArchiveMember_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var controller = new MemberController(context, mapper);

            var result = await controller.ArchiveMember(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UnarchiveMember_SetsIsArchivedFalse()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var member = new Member { Username = "ArchivedUser", Email = "archived@test.com", PhoneNumber = "777", IsArchived = true };
            context.Members.Add(member);
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            await controller.UnarchiveMember(member.Id);

            var updated = await context.Members.FindAsync(member.Id);
            Assert.False(updated.IsArchived);
        }

        [Fact]
        public async Task UnarchiveMember_ReturnsNotFound()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            var controller = new MemberController(context, mapper);

            var result = await controller.UnarchiveMember(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task SearchByUsername_ReturnsMatchingResults()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            context.Members.Add(new Member { Username = "Alice", Email = "alice@example.com", PhoneNumber = "1234" });
            context.Members.Add(new Member { Username = "Bob", Email = "bob@example.com", PhoneNumber = "5678" });
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var result = await controller.SearchByUsername("Ali");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var members = Assert.IsType<List<MemberDto>>(okResult.Value);
            Assert.Single(members);
            Assert.Contains(members, m => m.Username.Contains("Ali"));
        }

        [Fact]
        public async Task SearchByEmail_ReturnsMatchingResults()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            context.Members.Add(new Member { Username = "Charlie", Email = "charlie@example.com", PhoneNumber = "3333" });
            context.Members.Add(new Member { Username = "David", Email = "david@example.com", PhoneNumber = "4444" });
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var result = await controller.SearchByEmail("charlie");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var members = Assert.IsType<List<MemberDto>>(okResult.Value);
            Assert.Single(members);
            Assert.Equal("charlie@example.com", members[0].Email);
        }

        [Fact]
        public async Task GetAllMembers_ReturnsAll()
        {
            var context = GetInMemoryDbContext();
            var mapper = GetMapper();
            context.Members.Add(new Member { Username = "User1", Email = "user1@test.com", PhoneNumber = "1111" });
            context.Members.Add(new Member { Username = "User2", Email = "user2@test.com", PhoneNumber = "2222" });
            await context.SaveChangesAsync();

            var controller = new MemberController(context, mapper);
            var result = await controller.GetMembers();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var members = Assert.IsType<List<MemberDto>>(okResult.Value);
            Assert.Equal(2, members.Count);
        }
    }
}
