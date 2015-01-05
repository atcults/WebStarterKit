using StructureMap.Configuration.DSL;

namespace Common
{
    public class CommonRegistry : Registry
    {
        public CommonRegistry()
        {
            Scan(x =>
            {
              
            });
        }
    }
}