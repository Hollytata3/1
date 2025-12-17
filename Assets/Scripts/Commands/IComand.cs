

namespace Xianxiao
{
    public interface IComand
    {
        bool CanHandle(CommandContext context);
        void Handle(CommandContext context);
    }
}
