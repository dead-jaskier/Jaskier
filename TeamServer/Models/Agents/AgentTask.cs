﻿namespace TeamServer.Models
{
    public class AgentTask
    {
        public string Id { get; set; }
        public string Command { get; set; }
        public string[] Arguments { get; set; }
        public byte[] File { get; set; }
    }
}
