// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Interface representing a Sextant decorator of <see cref="IPopupNavigation"/>.
    /// </summary>
    public interface IPopupViewStackService
    {
        /// <summary>
        /// Gets an observable sequence of pushing events.
        /// </summary>
        IObservable<PopupNavigationEventArgs> Pushing { get; }

        /// <summary>
        /// Gets an observable sequence of pushed events.
        /// </summary>
        IObservable<PopupNavigationEventArgs> Pushed { get; }

        /// <summary>
        /// Gets an observable sequence of popping events.
        /// </summary>
        IObservable<PopupNavigationEventArgs> Popping { get; }

        /// <summary>
        /// Gets an observable sequence of popped events.
        /// </summary>
        IObservable<PopupNavigationEventArgs> Popped { get; }

        /// <summary>
        /// Gets the popup stack.
        /// </summary>
        IReadOnlyList<IViewModel> PopupStack { get; }

        /// <summary>
        /// Push a pop up page to the stack.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PushPopup(IViewModel viewModel, string? contract = null, bool animate = true);

        /// <summary>
        /// Push a pop up page to the stack.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="animate">Animate the page.</param>
        /// <typeparam name="TViewModel">The view model type.</typeparam>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PushPopup<TViewModel>(string? contract = null, bool animate = true)
            where TViewModel : IViewModel;

        /// <summary>
        /// Pop a pop up page.
        /// </summary>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PopPopup(bool animate = true);

        /// <summary>
        /// Pop all popups from the stack.
        /// </summary>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> PopAllPopups(bool animate = true);

        /// <summary>
        /// Remove Popup.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="animate">Animate the page.</param>
        /// <returns>A completion notification.</returns>
        IObservable<Unit> RemovePopup(IViewModel viewModel, bool animate = true);
    }
}
