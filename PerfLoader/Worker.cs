#region Copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using PerfRunner.V1;

namespace PerfLoader
{
    #region snippet_Worker
    public class Worker : BackgroundService
    {
        private readonly Perf.PerfClient _client;
        private readonly IGreetRepository _greetRepository;

        public Worker(Perf.PerfClient client, IGreetRepository greetRepository)
        {
            _client = client;
            _greetRepository = greetRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var count = 0;
            while (!stoppingToken.IsCancellationRequested)
            {
                count++;

                var reply = await _client.PingAsync(
                    new PingRequest { Name = $"Worker {count}" });

                _greetRepository.SaveGreeting(reply.Message);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
    #endregion
}
