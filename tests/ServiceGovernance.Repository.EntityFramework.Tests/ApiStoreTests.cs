using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using ServiceGovernance.Repository.EntityFramework.Stores;
using ServiceGovernance.Repository.EntityFramework.Tests.Builder;
using ServiceGovernance.Repository.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceGovernance.Repository.EntityFramework.Tests
{
    [TestFixture]
    public class ApiStoreTests : DbAwareTests<RepositoryDbContext, RepositoryStoreOptions>
    {
        public ApiStoreTests() : base("ApiStoreTests")
        {
            using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                context.Database.EnsureCreated();
        }

        public class FindByServiceIdAsyncMethod : ApiStoreTests
        {
            [Test]
            public async Task Returns_Item_When_Api_Exists()
            {
                var model = new ServiceApiDescription
                {
                    ServiceId = "Returns_Item_When_Api_Exists"
                };

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Apis.Add(model.ToEntity());
                    context.SaveChanges();
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    var item = await store.FindByServiceIdAsync(model.ServiceId);
                    item.Should().NotBeNull();
                }
            }

            [Test]
            public async Task Returns_Null_When_Api_Not_Exists()
            {
                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    var item = await store.FindByServiceIdAsync("someServiceId");

                    item.Should().BeNull();
                }
            }

            [Test]
            public async Task Returns_Item_With_All_Properties_When_Api_Exists()
            {
                var model = new ServiceApiDescriptionBuilder().Build();

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Apis.Add(model.ToEntity());
                    context.SaveChanges();
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    var item = await store.FindByServiceIdAsync(model.ServiceId);

                    item.Should().NotBeNull();
                    item.ServiceId.Should().Be(model.ServiceId);

                    // TODO check api document
                }
            }
        }

        public class GetAllAsyncMethod : ApiStoreTests
        {
            [Test]
            public async Task Returns_All_Items()
            {
                var model1 = new ServiceApiDescriptionBuilder().WithServiceId("AllItem1").Build();
                var model2 = new ServiceApiDescriptionBuilder().WithServiceId("AllItem2").Build();

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Apis.RemoveRange(context.Apis.ToArray());
                    context.Apis.Add(model1.ToEntity());
                    context.Apis.Add(model2.ToEntity());
                    context.SaveChanges();
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    var services = (await store.GetAllAsync()).ToList();

                    services.Should().HaveCount(2);
                    services[0].ServiceId.Should().Be(model1.ServiceId);
                    services[0].ApiDocument.Info.Title.Should().Be(model1.ApiDocument.Info.Title);

                    services[1].ServiceId.Should().Be(model2.ServiceId);
                    services[1].ApiDocument.Info.Title.Should().Be(model2.ApiDocument.Info.Title);
                }
            }
        }

        public class RemoveAsyncMethod : ApiStoreTests
        {
            [Test]
            public async Task Removes_Existing_Item()
            {
                var model = new ServiceApiDescriptionBuilder().WithServiceId("Removes_Existing_Item").Build();      

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Apis.Add(model.ToEntity());
                    context.SaveChanges();
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    await store.RemoveAsync(model.ServiceId);
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Apis.SingleOrDefault(s => s.ServiceId == model.ServiceId).Should().BeNull();
                }
            }

            [Test]
            public void Does_Not_Throw_On_NonExisting_Item()
            {
                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    Func<Task> action = async () => await store.RemoveAsync("anyserviceid");

                    action.Should().NotThrow();
                }
            }
        }

        public class StoreAsyncMethod : ApiStoreTests
        {
            [Test]
            public async Task Saves_New_Item()
            {
                var model = new ServiceApiDescriptionBuilder().WithServiceId("Saves_New_Item").Build();               

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    await store.StoreAsync(model);
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var apiDescription = context.Apis.SingleOrDefault(s => s.ServiceId == model.ServiceId);
                    apiDescription.Should().NotBeNull();

                    apiDescription.ServiceId.Should().Be(model.ServiceId);
                    // TODO check api document
                }
            }

            [Test]
            public async Task Updates_ApiDocument_On_Existing_Item()
            {
                var model = new ServiceApiDescriptionBuilder().WithServiceId("Updates_DisplayName_On_Existing_Item").Build(); 

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    context.Apis.Add(model.ToEntity());
                    context.SaveChanges();
                }

                model.ApiDocument.Info.Title = "New title";

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var store = new ApiStore(context, new Mock<ILogger<ApiStore>>().Object);
                    await store.StoreAsync(model);
                }

                using (var context = new RepositoryDbContext(DbContextOptions, StoreOptions))
                {
                    var item = context.Apis.SingleOrDefault(s => s.ServiceId == model.ServiceId);
                    item.Should().NotBeNull();

                    var document = OpenApiDocumentHelper.ReadFromJson(item.ApiDocument);

                    document.Info.Title.Should().Be(model.ApiDocument.Info.Title);
                }
            }           
        }
    }
}
