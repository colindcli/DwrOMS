using MassTransit;
using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity
{
    /// <summary>
    /// 有序Guid
    /// </summary>
    public struct SeqGuid
    {
        /// <summary>
        /// 产生Guid
        /// SeqGuid.NewGuid().ToNewId().Timestamp==DateTime.Now.ToUniversalTime()
        /// </summary>
        /// <returns></returns>
        public static Guid NewGuid()
        {
            return NewId.NextGuid();
        }
    }

    public class MockTickProvider : ITickProvider
    {
        public MockTickProvider(long ticks)
        {
            Ticks = ticks;
        }

        public long Ticks { get; }
    }

    public class MockNetworkProvider : IWorkerIdProvider
    {
        private readonly byte[] _workerId;

        public MockNetworkProvider(byte[] workerId)
        {
            _workerId = workerId;
        }

        public byte[] GetWorkerId(int index)
        {
            return _workerId;
        }
    }

    public class MockProcessIdProvider : IProcessIdProvider
    {
        private readonly byte[] _processId;

        public MockProcessIdProvider(byte[] processId)
        {
            _processId = processId;
        }

        public byte[] GetProcessId()
        {
            return _processId;
        }
    }
}
