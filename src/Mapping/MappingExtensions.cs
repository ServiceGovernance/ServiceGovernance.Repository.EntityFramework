using AutoMapper;
using ServiceGovernance.Repository.EntityFramework.Entities;
using ServiceGovernance.Repository.EntityFramework.Mapping;
using ServiceGovernance.Repository.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceGovernance.Repository.EntityFramework
{
    /// <summary>
    /// Extensions methods to map from or to entites/models
    /// </summary>
    public static class MappingExtensions
    {
        static MappingExtensions()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ApiDescriptionMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        /// <summary>
        /// Maps an entity to a model.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static ServiceApiDescription ToModel(this ApiDescription entity)
        {
            return Mapper.Map<ServiceApiDescription>(entity);
        }

        /// <summary>
        /// Maps an entity list to a model list.
        /// </summary>
        /// <param name="entityList">The entity list.</param>
        /// <returns></returns>
        public static List<ServiceApiDescription> ToModelList(this IEnumerable<ApiDescription> entityList)
        {
            return Mapper.Map<List<ServiceApiDescription>>(entityList);
        }

        /// <summary>
        /// Maps a model to an entity.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public static ApiDescription ToEntity(this ServiceApiDescription model)
        {
            return Mapper.Map<ApiDescription>(model);
        }

        /// <summary>
        /// Updates an entity from a model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="entity">The entity.</param>
        public static void UpdateEntity(this ServiceApiDescription model, ApiDescription entity)
        {
            Mapper.Map(model, entity);
        }
    }
}
