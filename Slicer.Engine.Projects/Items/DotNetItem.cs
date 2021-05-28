﻿namespace Slicer.Engine.Projects.Items
{
    using Slicer.Engine.Memory;
    using Squalr.Engine.Processes;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class DotNetItem : AddressItem
    {
        /// <summary>
        /// The extension for this project item type.
        /// </summary>
        public new const String Extension = ".clr";

        [DataMember]
        private String identifier;

        public DotNetItem(ProcessSession processSession) : base(processSession)
        {
        }

        public DotNetItem(ProcessSession processSession, String name, Type type, String identifier) : base(processSession, type, name)
        {
            this.Identifier = identifier;
        }

        /// <summary>
        /// Gets or sets the identifier for this .NET object.
        /// </summary>
        public virtual String Identifier
        {
            get
            {
                return this.identifier;
            }

            set
            {
                this.identifier = value;
                this.RaisePropertyChanged(nameof(this.Identifier));
            }
        }

        /// <summary>
        /// Gets the extension for this project item.
        /// </summary>
        public override String GetExtension()
        {
            return DotNetItem.Extension;
        }

        /// <summary>
        /// Resolves the address of this object.
        /// </summary>
        /// <returns>The base address of this object.</returns>
        protected override UInt64 ResolveAddress()
        {
            return AddressResolver.GetInstance().ResolveDotNetObject(this.Identifier);
        }
    }
    //// End class
}
//// End namespace