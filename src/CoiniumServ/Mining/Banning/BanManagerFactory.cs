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

using CoiniumServ.Mining.Pools.Config;
using CoiniumServ.Mining.Shares;
using CoiniumServ.Repository.Context;
using Nancy.TinyIoc;

namespace CoiniumServ.Mining.Banning
{
    public class BanManagerFactory : IBanManagerFactory
    {
        /// <summary>
        /// The _kernel
        /// </summary>
        private readonly IApplicationContext _applicationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShareManagerFactory" /> class.
        /// </summary>
        /// <param name="applicationContext">The application context.</param>
        public BanManagerFactory(IApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IBanManager Get(IBanConfig banConfig, IShareManager shareManager)
        {
            var @params = new NamedParameterOverloads
            {
                {"banConfig", banConfig},
                {"shareManager", shareManager},
            };

            return _applicationContext.Container.Resolve<IBanManager>(@params);
        }
    }
}
