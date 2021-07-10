using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.DAL.App;
using DAL.App.DTO;
using DAL.App.EF;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Controllers;
using WebApp.ViewModels.Test;
using Xunit;
using Xunit.Abstractions;
using Event = DAL.App.DTO.Event;

namespace TestProject1.UnitTests
{
    public class TestControllerUnitTests
    {
        private readonly TestController _testController;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly AppDbContext _ctx;
        private DbContextOptionsBuilder<AppDbContext> optionBuilder;


        public TestControllerUnitTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            // set up db context for testing - using InMemory db engine
            optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // provide new random database name here
            // or parallel test methods will conflict each other
            optionBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _ctx = new AppDbContext(optionBuilder.Options);
            _ctx.Database.EnsureDeleted();
            _ctx.Database.EnsureCreated();

            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<TestController>();

            _testController = new TestController(logger, _ctx);
        }

        public IAppUnitOfWork GetDLL()
        {
            var context = new AppDbContext(optionBuilder.Options);


            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DAL.App.DTO.MappingProfiles.AutoMapperProfile>();
            });
            var mapper = mockMapper.CreateMapper();
            var uow = new AppUnitOfWork(context, mapper);
            return new AppUnitOfWork(context, mapper);
            // return new AppDLL(uow, mapper);
        }



        [Fact]
        public async Task Action_Test__Returns_ViewModel()
        {

            // ACT
            var result = await _testController.Test();

            // ASSERT
            Assert.NotNull(result); // poleks null
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.NotNull(viewResult); //viewmodel poleks null
            var vm = viewResult!.Model;
            Assert.IsType<TestViewModel>(vm);
            var testVm = vm as TestViewModel;
            Assert.NotNull(testVm!.Events);
            // for debugging
            Assert.Equal(0, testVm.Events.Count);
        }

        [Fact]
        public async Task Action_Test__Returns_ViewModel_WithData()
        {
            // ARRANGE
            await SeedData();

            // ACT
            var result = await _testController.Test();

            // ASSERT
            var testVm = (result as ViewResult)?.Model as TestViewModel;
            Assert.NotNull(testVm);
            Assert.Equal(2, testVm!.Events.Count);
            Assert.Equal("Tartu", testVm.Events.First()!.Location);
            Assert.Equal("Proov 0", testVm.Events.First()!.EventName);
        }

        [Fact]
        public async Task Action_Test__Returns_ViewModel_WithNoData__Fails_With_Exception()
        {

            // ACT
            var result = await _testController.Test();

            // ASSERT
            var testVm = (result as ViewResult)?.Model as TestViewModel;
            Assert.NotNull(testVm);
            Assert.ThrowsAny<Exception>(() => testVm!.Events.First());
        }


        [Theory]
        //[InlineData(5)]
        [ClassData(typeof(CountGenerator))]
        public async Task Action_Test__Returns_ViewModel_WithData_Fluent(int count)
        {
            // ARRANGE
            await SeedData(count);

            // ACT
            var result = await _testController.Test();

            // ASSERT
            var testVm = (result as ViewResult)?.Model as TestViewModel;
            testVm.Should().NotBeNull();
            testVm!.Events
                .Should().NotBeNull()
                .And.HaveCount(count)
                .And.Contain(ct => ct.Location!.ToString() == "Tartu")
                .And.Contain(ct => ct.EventName!.ToString() == $"Proov {count - 1}");
        }

        [Fact]
        public async Task Action_Test__Returns_Model_WithData()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);

            // ACT
            var result = await _uow.Event.GetAllAsync();

            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Tartu", result.Select(p => p.Location).First());
        }


        [Fact]
        public async Task Action_Test__Returns_Model_WithOneEntity()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);
            // ACT

            var all = await _uow.Event.GetAllAsync();
            var events = all.ToList();
            var result = await _uow.Event.FirstOrDefaultAsync(events[0].Id);


            Assert.NotNull(result);
            Assert.Equal("Jaanipidu", result!.EventName);
        }



        [Fact]
        public async Task Action_Test__Returns_Model_WithEditedData()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);
            // ACT

            var all = await _uow.Event.GetAllAsync();
            var events = all.ToList();
            var result = await _uow.Event.FirstOrDefaultAsync(events[0].Id);
            await EditData(result!, _uow);

            Assert.NotNull(result);
            Assert.Equal("Narva", result!.Location);
        }

        private async Task EditData(Event @event, IAppUnitOfWork _uow)
        {
            _uow = GetDLL();
            @event.Location = "Narva";

            _uow.Event.Update(@event);
            await _uow.SaveChangesAsync();
        }

        [Fact]
        public async Task Action_Test__GetAllComingEvents()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);
            // ACT
            var all = await _uow.Event.GetAllComingEvents();
            var events = all.ToList();

            var result = await _uow.Event.FirstOrDefaultAsync(events[0]!.Id);


            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Tallinn", result!.Location);
            Assert.Equal("Ironman", result!.EventName);
        }

        [Fact]
        public async Task Action_Test__GetAllPreviousEvents()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);
            // ACT
            var all = await _uow.Event.GetAllPreviousEvents();
            var events = all.ToList();

            var result = await _uow.Event.FirstOrDefaultAsync(events[0]!.Id);


            // ASSERT
            Assert.NotNull(result);
            Assert.Equal("Tartu", result!.Location);
            Assert.Equal("Jaanipidu", result!.EventName);
        }

        [Fact]
        public async Task Action_Test__GetAllParticipants()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);
            // ACT
            var all = await _uow.Event.GetAllAsync();
            var events = all.ToList();
            var participants =  _uow.Event.GetAllParticipants(events[0]!.Id);

            // ASSERT
            Assert.NotNull(participants);
            Assert.Equal("Jaana Lind", participants[0].Name);

        }



        [Fact]
        public async Task Action_Test__DeleteEvent()
        {
            var _uow = GetDLL();
            // ARRANGE
            await SeedDataDLL(_uow);
            _uow = GetDLL();
            var all = await _uow.Event.GetAllAsync();
            var events = all.ToList();

            await _uow.Event.RemoveAsync(events[0].Id);

            await _uow.SaveChangesAsync();
            // ACT
            var result = await _uow.Event.GetAllAsync();

            // ASSERT
            Assert.Single(result);
        }


        private async Task SeedDataDLL(IAppUnitOfWork tempuow)
        {


            tempuow.Event.Add(new Event
            {
                EventName = "Jaanipidu",
                Location = "Tartu",
                EventDate = new DateTime(2021, 06, 23)


            });


            tempuow.Event.Add(new Event()
            {
                EventName = "Ironman",
                Location = "Tallinn",
                EventDate = new DateTime(2030,07,07),

            });

            await tempuow.SaveChangesAsync();

            var events = await tempuow.Event.GetAllAsync();
            var eventObject = events.ToList();

            tempuow.Person.Add( new Person()
            {
                FirstName = "Jaana",
                LastName = "Lind",
                IdentificationCode = "123456",
                PaymentType = PaymentType.Cash,
                EventId = eventObject[0].Id
            });
            tempuow.Company.Add( new Company()
            {
                CompanyName = "Mina ise OÜ",
                PeopleCount = 5,
                RegisterCode = "980292r",
                PaymentType = PaymentType.Cash,
                EventId = eventObject[0].Id
            });
            await tempuow.SaveChangesAsync();
        }


        private async Task SeedData(int count = 2)
        {
            for (int i = 0; i < count; i++)
            {
                _ctx.Events.Add(new Domain.App.Event()
                {
                    EventName = $"Proov {i}",
                    EventDate = new DateTime(2021,07,07),
                    Location = "Tartu",

                });
            }
            await _ctx.SaveChangesAsync();
        }
    }

    public class CountGenerator : IEnumerable<object[]>
    {
        private static List<object[]> _data
        {
            get
            {
                var res = new List<Object[]>();
                for (int i = 1; i <= 100; i++)
                {
                    res.Add(new object[]{i});
                }

                return res;
            }
        }
        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
