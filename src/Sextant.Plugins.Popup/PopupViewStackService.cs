// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;
using Sextant.Abstractions;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Represents a popup view stack service implementation.
    /// </summary>
    public class PopupViewStackService : ParameterViewStackService, IPopupViewStackService
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IViewLocator _viewLocator;
        private readonly IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupViewStackService"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="popupNavigation">The popup navigation.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        public PopupViewStackService(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
            : base(view)
        {
            _popupNavigation = popupNavigation;
            _viewLocator = viewLocator;
            _viewModelFactory = viewModelFactory;

            Pushing = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Pushing += x,
                x => _popupNavigation.Pushing -= x)
                .Select(x => new PopupNavigationEvent(x.Page, x.IsAnimated));

            Pushed = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Pushed += x,
                x => _popupNavigation.Pushed -= x)
                .Select(x => new PopupNavigationEvent(x.Page, x.IsAnimated));

            Popping = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Popping += x,
                x => _popupNavigation.Popping -= x)
                .Select(x => new PopupNavigationEvent(x.Page, x.IsAnimated));

            Popped = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Popped += x,
                x => _popupNavigation.Popped -= x)
                .Select(x => new PopupNavigationEvent(x.Page, x.IsAnimated));
        }

        /// <inheritdoc/>
        public IObservable<PopupNavigationEvent> Pushing { get; }

        /// <inheritdoc/>
        public IObservable<PopupNavigationEvent> Pushed { get; }

        /// <inheritdoc/>
        public IObservable<PopupNavigationEvent> Popping { get; }

        /// <inheritdoc/>
        public IObservable<PopupNavigationEvent> Popped { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IViewModel> PopupStack { get; }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup(IViewModel viewModel, string? contract = null, bool animate = true)
        {
            var page = (PopupPage)_viewLocator.ResolveView(viewModel, contract);
            return _popupNavigation.PushAsync(page, animate).ToObservable();
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup<TViewModel>(string? contract = null, bool animate = true)
            where TViewModel : IViewModel
        {
            var viewModel = _viewModelFactory.Create<TViewModel>();
            return PushPopup(viewModel, contract, animate);
        }

        /// <inheritdoc/>
        public IObservable<Unit> PopPopup(bool animate = true) => _popupNavigation.PopAsync(animate).ToObservable();

        /// <inheritdoc/>
        public IObservable<Unit> PopAllPopups(bool animate = true) => _popupNavigation.PopAllAsync(animate).ToObservable();

        /// <inheritdoc/>
        public IObservable<Unit> RemovePopup(IViewModel viewModel, bool animate = true)
        {
            var page = (PopupPage)_viewLocator.ResolveView(viewModel);
            return _popupNavigation.RemovePageAsync(page).ToObservable();
        }
    }
}
