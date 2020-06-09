// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Reactive;
using System.Reactive.Linq;
using NSubstitute;
using ReactiveUI;
using ReactiveUI.Testing;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Pages;
using Sextant.Abstractions;
using Sextant.Mocks;

namespace Sextant.Plugins.Popup.Tests
{
    internal class PopupViewStackServiceFixture : IBuilder
    {
        private IView _view;
        private IPopupNavigation _popupNavigation;
        private IViewLocator _viewLocator;
        private IViewModelFactory _viewModelFactory;

        public PopupViewStackServiceFixture()
        {
            _view = Substitute.For<IView>();
            _popupNavigation = Substitute.For<IPopupNavigation>();
            _viewLocator = Substitute.For<IViewLocator>();
            _viewModelFactory = Substitute.For<IViewModelFactory>();

            _view.PushPage(Arg.Any<INavigable>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Observable.Return(Unit.Default));
            _view.PopPage().Returns(Observable.Return(Unit.Default));

            _viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(new PopupMock());
        }

        public static implicit operator PopupViewStackService(PopupViewStackServiceFixture fixture) =>
            fixture.Build();

        public PopupViewStackServiceFixture WithNavigation(IPopupNavigation popupNavigation) =>
            this.With(ref _popupNavigation, popupNavigation);

        private PopupViewStackService Build() =>
            new PopupViewStackService(_view, _popupNavigation, _viewLocator, _viewModelFactory);
    }
}