using System;

namespace Mcd79.MigrationValidationEngine.Core.Model
{
    //TODO: consider not using this and use a standard site model as staging
    public class MVEFileLineage
    {
        public string SiteDisplayName { get; set; }
        public string SiteAlias { get; set; }
    }
}
