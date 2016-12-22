﻿using System;
using System.Threading.Tasks;

namespace Ractor {

    public struct QueueReceiveResult<T> where T : class {
        public bool OK { get; internal set; }
        public T Value { get; internal set; }
        public string DeleteHandle { get; internal set; }
    }

    /// <summary>
    /// Distributed queue (AWS SQS model)
    /// </summary>
    public interface IQueue<T> where T : class {
        int Timeout { get; }
        Task<bool> TrySendMessage(T message);
        Task<QueueReceiveResult<T>> TryReceiveMessage();
        Task<bool> TryDeleteMessage(string deleteHandle);
    }
}
