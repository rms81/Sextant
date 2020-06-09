// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NSubstitute;
using ReactiveUI;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;
using Sextant.Abstractions;
using Sextant.Mocks;
using Shouldly;
using Xunit;

namespace Sextant.Plugins.Popup.Tests
{
    /// <summary>
    /// Tests the <see cref="PopupViewStackService"/> implementation.
    /// </summary>
    public sealed class PopupViewStackServiceTests
    {
        /// <summary>
        /// Tests that verify the Pushing property.
        /// </summary>
        public class ThePushingProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Pushing()
            {
                // Given
                PopupNavigationEventArgs pushing = null;
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Pushing.Subscribe(x => pushing = x);

                // When
                PopupMock popupMock = new PopupMock();
                navigation.Pushing += Raise.EventWith(new PopupNavigationEventArgs(popupMock, true));

                // Then
                pushing.Page.ShouldBe(popupMock);
            }
        }

        /// <summary>
        /// Tests that verify the Pushed property.
        /// </summary>
        public class ThePushedProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Pushed()
            {
                // Given
                PopupNavigationEventArgs pushing = null;
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Pushed.Subscribe(x => pushing = x);

                // When
                PopupMock popupMock = new PopupMock();
                navigation.Pushed += Raise.EventWith(new PopupNavigationEventArgs(popupMock, true));

                // Then
                pushing.Page.ShouldBe(popupMock);
            }
        }

        /// <summary>
        /// Tests that verify the Popping property.
        /// </summary>
        public class ThePoppingProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Pushing()
            {
                // Given
                PopupNavigationEventArgs pushing = null;
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Popping.Subscribe(x => pushing = x);

                // When
                PopupMock popupMock = new PopupMock();
                navigation.Popping += Raise.EventWith(new PopupNavigationEventArgs(popupMock, true));

                // Then
                pushing.Page.ShouldBe(popupMock);
            }
        }

        /// <summary>
        /// Tests that verify the Popped property.
        /// </summary>
        public class ThePoppedProperty
        {
            /// <summary>
            /// Tests the observer can respond to events.
            /// </summary>
            [Fact]
            public void Should_Observe_Popped()
            {
                // Given
                PopupNavigationEventArgs pushing = null;
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);
                sut.Popped.Subscribe(x => pushing = x);

                // When
                PopupMock popupMock = new PopupMock();
                navigation.Popped += Raise.EventWith(new PopupNavigationEventArgs(popupMock, true));

                // Then
                pushing.Page.ShouldBe(popupMock);
            }
        }

        /// <summary>
        /// Tests that verify the PushPopup generic method.
        /// </summary>
        public class ThePushPopupGenericMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                await navigation.Received(1).PushAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method calls the view model factory.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_View_Model_Factory()
            {
                // Given
                var viewModelFactory = Substitute.For<IViewModelFactory>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewModelFactory(viewModelFactory);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                viewModelFactory.Received(1).Create<NavigableViewModelMock>();
            }

            /// <summary>
            /// Tests the method calls the view locator.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_View_Locator()
            {
                // Given
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(new PopupMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewLocator(viewLocator);

                // When
                await sut.PushPopup<NavigableViewModelMock>();

                // Then
                viewLocator.Received(1).ResolveView(Arg.Any<IViewModel>());
            }
        }

        /// <summary>
        /// Tests that verify the PushPopup method.
        /// </summary>
        public class ThePushPopupMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var viewModel = new NavigableViewModelMock();
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PushPopup(viewModel);

                // Then
                await navigation.Received(1).PushAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }

            /// <summary>
            /// Tests the method calls the view locator.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_View_Locator()
            {
                // Given
                var viewModel = new NavigableViewModelMock();
                var viewLocator = Substitute.For<IViewLocator>();
                viewLocator.ResolveView(Arg.Any<IViewModel>()).Returns(new PopupMock());
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithViewLocator(viewLocator);

                // When
                await sut.PushPopup(viewModel);

                // Then
                viewLocator.Received(1).ResolveView(Arg.Any<IViewModel>());
            }
        }

        /// <summary>
        /// Tests that verify the PopPopup method.
        /// </summary>
        public class ThePopPopupMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PopPopup();

                // Then
                await navigation.Received(1).PopAsync(Arg.Any<bool>()).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Tests that verify the PopAllPopups method.
        /// </summary>
        public class ThePopAllPopupsMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.PopAllPopups();

                // Then
                await navigation.Received(1).PopAllAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Tests that verify the RemovePopup method.
        /// </summary>
        public class TheRemovePopupMethod
        {
            /// <summary>
            /// Tests the method calls the decorated method.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task Should_Call_Popup_Navigation()
            {
                // Given
                var navigation = Substitute.For<IPopupNavigation>();
                PopupViewStackService sut = new PopupViewStackServiceFixture().WithNavigation(navigation);

                // When
                await sut.RemovePopup(new NavigableViewModelMock());

                // Then
                await navigation.Received(1).RemovePageAsync(Arg.Any<PopupPage>()).ConfigureAwait(false);
            }
        }
    }
}
