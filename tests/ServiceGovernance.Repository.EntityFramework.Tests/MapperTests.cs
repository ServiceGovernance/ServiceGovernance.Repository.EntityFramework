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

            var newmodel = entity.ToModel();
            newmodel.ApiDocument.Info.Title.Should().Be(model.ApiDocument.Info.Title);
            document.Info.Title.Should().Be(model.ApiDocument.Info.Title);
        }

        [Test]
        public void UpdateEntity_Updates_ApiDocument()
        {
            var model = new ServiceApiDescriptionBuilder().Build();
            var entity = model.ToEntity();

            model.ApiDocument.Info.Title = "UpdateEntity_Updates_ApiDocument";
            model.UpdateEntity(entity);

            var document = OpenApiDocumentHelper.ReadFromJson(entity.ApiDocument);
            document.Info.Title.Should().Be(model.ApiDocument.Info.Title);
        }

        [Test]
        public void UpdateEntity_Updates_ServiceId()
        {
            var model = new ServiceApiDescriptionBuilder().Build();
            var entity = model.ToEntity();

            model.ServiceId = "UpdateEntity_Updates_ServiceId";
            model.UpdateEntity(entity);

            entity.ServiceId.Should().Be(model.ServiceId);
        }
    }
}
