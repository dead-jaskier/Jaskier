using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using TeamServer.Models.Agents;

namespace TeamServer.Models
{
    public class Agent
    {
        public AgentMetaData Metadata { get; set; }
        public DateTime LastSeen { get; private set; }

        private readonly ConcurrentQueue<AgentTask> _pendingTasks = new();

        public Agent(AgentMetaData metadata)
        {
            Metadata = metadata;
        }

        public void CheckIn()
        {
            LastSeen = DateTime.Now;
        }

        public void QueueTask(AgentTask task)
        {
            _pendingTasks.Enqueue(task);
        }

        public IEnumerable<AgentTask> GetPendingTasks()
        {
            List<AgentTask> tasks = new();
            while (_pendingTasks.TryDequeue(out var task))
            {
                tasks.Add(task);
            }

            return tasks;
        }
    }
}
