﻿namespace Slicer.Engine.Projects.Items
{
    using System;
    using System.ComponentModel;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines a script that can be added to the project explorer.
    /// </summary>
    [KnownType(typeof(ProjectItem))]
    [KnownType(typeof(ScriptItem))]
    [KnownType(typeof(AddressItem))]
    [KnownType(typeof(InstructionItem))]
    [KnownType(typeof(PointerItem))]
    [KnownType(typeof(DotNetItem))]
    [KnownType(typeof(JavaItem))]
    [DataContract]
    public class ScriptItem : ProjectItem
    {
        /// <summary>
        /// The extension for this project item type.
        /// </summary>
        public const String Extension = ".cs";

        /// <summary>
        /// The raw script text.
        /// </summary>
        [Browsable(false)]
        [DataMember]
        private String script;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptItem" /> class.
        /// </summary>
        public ScriptItem() : this("New Script", null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptItem" /> class.
        /// </summary>
        /// <param name="description">The description of the project item.</param>
        /// <param name="script">The raw script text.</param>
        /// <param name="compiled">Whether or not this script is compiled.</param>
        public ScriptItem(String description, String script) : base(description)
        {
            // Initialize script and bypass setters
            this.script = script;
        }

        /// <summary>
        /// Gets or sets the raw script text.
        /// </summary>
        public virtual String Script
        {
            get
            {
                return this.script;
            }

            set
            {
                this.script = value;
            }
        }

        /// <summary>
        /// Gets the extension for this project item.
        /// </summary>
        public override String GetExtension()
        {
            return ScriptItem.Extension;
        }

        /// <summary>
        /// Invoked when this object is deserialized.
        /// </summary>
        /// <param name="streamingContext">Streaming context.</param>
        [OnDeserialized]
        public new void OnDeserialized(StreamingContext streamingContext)
        {
            base.OnDeserialized(streamingContext);
        }
    }
    //// End class
}
//// End namespace