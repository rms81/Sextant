// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using ReactiveUI;
using Rg.Plugins.Popup.Pages;
using Sextant.Mocks;

namespace Sextant.Plugins.Popup.Tests
{
    /// <summary>
    /// Represents a popup page.
    /// </summary>
    public class PopupMock : PopupPage, IViewFor<NavigableViewModelMock>
    {
        private NavigableViewModelMock? _viewModel;

        /// <summary>
        /// Gets or sets the view model.
        /// </summary>
        object IViewFor.ViewModel
        {
            get => _viewModel;
            set => _viewModel = value as NavigableViewModelMock;
        }

        /// <inheritdoc />
        public NavigableViewModelMock? ViewModel
        {
            get => _viewModel;
            set => _viewModel = value;
        }
    }
}
