namespace Narvalo.Web {
    using System;
    using System.ComponentModel;
    using System.Resources;

    public class LocalizedDisplayNameAttribute : DisplayNameAttribute {
        public LocalizedDisplayNameAttribute(Type resourceType, string resourceName)
            : this(new ResourceManager(resourceType), resourceName) {
            ;
        }

        protected LocalizedDisplayNameAttribute(ResourceManager resourceManager, string resourceName)
            : base() {
            ResourceManager = resourceManager;
            ResourceName = resourceName;
        }

        public override string DisplayName {
            get {
                return ResourceManager.GetString(ResourceName);
            }
        }

        public string ResourceName {
            get;
            protected set;
        }

        protected ResourceManager ResourceManager {
            get;
            set;
        }
    }
}
