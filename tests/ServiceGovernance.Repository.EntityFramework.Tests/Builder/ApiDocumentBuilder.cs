using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ServiceGovernance.Repository.EntityFramework.Tests.Builder
{
    /// <summary>
    /// Helper class to build test api documents
    /// </summary>
    public class ApiDocumentBuilder
    {
        private readonly OpenApiDocument _document = BuildDefaultDocument();

        private static OpenApiDocument BuildDefaultDocument()
        {
            return new OpenApiDocument
            {
                Info = new OpenApiInfo
                {
                    Title = "My Api",
                    Version = "1.0"
                },
                Paths = new OpenApiPaths
                {
                    { "/pets", new OpenApiPathItem {
                        Description = "test path",
                        Operations = new Dictionary<OperationType, OpenApiOperation>
                        {
                            {OperationType.Get, new OpenApiOperation(){
                                Description = "Returns all pets from the system that the user has access to",
                                Responses = new OpenApiResponses(){
                                    { "200", new OpenApiResponse(){Description = "A list of pets"} }
                                }
                            } }
                        }
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Returns the built document
        /// </summary>
        /// <returns></returns>
        public OpenApiDocument Build()
        {
            return _document;
        }

        /// <summary>
        /// Returns the built document as serialized (V3) string
        /// </summary>
        /// <returns></returns>
        public string BuildAsString()
        {
            var document = Build();

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                var apiWriter = new OpenApiJsonWriter(writer);
                document.SerializeAsV3(apiWriter);
                apiWriter.Flush();
                writer.Flush();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes the Info.Title property value
        /// </summary>
        /// <returns></returns>
        public ApiDocumentBuilder WithoutTitle()
        {
            _document.Info.Title = "";

            return this;
        }

        /// <summary>
        /// Removes the Info property
        /// </summary>
        /// <returns></returns>
        public ApiDocumentBuilder WithoutInfo()
        {
            _document.Info = null;

            return this;
        }

        /// <summary>
        /// Removes all paths
        /// </summary>
        /// <returns></returns>
        public ApiDocumentBuilder WithoutPaths()
        {
            _document.Paths.Clear();

            return this;
        }
    }
}
