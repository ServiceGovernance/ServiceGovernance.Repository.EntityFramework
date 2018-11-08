using FluentAssertions;
using NUnit.Framework;
using ServiceGovernance.Repository.EntityFramework.Mapping;
using ServiceGovernance.Repository.EntityFramework.Tests.Builder;
using ServiceGovernance.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceGovernance.Repository.EntityFramework.Tests
{
    [TestFixture]
    public class MapperTests
    {
        [Test]
        public void AutomapperConfigurationIsValid()
        {
            MappingExtensions.Mapper.ConfigurationProvider.AssertConfigurationIsValid<ApiDescriptionMapperProfile>();
        }

        [Test]
        public void Can_Convert_To_And_From_Entity()
        {
            var model = new ServiceApiDescription();
            var entity = model.ToEntity();
            model = entity.ToModel();

            entity.Should().NotBeNull();
            model.Should().NotBeNull();
        }

        [Test]
        public void Maps_All_Properties()
        {
            var model = new ServiceApiDescriptionBuilder().Build();
            var entity = model.ToEntity();     
            entity.ServiceId.Should().Be(model.ServiceId);

            var document = OpenApiDocumentHelper.ReadFromJson(entity.ApiDocument);

            model = entity.ToModel();
            
            //model.ServiceId.Should().Be(entity.ServiceId);
            //model.Endpoints.Should().HaveCount(2);
            //model.Endpoints[0].Should().Be(entity.Endpoints[0].EndpointUri);
            //model.Endpoints[1].Should().Be(entity.Endpoints[1].EndpointUri);

            //model.IpAddresses.Should().HaveCount(2);
            //model.IpAddresses[0].Should().Be(entity.IpAddresses[0].IpAddress);
            //model.IpAddresses[1].Should().Be(entity.IpAddresses[1].IpAddress);

            //model.PublicUrls.Should().HaveCount(1);
            //model.PublicUrls[0].Should().Be(entity.PublicUrls[0].Url);
        }
    }
}
