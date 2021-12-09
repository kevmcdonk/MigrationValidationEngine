using System;
using System.Collections;
using System.Collections.Generic;

namespace Mcd79.MigrationValidationEngine.Core.Model
{
    //TODO: consider not using this and use a standard site model as staging
    public class MVESiteInfo
    {
        public string SiteDisplayName { get; set; }
        public string SiteAlias { get; set; }
        public List<MVESiteContentTypeInfo> SiteContentTypes { get; set; }
    }
}
