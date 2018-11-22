using System.Diagnostics;

namespace ServiceGovernance.Repository.EntityFramework.Entities
{
    [DebuggerDisplay("{Id} ({ServiceId})")]
    public class ApiDescription
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a unique service identifier
        /// </summary>
        public string ServiceId { get; set; }

        /// <summary>
        /// Gets or sets the openApi document as json
        /// </summary>
        public string ApiDocument { get; set; }
    }
}
