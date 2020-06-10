// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Events;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Internals;

namespace Sextant.Plugins.Popup.Tests
{
    /// <summary>
    /// A mocked implementation of <see cref="IPopupNavigation"/>.
    /// </summary>
    public class PopupNavigationMock : IPopupNavigation
    {
        private Stack<PopupPage> _stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="PopupNavigationMock"/> class.
        /// </summary>
        public PopupNavigationMock()
        {
            _stack = new Stack<PopupPage>();
        }

        /// <inheritdoc/>
        public event EventHandler<PopupNavigationEventArgs> Pushing;

        /// <inheritdoc/>
        public event EventHandler<PopupNavigationEventArgs> Pushed;

        /// <inheritdoc/>
        public event EventHandler<PopupNavigationEventArgs> Popping;

        /// <inheritdoc/>
        public event EventHandler<PopupNavigationEventArgs> Popped;

        /// <inheritdoc/>
        public IReadOnlyList<PopupPage> PopupStack => _stack.ToArray();

        /// <inheritdoc/>
        public Task PushAsync(PopupPage page, bool animate = true)
        {
            Pushing?.Invoke(this, new PopupNavigationEventArgs(page, animate));
            _stack.Push(page);
            Pushed?.Invoke(this, new PopupNavigationEventArgs(page, animate));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PopAsync(bool animate = true)
        {
            Popping?.Invoke(this, new PopupNavigationEventArgs(_stack.Peek(), animate));
            var poppedPage = _stack.Pop();
            Popped?.Invoke(this, new PopupNavigationEventArgs(poppedPage, animate));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task PopAllAsync(bool animate = true)
        {
            Popping?.Invoke(this, new PopupNavigationEventArgs(_stack.Peek(), animate));
            var poppedPage = _stack.ToArray().First();
            _stack.Clear();
            Popped?.Invoke(this, new PopupNavigationEventArgs(poppedPage, animate));
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task RemovePageAsync(PopupPage page, bool animate = true)
        {
            var remove = _stack.ToArray()[EnumerableExtensions.IndexOf(_stack.ToArray(), page)];
            _stack.ToArray().ToList().Remove(remove);
            return Task.CompletedTask;
        }
    }
}
