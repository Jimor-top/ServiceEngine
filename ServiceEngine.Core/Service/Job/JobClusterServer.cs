namespace ServiceEngine.Core.Service;

/// <summary>
/// 作业集群控制
/// </summary>
public class JobClusterServer : IJobClusterServer
{
    private readonly Random rd = new(DateTime.Now.Millisecond);

    public JobClusterServer()
    {
    }

    /// <summary>
    /// 当前作业调度器启动通知
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public async void Start(JobClusterContext context)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，如果 clusterId 不存在，则新增一条（否则更新一条），并设置 status 为 ClusterStatus.Waiting
        if (await _sysJobClusterRep.IsAnyAsync(u => u.ClusterId == context.ClusterId))
        {
            await _sysJobClusterRep.AsUpdateable().SetColumns(u => u.Status == ClusterStatus.Waiting).Where(u => u.ClusterId == context.ClusterId).ExecuteCommandAsync();
        }
        else
        {
            await _sysJobClusterRep.AsInsertable(new SysJobCluster { ClusterId = context.ClusterId, Status = ClusterStatus.Waiting }).ExecuteCommandAsync();
        }
    }

    /// <summary>
    /// 等待被唤醒
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    /// <returns><see cref="Task"/></returns>
    public async Task WaitingForAsync(JobClusterContext context)
    {
        var clusterId = context.ClusterId;

        while (true)
        {
            // 控制集群心跳频率（放在头部为了防止 IsAnyAsync continue 没sleep占用大量IO和CPU）
            await Task.Delay(3000 + rd.Next(500, 1000)); // 错开集群同时启动

            try
            {
                ICache _cache = App.GetService<ICache>();
                //使用分布式锁
                using (_cache.AcquireLock("lock:JobClusterServer:WaitingForAsync", 1000))
                {
                    var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
                    // 在这里查询数据库，根据以下两种情况处理
                    // 1) 如果作业集群表已有 status 为 ClusterStatus.Working 则继续循环
                    // 2) 如果作业集群表中还没有其他服务或只有自己，则插入一条集群服务或调用 await WorkNowAsync(clusterId); 之后 return;
                    // 3) 如果作业集群表中没有 status 为 ClusterStatus.Working 的，调用 await WorkNowAsync(clusterId); 之后 return;
                    if (await _sysJobClusterRep.IsAnyAsync(u => u.Status == ClusterStatus.Working))
                        continue;

                    await WorkNowAsync(clusterId);
                    return;
                }
            }
            catch { }
        }
    }

    /// <summary>
    /// 当前作业调度器停止通知
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public async void Stop(JobClusterContext context)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Crashed
        await _sysJobClusterRep.UpdateSetColumnsTrueAsync(u => new SysJobCluster { Status = ClusterStatus.Crashed }, u => u.ClusterId == context.ClusterId);
    }

    /// <summary>
    /// 当前作业调度器宕机
    /// </summary>
    /// <param name="context">作业集群服务上下文</param>
    public async void Crash(JobClusterContext context)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Crashed
        await _sysJobClusterRep.UpdateSetColumnsTrueAsync(u => new SysJobCluster { Status = ClusterStatus.Crashed }, u => u.ClusterId == context.ClusterId);
    }

    /// <summary>
    /// 指示集群可以工作
    /// </summary>
    /// <param name="clusterId">集群 Id</param>
    /// <returns></returns>
    private async Task WorkNowAsync(string clusterId)
    {
        var _sysJobClusterRep = App.GetService<SqlSugarRepository<SysJobCluster>>();
        // 在作业集群表中，更新 clusterId 的 status 为 ClusterStatus.Working
        await _sysJobClusterRep.UpdateSetColumnsTrueAsync(u => new SysJobCluster { Status = ClusterStatus.Working }, u => u.ClusterId == clusterId);
    }
}