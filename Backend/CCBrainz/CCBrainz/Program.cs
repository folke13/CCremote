﻿using System;
using System.Threading.Tasks;

namespace CCBrainz
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {

            await Task.Delay();
        }
    }
}