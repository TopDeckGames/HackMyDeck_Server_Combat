﻿using System;
using MySql.Data.MySqlClient;

namespace CombatServer.Data
{
    public abstract class BaseAdapter
    {
        protected string connectionString;
        protected MySqlConnection connection;

        public BaseAdapter()
        {
            this.connectionString = "SERVER=localhost; DATABASE=test; UID=root";
            this.connection = new MySqlConnection(this.connectionString);
        }
    }
}