﻿using Slicer.Source.Controls;
using Slicer.Source.Editors.ScriptEditor;
using Slicer.Source.Utils.TypeConverters;

namespace Slicer.Source.ProjectExplorer.ProjectItems
{
    using Slicer.Engine.Projects.Items;
    using Slicer.Engine.Scripting;
    using Slicer.Source.Controls;
    using Slicer.Source.Editors.ScriptEditor;
    using Slicer.Source.Utils.TypeConverters;
    using System;
    using System.ComponentModel;
    using System.Drawing.Design;
    using System.Reflection;

    /// <summary>
    /// Decorates the base project item class with annotations for use in the view.
    /// </summary>
    internal class ScriptItemView : ProjectItemView
    {
        private ScriptItem scriptItem;

        public ScriptItemView(ScriptItem scriptItem)
        {
            this.ScriptItem = scriptItem;
        }

        [Browsable(false)]
        private ScriptItem ScriptItem
        {
            get
            {
                return this.scriptItem;
            }

            set
            {
                this.scriptItem = value;
                this.ProjectItem = value;
                this.RaisePropertyChanged(nameof(this.ScriptItem));
            }
        }

        /// <summary>
        /// Gets or sets the description for this object.
        /// </summary>
        [Browsable(true)]
        [SortedCategory(SortedCategory.CategoryType.Common), DisplayName("Name"), Description("The name of this script")]
        public String Name
        {
            get
            {
                return this.ScriptItem.Name;
            }

            set
            {
                this.ScriptItem.Name = value;
                this.RaisePropertyChanged(nameof(this.Name));
            }
        }

        /// <summary>
        /// Gets or sets the raw script text.
        /// </summary>
        [Browsable(true)]
        [ReadOnly(false)]
        [TypeConverter(typeof(ScriptConverter))]
        [Editor(typeof(ScriptEditorModel), typeof(UITypeEditor))]
        [SortedCategory(SortedCategory.CategoryType.Common), DisplayName("Script"), Description("C# script to interface with engine")]
        public String Script
        {
            get
            {
                return this.ScriptItem.Script;
            }

            set
            {
                this.ScriptItem.Script = value;
                this.RaisePropertyChanged(nameof(this.Script));
                this.CompileTest();
            }
        }

        private void CompileTest()
        {
            Assembly assembly = this.ScriptItem.Compile(false);
            ModScript modScript = ModScript.FromAssembly(assembly);
        }
    }
    //// End class
}
//// End namespace