
namespace Patterns.Executors
{
    public abstract class Executor : IExecutor
    {

        protected virtual void  PreExecute() { InExecute(); }
        protected virtual void InExecute() { PostExecute(); }
        protected virtual void PostExecute() { }

        public void Execute()
        {
            PreExecute();
        }
    }
}
