// <copyright file="TestRequest.cs" company="Garden Systems"
//   Copyright (c) Garden Systems. All rights reserved.
// </copyright>

// ReSharper disable once CheckNamespace
// using PerfRunner.V1.Perf.PerfBase;

namespace PerfRunner.V1
{

  using PerfRunner.Services;
  using PerfRunner.Tests;

  /// <summary>
  /// Extend the protobuf type to add some internal fields.
  /// </summary>
  public sealed partial class TestRequest
  {
    /// <summary>
    /// The <see cref="TestStateManager"/> the test should run with.
    /// </summary>
    public CancellationTokenSource CancellationTokenSource { get; set; }

    private IList<IActionRunner<ITestBase>> _actionRunners = new List<IActionRunner<ITestBase>>();

    public IList<IActionRunner<ITestBase>> ActionRunners { get => _actionRunners; }

    //return the first action
    public IActionRunner<ITestBase>? GetActionRunner(string guid) => ActionRunners.First(action => action.Guid.ToString().
       Equals(guid));

    public string Guid_ { get => this.Guid; }
  }
}
