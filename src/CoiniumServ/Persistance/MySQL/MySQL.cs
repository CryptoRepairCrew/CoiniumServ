#region License
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
using CoiniumServ.Payments;
using CoiniumServ.Persistance.Blocks;
using CoiniumServ.Utils.Configuration;
using CoiniumServ.Utils.Extensions;
using CoiniumServ.Utils.Helpers.Time;
using MySql;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;



namespace CoiniumServ.Persistance.MySQL
{
    public class MySQL : IStorage, IMySQL
    {
        public bool IsEnabled { get; private set; }
        public bool IsConnected { get; }

        private readonly Version _requiredMinimumVersion = new Version(2, 6);
        private readonly IMySQLConfig _MySQLConfig;
        private readonly IPoolConfig _poolConfig;
        private MySql.Data.MySqlClient.MySqlConnection _database;

        public MySQL(IGlobalConfigFactory globalConfigFactory, IPoolConfig poolConfig)
        {
            _poolConfig = poolConfig; // the pool config.
            _MySQLConfig = globalConfigFactory.GetMySQLConfig(); // read the redis config.
            IsEnabled = _MySQLConfig.Enabled;

            if (IsEnabled)
                Initialize();
        }

        private void Initialize()
        {
            // Open the MySQL Connection Here but how?
            var connectionString = string.Format("Server={0};,Uid={1};Pwd={2};Database={3};", _MySQLConfig.Host, _MySQLConfig.User, _MySQLConfig.Password, _MySQLConfig.Database);
            
            try
            {
                _database = new MySql.Data.MySqlClient.MySqlConnection();
                _database.Open();
            }
            catch (Exception ex)
            {
               
                Log.Fatal("Cannot Connect to MySQL Database.");
            }

        }

    }
      
}
