﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CaseOnline.Azure.WebJobs.Extensions.Mqtt.Listeners;
using CaseOnline.Azure.WebJobs.Extensions.Mqtt.Messaging;
using Microsoft.Azure.WebJobs;
using MQTTnet;

namespace CaseOnline.Azure.WebJobs.Extensions.Mqtt.Config
{
    public class MqttMessageCollector : IAsyncCollector<IMqttMessage>
    {
        private readonly MqttConnection _mqttConnection;

        public MqttMessageCollector(MqttAttribute attr, MqttConnection mqttConnection)
        {
            _mqttConnection = mqttConnection;
        }

        public async Task AddAsync(IMqttMessage item, CancellationToken cancellationToken = default(CancellationToken))
        {
            var qos = (MQTTnet.Protocol.MqttQualityOfServiceLevel)Enum.Parse(typeof(MQTTnet.Protocol.MqttQualityOfServiceLevel), item.QosLevel.ToString());
            var mqttApplicationMessage = new MqttApplicationMessage(item.Topic, item.GetMessage(), qos, item.Retain);
            await _mqttConnection.PublishAsync(mqttApplicationMessage).ConfigureAwait(false);
        }

        public Task FlushAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}