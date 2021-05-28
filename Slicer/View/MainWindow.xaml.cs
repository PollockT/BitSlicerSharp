﻿using Slicer.Source.Controls;
using Slicer.Source.Scanning;

namespace Slicer.View
{
    using Slicer.Engine.Common.DataTypes;
    using Slicer.Source.Controls;
    using Slicer.Source.Results;
    using Slicer.Source.Scanning;
    using System;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManualScanner"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.ValueHexDecBoxViewModel = this.ValueHexDecBox.DataContext as HexDecBoxViewModel;
            this.ValueHexDecBoxViewModel.PropertyChanged += HexDecBoxViewModelPropertyChanged;
        }

        private HexDecBoxViewModel ValueHexDecBoxViewModel { get; set; }

        /// <summary>
        /// Updates the active type.
        /// </summary>
        /// <param name="activeType">The new active type.</param>
        public void Update(DataTypeBase activeType)
        {
            this.ValueHexDecBoxViewModel.DataType = activeType;
        }

        private void HexDecBoxViewModelPropertyChanged(Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ValueHexDecBoxViewModel.Text))
            {
                ManualScannerViewModel.GetInstance().UpdateActiveValueCommand.Execute(this.ValueHexDecBoxViewModel.GetValue());
            }
        }
    }
    //// End class
}
//// End namespace