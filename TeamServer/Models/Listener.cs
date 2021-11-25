using System.Threading.Tasks;

namespace TeamServer.Models
{
    public abstract class Listener
    {
        public abstract string Name { get; }

        public abstract Task Start();
        public abstract void Stop();
    }
}
