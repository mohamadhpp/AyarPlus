namespace ApiTask.WebApi
{
    public static class Constant
    {
        // Folders
        public static readonly string DataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
        public static readonly string ContractsFolder = Path.Combine(DataFolder, "Contracts");
        public static readonly string WwwRootFolder = Path.Combine(AppContext.BaseDirectory, "wwwroot");
    }
}
