// <copyright file="TestRequest.cs" company="Garden Systems"
//   Copyright (c) Garden Systems. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
// using PerfRunner.V1.Perf.PerfBase;

using System.Diagnostics;

namespace PerfRunner.V1
{
  /// <summary>
  /// Extend the protobuf type to add some internal fields.
  /// </summary>
  public sealed partial class ActionOption
  {
    private Stopwatch _stopWatch;

    public CancellationTokenSource CancellationTokenSource { get; set; }

    public Stopwatch Stopwatch { get { return _stopWatch; } set { _stopWatch = value; } }

    public bool CheckActionDurationElapsed()
    {
      if(this.Duration == null)
      {
        return false;
      }

      if (_stopWatch.Elapsed.TotalSeconds < this.Duration.Seconds)
      {
        return false;
      }

      return true;
    }
  }
}