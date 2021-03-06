﻿#region License
// 
//     CoiniumServ - Crypto Currency Mining Pool Server Software
//     Copyright (C) 2013 - 2014, CoiniumServ Project - http://www.coinium.org
//     http://www.coiniumserv.com - https://github.com/CoiniumServ/CoiniumServ
// 
//     This software is dual-licensed: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
// 
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//    
//     For the terms of this license, see licenses/gpl_v3.txt.
// 
//     Alternatively, you can license this software under a commercial
//     license or white-label it as set out in licenses/commercial.txt.
// 
#endregion

using System;
using CoiniumServ.Crypto.Algorithms;
using CoiniumServ.Daemon;
using CoiniumServ.Daemon.Responses;
using CoiniumServ.Mining.Banning;
using CoiniumServ.Mining.Jobs.Manager;
using CoiniumServ.Mining.Jobs.Tracker;
using CoiniumServ.Mining.Miners;
using CoiniumServ.Mining.Pools;
using CoiniumServ.Mining.Pools.Config;
using CoiniumServ.Mining.Pools.Statistics;
using CoiniumServ.Mining.Shares;
using CoiniumServ.Mining.Vardiff;
using CoiniumServ.Payments;
using CoiniumServ.Persistance;
using CoiniumServ.Server;
using CoiniumServ.Server.Mining;
using CoiniumServ.Server.Mining.Service;
using NSubstitute;
using Should.Fluent;
using Xunit;

namespace CoiniumServ.Tests.Mining.Pools
{
    public class PoolTests
    {
        // factory mocks
        private readonly IServerFactory _serverFactory;
        private readonly IServiceFactory _serviceFactory;
        private readonly IJobManagerFactory _jobManagerFactory;
        private readonly IJobTrackerFactory _jobTrackerFactory;
        private readonly IShareManagerFactory _shareManagerFactory;
        private readonly IHashAlgorithmFactory _hashAlgorithmFactory;
        private readonly IMinerManagerFactory _minerManagerFactory;
        private readonly IStorageFactory _storageFactory;
        private readonly IPaymentProcessorFactory _paymentProcessorFactory;
        private readonly IStatisticsObjectFactory _statisticsObjectFactory;
        private readonly IVardiffManagerFactory _vardiffManagerFactory;
        private readonly IBanManagerFactory _banManagerFactory;

        // object mocks.
        private readonly IDaemonClient _daemonClient;
        private readonly IMinerManager _minerManager;
        private readonly IJobManager _jobManager;
        private readonly IJobTracker _jobTracker;
        private readonly IShareManager _shareManager;
        private readonly IStorage _storage;
        private readonly IMiningServer _miningServer;
        private readonly IRpcService _rpcService;
        private readonly IPaymentProcessor _paymentProcessor;
        private readonly IStatistics _statistics;
        private readonly IVardiffManager _vardiffManager;
        private readonly IBanManager _banManager;

        /// <summary>
        /// Initialize mock objects.
        /// </summary>
        public PoolTests()
        {
            _jobManagerFactory = Substitute.For<IJobManagerFactory>();
            _jobTrackerFactory = Substitute.For<IJobTrackerFactory>();
            _hashAlgorithmFactory = Substitute.For<IHashAlgorithmFactory>();
            _shareManagerFactory = Substitute.For<IShareManagerFactory>();
            _minerManagerFactory = Substitute.For<IMinerManagerFactory>();
            _serverFactory = Substitute.For<IServerFactory>();
            _serviceFactory = Substitute.For<IServiceFactory>();
            _storageFactory = Substitute.For<IStorageFactory>();
            _paymentProcessorFactory = Substitute.For<IPaymentProcessorFactory>();
            _statisticsObjectFactory = Substitute.For<IStatisticsObjectFactory>();
            _vardiffManagerFactory = Substitute.For<IVardiffManagerFactory>();
            _banManagerFactory = Substitute.For<IBanManagerFactory>();

            _daemonClient = Substitute.For<IDaemonClient>();
            _minerManager = Substitute.For<IMinerManager>();
            _jobManager = Substitute.For<IJobManager>();
            _jobTracker = Substitute.For<IJobTracker>();
            _shareManager = Substitute.For<IShareManager>();
            _miningServer = Substitute.For<IMiningServer>();
            _rpcService = Substitute.For<IRpcService>();
            _storage = Substitute.For<IStorage>();
            _paymentProcessor = Substitute.For<IPaymentProcessor>();
            _statistics = Substitute.For<IStatistics>();
            _vardiffManager = Substitute.For<IVardiffManager>();
            _banManager = Substitute.For<IBanManager>();
        }

