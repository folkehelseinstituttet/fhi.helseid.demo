namespace dotnet_new_angular.DataProtection
{
    public class DataProtectionConfig
    {
        public DataProtectionConfig()
        {
        }

        public bool Enabled { get; set; }
        public string ConnectionString { get; set; }
        public string Schema { get; set; }
        public string TableName { get; set; }
    }
}
