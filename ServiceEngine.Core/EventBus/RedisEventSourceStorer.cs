﻿using System.Threading.Channels;

namespace ServiceEngine.Core;

/// <summary>
/// Redis自定义事件源存储器
/// </summary>
public sealed class RedisEventSourceStorer : IEventSourceStorer, IDisposable
{
    /// <summary>
    /// 消费者
    /// </summary>
    private readonly EventConsumer<ChannelEventSource> _eventConsumer;

    /// <summary>
    /// 内存通道事件源存储器
    /// </summary>
    private readonly Channel<IEventSource> _channel;

    /// <summary>
    /// Redis 连接对象
    /// </summary>
    private readonly FullRedis _redis;

    /// <summary>
    /// 路由键
    /// </summary>
    private readonly string _routeKey;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="redis">Redis 连接对象</param>
    /// <param name="routeKey">路由键</param>
    /// <param name="capacity">存储器最多能够处理多少消息，超过该容量进入等待写入</param>
    public RedisEventSourceStorer(ICache redis, string routeKey, int capacity)
    {
        // 配置通道，设置超出默认容量后进入等待
        var boundedChannelOptions = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };

        // 创建有限容量通道
        _channel = Channel.CreateBounded<IEventSource>(boundedChannelOptions);

        _redis = redis as FullRedis;
        _routeKey = routeKey;

        // 创建消息订阅者
        _eventConsumer = new EventConsumer<ChannelEventSource>(_redis, _routeKey);


        // 订阅消息写入 Channel
        _eventConsumer.Received += (send, cr) =>
        {
            // 反序列化消息
            //var eventSource = JsonConvert.DeserializeObject<ChannelEventSource>(cr);

            // 写入内存管道存储器
            _channel.Writer.WriteAsync(cr);
        };

        // 启动消费者
        _eventConsumer.Start();
    }

    /// <summary>
    /// 将事件源写入存储器
    /// </summary>
    /// <param name="eventSource">事件源对象</param>
    /// <param name="cancellationToken">取消任务 Token</param>
    /// <returns><see cref="ValueTask"/></returns>
    public async ValueTask WriteAsync(IEventSource eventSource, CancellationToken cancellationToken)
    {
        // 空检查
        if (eventSource == default)
        {
            throw new ArgumentNullException(nameof(eventSource));
        }

        // 这里判断是否是 ChannelEventSource 或者 自定义的 EventSource
        if (eventSource is ChannelEventSource source)
        {
            // 序列化消息
            //var data = JsonSerializer.Serialize(source);

            // 获取一个订阅对象
            var queue = _redis.GetQueue<ChannelEventSource>(_routeKey);

            // 异步发布
            await Task.Factory.StartNew(() =>
            {

                queue.Add(source);
            }, cancellationToken, TaskCreationOptions.LongRunning, System.Threading.Tasks.TaskScheduler.Default);
        }
        else
        {
            // 这里处理动态订阅问题
            await _channel.Writer.WriteAsync(eventSource, cancellationToken);
        }
    }

    /// <summary>
    /// 从存储器中读取一条事件源
    /// </summary>
    /// <param name="cancellationToken">取消任务 Token</param>
    /// <returns>事件源对象</returns>
    public async ValueTask<IEventSource> ReadAsync(CancellationToken cancellationToken)
    {
        // 读取一条事件源
        var eventSource = await _channel.Reader.ReadAsync(cancellationToken);
        return eventSource;
    }


    /// <summary>
    /// 释放非托管资源
    /// </summary>
    public async void Dispose()
    {
        await _eventConsumer.Stop();
        GC.SuppressFinalize(this);
    }
}