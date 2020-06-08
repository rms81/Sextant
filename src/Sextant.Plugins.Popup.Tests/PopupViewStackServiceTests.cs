// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Rg.Plugins.Popup.Contracts;
using Xunit;

namespace Sextant.Plugins.Popup.Tests
{
    /// <summary>
    /// Tests the <see cref="PopupViewStackService"/> implementation.
    /// </summary>
    public class PopupViewStackServiceTests
    {
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
    }
}
