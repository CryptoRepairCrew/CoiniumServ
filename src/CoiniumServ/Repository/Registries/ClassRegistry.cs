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

using CoiniumServ.Crypto.Algorithms;
using CoiniumServ.Daemon;
using CoiniumServ.Mining.Jobs.Tracker;
using CoiniumServ.Mining.Pools;
using CoiniumServ.Mining.Pools.Config;
using CoiniumServ.Mining.Pools.Statistics;
using CoiniumServ.Payments;
using CoiniumServ.Persistance;
using CoiniumServ.Persistance.Redis;
using CoiniumServ.Repository.Context;

namespace CoiniumServ.Repository.Registries
{
    public class ClassRegistry : IRegistry
    {
        private readonly IApplicationContext _applicationContext;

        public ClassRegistry(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void RegisterInstances()
        {
            // hash algorithms
            _applicationContext.Container.Register<IHashAlgorithm, Blake>(Crypto.Algorithms.Algorithms.Blake).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Fugue>(Crypto.Algorithms.Algorithms.Fugue).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Groestl>(Crypto.Algorithms.Algorithms.Groestl).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Keccak>(Crypto.Algorithms.Algorithms.Keccak).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Scrypt>(Crypto.Algorithms.Algorithms.Scrypt).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Sha256>(Crypto.Algorithms.Algorithms.Sha256).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Shavite3>(Crypto.Algorithms.Algorithms.Shavite3).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, Skein>(Crypto.Algorithms.Algorithms.Skein).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, X11>(Crypto.Algorithms.Algorithms.X11).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, X13>(Crypto.Algorithms.Algorithms.X13).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, X15>(Crypto.Algorithms.Algorithms.X15).AsSingleton();
            _applicationContext.Container.Register<IHashAlgorithm, X17>(Crypto.Algorithms.Algorithms.X17).AsSingleton();

            _applicationContext.Container.Register<IDaemonClient, DaemonClient>().AsMultiInstance();
            _applicationContext.Container.Register<IPool, Pool>().AsMultiInstance();
            _applicationContext.Container.Register<IPoolConfig, PoolConfig>().AsMultiInstance();
            _applicationContext.Container.Register<IStorage, Redis>(Storages.Redis).AsMultiInstance();
            _applicationContext.Container.Register<IJobTracker, JobTracker>().AsMultiInstance();
            _applicationContext.Container.Register<IPaymentProcessor, PaymentProcessor>().AsMultiInstance();
            _applicationContext.Container.Register<IStatistics, Statistics>().AsSingleton();
            _applicationContext.Container.Register<IPools, Pools>().AsSingleton();            
            _applicationContext.Container.Register<IPerPool, PerPool>().AsMultiInstance();
            _applicationContext.Container.Register<IBlocks, Blocks>().AsMultiInstance();
            _applicationContext.Container.Register<ILatestBlocks, LatestBlocks>().AsMultiInstance();
            _applicationContext.Container.Register<IGlobal, Global>().AsSingleton();
            _applicationContext.Container.Register<IAlgorithms, Mining.Pools.Statistics.Algorithms>().AsSingleton();
        }
    }
}