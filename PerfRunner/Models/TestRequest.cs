// <copyright file="TestRequest.cs" company="Garden Systems"
//   Copyright (c) Garden Systems. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
// using PerfRunner.V1.Perf.PerfBase;

namespace PerfRunner.V1
{
  using System.Diagnostics;
  using PerfRunner.Services;
  using PerfRunner.Tests;

  /// <summary>
  /// Extend the protobuf type to add some internal fields.
  /// </summary>
  public partial class TestRequest
  {
    private IList<IActionRunner<ITestBase>> _actionRunners = new List<IActionRunner<ITestBase>>();

    private Stopwatch _stopWatch;

    /// <summary>
    /// The <see cref="TestStateManager"/> the test should run with.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; set; }

    public Stopwatch Stopwatch { get { return _stopWatch; } set { _stopWatch = value; } }

    public IList<IActionRunner<ITestBase>> ActionRunners { get => _actionRunners; }

    //return the first action
    public IActionRunner<ITestBase>? GetActionRunner(string guid) => ActionRunners.First(action => action.Guid.ToString().
       Equals(guid));

    public string Guid_ { get => this.Guid; }

    public bool CheckTestDurationElapsed()
    {
      if (_stopWatch.Elapsed.TotalSeconds < this.Duration.Seconds)
      {
        return false;
      }

      return true;
    }
  }
}
