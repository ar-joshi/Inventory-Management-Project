
namespace Inventory_Management.Helpers
{
    public interface ILogHelper
    {
        void Error(string message, string arg = null);
        void Info(string message, string arg = null);
    }
}