using System;
using System.Collections;

namespace Mcd79.MigrationValidationEngine.Core.Model
{
    public class DataRepoConfig
    {
        public string Endpoint { get; set; }
        public string PrimaryKey { get; set; }
        public string DatabaseId { get; set; }
        public string ContainerId { get; set;}
    }
}
