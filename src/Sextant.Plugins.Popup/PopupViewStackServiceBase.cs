// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;

namespace Sextant.Plugins.Popup
{
    /// <summary>
    /// Represents a popup view stack service implementation.
    /// </summary>
    public class PopupViewStackServiceBase : ParameterViewStackServiceBase, IPopupViewStackService
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IViewLocator _viewLocator;
        private readonly IViewModelFactory _viewModelFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupViewStackServiceBase"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="popupNavigation">The popup navigation.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <param name="viewModelFactory">The view model factory.</param>
        public PopupViewStackServiceBase(IView view, IPopupNavigation popupNavigation, IViewLocator viewLocator, IViewModelFactory viewModelFactory)
            : base(view)
        {
            _popupNavigation = popupNavigation;
            _viewLocator = viewLocator;
            _viewModelFactory = viewModelFactory;
            PopupSubject = new BehaviorSubject<IImmutableList<IViewModel>>(ImmutableList<IViewModel>.Empty);

            Pushing = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args) => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Pushing += x,
                x => _popupNavigation.Pushing -= x)
                .Select(x => new PopupNavigationEvent((IViewFor<IViewModel>)x.Page, x.IsAnimated));

            Pushed = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Pushed += x,
                x => _popupNavigation.Pushed -= x)
                .Select(x => new PopupNavigationEvent((IViewFor<IViewModel>)x.Page, x.IsAnimated));

            Popping = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args)
                        => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Popping += x,
                x => _popupNavigation.Popping -= x)
                .Select(x => new PopupNavigationEvent((IViewFor<IViewModel>)x.Page, x.IsAnimated));

            Popped = Observable.FromEvent<EventHandler<PopupNavigationEventArgs>, PopupNavigationEventArgs>(
                eventHandler =>
                {
                    void Handler(object sender, PopupNavigationEventArgs args) => eventHandler(args);

                    return Handler;
                },
                x => _popupNavigation.Popped += x,
                x => _popupNavigation.Popped -= x)
                .Select(x => new PopupNavigationEvent((IViewFor<IViewModel>)x.Page, x.IsAnimated));
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
        public IReadOnlyList<IViewModel> PopupStack =>
#pragma warning disable 8619
            _popupNavigation
                .PopupStack
                .Cast<IViewFor<IViewModel>>()
                .Select(x => x.ViewModel)
                .Where(x => x != null)
                .ToList();
#pragma warning restore 8619

        /// <summary>
        /// Gets the popup subject that contains the stack history.
        /// </summary>
        protected BehaviorSubject<IImmutableList<IViewModel>> PopupSubject { get; }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup(IViewModel viewModel, string? contract = null, bool animate = true)
        {
            PopupPage? popupPage = _viewLocator.ResolveView(viewModel, contract) as PopupPage;
            if (popupPage == null)
            {
                throw new InvalidOperationException($"{nameof(popupPage)} does not exist.");
            }

            return _popupNavigation.PushAsync(popupPage, animate).ToObservable();
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup<TViewModel>(string? contract = null, bool animate = true)
            where TViewModel : IViewModel
        {
            var viewModel = _viewModelFactory.Create<TViewModel>();
            return PushPopup(viewModel, contract, animate);
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopupUntilPopped(IViewModel viewModel, string? contract = null, bool animate = true)
        {
            return null;
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopupUntilPopped<TViewModel>(string? contract = null, bool animate = true)
            where TViewModel : IViewModel
        {
            return null;
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup(
            INavigable viewModel,
            INavigationParameter navigationParameter,
            string? contract = null,
            bool animate = true)
        {
            return null;
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopup<TViewModel>(
            INavigationParameter navigationParameter,
            string? contract = null,
            bool animate = true)
            where TViewModel : INavigable
        {
            return null;
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopupUntilPopped(
            INavigable viewModel,
            INavigationParameter navigationParameter,
            string? contract = null,
            bool animate = true)
        {
            return null;
        }

        /// <inheritdoc/>
        public IObservable<Unit> PushPopupUntilPopped<TViewModel>(
            INavigationParameter navigationParameter,
            string? contract = null,
            bool animate = true)
            where TViewModel : INavigable
        {
            return null;
        }

        /// <inheritdoc/>
        public IObservable<Unit> PopPopup(bool animate = true) =>
            Observable.FromAsync(() => _popupNavigation.PopAsync(animate));

        /// <inheritdoc/>
        public IObservable<Unit> PopAllPopups(bool animate = true) =>
            Observable.FromAsync(() => _popupNavigation.PopAllAsync(animate));

        /// <inheritdoc/>
        public IObservable<Unit> RemovePopup(IViewModel viewModel, string contract, bool animate = true)
        {
            PopupPage? popupPage = _viewLocator.ResolveView(viewModel, contract) as PopupPage;
            if (popupPage == null)
            {
                throw new InvalidOperationException($"{nameof(popupPage)} does not exist.");
            }

            return Observable.FromAsync(() => _popupNavigation.RemovePageAsync(popupPage, animate))
                             .Do(_ => RemoveFromStackAndTick(PopupSubject, viewModel));
        }

        private void RemoveFromStackAndTick<T>(BehaviorSubject<IImmutableList<T>> stackSubject, T item)
        {
            if (stackSubject == null)
            {
                throw new ArgumentNullException(nameof(stackSubject));
            }

            var stack = stackSubject.Value;

            stack = stack.Remove(item);

            stackSubject.OnNext(stack);
        }

        private PopupPage LocatePopupFor(object viewModel, string? contract)
        {
            var view = _viewLocator.ResolveView(viewModel, contract);
            var page = view as PopupPage;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (view == null)
            {
                throw new InvalidOperationException($"Could not find view for type '{viewModel.GetType().FullName}', contract '{contract}' does not implement IViewFor.");
            }

            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            view.ViewModel = viewModel;

            return page;
        }
    }
}
