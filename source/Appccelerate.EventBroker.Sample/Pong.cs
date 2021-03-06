// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Pong.cs" company="Appccelerate">
//   Copyright (c) 2008-2020   Licensed under the Apache License, Version 2.0 (the "License");   you may not use this file except in compliance with the License.   You may obtain a copy of the License at       http://www.apache.org/licenses/LICENSE-2.0   Unless required by applicable law or agreed to in writing, software   distributed under the License is distributed on an "AS IS" BASIS,   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.   See the License for the specific language governing permissions and   limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Appccelerate.EventBroker.Sample
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Appccelerate.EventBroker.Handlers;
    using Appccelerate.Events;

    /// <summary>
    /// Pong class.
    /// </summary>
    public class Pong
    {
        /// <summary>
        /// Occurs when the pong UI from UI thread is fired.
        /// </summary>
        [EventPublication(EventTopics.PongUIFromUIThread)]
        public event EventHandler UiFromUiEvent;

        /// <summary>
        /// Occurs when the pong UI from async is fired.
        /// </summary>
        [EventPublication(EventTopics.PongUIFromAsync)]
        public event EventHandler UiFromAsyncEvent;

        [EventPublication(EventTopics.BurstPong, HandlerRestriction.Asynchronous)]
        public event EventHandler<EventArgs<int>> Burst; 

        /// <summary>
        /// Handles a ping.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments</param>
        [EventSubscription(EventTopics.PingUIFromUIThread, typeof(Handlers.OnPublisher))]
        public async void HandlePingUiFromUiThread(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            this.UiFromUiEvent(this, EventArgs.Empty);
        }

        [EventSubscription(EventTopics.PingUIFromAsync, typeof(Handlers.OnBackground))]
        public async void HandlePingUiFromAsync()
        {
            await Task.Delay(3000);
            this.UiFromAsyncEvent(this, EventArgs.Empty);
        }

        [EventSubscription(EventTopics.BurstPing, typeof(Handlers.OnBackground))]
        public void HandleBurst(EventArgs<int> e)
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(50 - i);
                this.Burst(this, new EventArgs<int>(i * e.Value));
            }
        }

        [EventSubscription(EventTopics.PingSubscriberException, typeof(OnPublisher))]
        public void HandleException()
        {
            throw new Exception("evil subscriber");
        }
    }
}