        /// <summary>
        /// Tests pool constructor with all valid parameters. Should succeed.
        /// </summary>
        [Fact]
        public void ConstructorTest_NonNullParams_ShouldSucceed()
        {
            var pool = new Pool(
                _hashAlgorithmFactory,
                _serverFactory,
                _serviceFactory,
                _daemonClient,
                _minerManagerFactory,
                _jobTrackerFactory,
                _jobManagerFactory,
                _shareManagerFactory,
                _storageFactory,
                _paymentProcessorFactory,
                _statisticsObjectFactory,
                _vardiffManagerFactory,
                _banManagerFactory);

            pool.Should().Not.Be.Null();
            pool.InstanceId.Should().Be.GreaterThan((UInt32)0);
        }
  
        /// <summary>
        /// Initializes pool with all valid parameters, should succeed.
        /// </summary>
        [Fact]
        public void InitializationTest_NonNullParams_ShouldSuccess()
        {
            var pool = new Pool(
                _hashAlgorithmFactory, 
                _serverFactory, 
                _serviceFactory, 
                _daemonClient,
                _minerManagerFactory,
                _jobTrackerFactory,
                _jobManagerFactory, 
                _shareManagerFactory,
                _storageFactory,
                _paymentProcessorFactory,
                _statisticsObjectFactory,
                _vardiffManagerFactory,
                _banManagerFactory);

            pool.Should().Not.Be.Null();
            pool.InstanceId.Should().Be.GreaterThan((UInt32)0);

            // pool-config mockup.
            var poolConfig = Substitute.For<IPoolConfig>();
            poolConfig.Daemon.Valid.Returns(true);

            // initialize hash algorithm
            var hashAlgorithm = Substitute.For<IHashAlgorithm>();
            _hashAlgorithmFactory.Get(poolConfig.Coin.Algorithm).Returns(hashAlgorithm);

            // initialize the miner manager.
            _minerManagerFactory.Get(_daemonClient);

            var walletConfig = Substitute.For<IWalletConfig>();
            var rewardsConfig = Substitute.For<IRewardsConfig>();

            // payment processor            
            _paymentProcessorFactory.Get(_daemonClient, _storage, walletConfig);

            // initialize storage manager
            _storageFactory.Get(Storages.Redis, poolConfig);

            // initialize the job tracker
            _jobTrackerFactory.Get();

            // initialize share manager.
            _shareManagerFactory.Get(_daemonClient, _jobTracker, _storage).Returns(_shareManager);

            // vardiff manager
            var vardiffConfig = Substitute.For<IVardiffConfig>();
            _vardiffManagerFactory.Get(vardiffConfig, _shareManager);

            // banning manager
            var banConfig = Substitute.For<IBanConfig>();
            _banManagerFactory.Get(banConfig, _shareManager);

            // initalize job manager.
            _jobManagerFactory.Get(_daemonClient, _jobTracker, _shareManager, _minerManager, hashAlgorithm, walletConfig,rewardsConfig).Returns(_jobManager);
            _jobManager.Initialize(pool.InstanceId);
        
            // init daemon client
            _daemonClient.Initialize(poolConfig.Daemon);
            _daemonClient.GetInfo().Returns(new Info());
            _daemonClient.GetMiningInfo().Returns(new MiningInfo());

            // init server
            _serverFactory.Get(Services.Stratum, pool, _minerManager, _jobManager,_banManager).Returns(_miningServer);

            // init service
            _serviceFactory.Get(Services.Stratum, poolConfig.Coin, _shareManager, _daemonClient).Returns(_rpcService);

            // initalize the server.
            _miningServer.Initialize(poolConfig.Stratum);

            // initialize the pool.
            pool.Initialize(poolConfig);
        }
    }
}
