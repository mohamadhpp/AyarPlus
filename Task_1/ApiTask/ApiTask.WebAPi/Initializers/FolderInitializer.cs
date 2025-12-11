namespace ApiTask.WebApi.Initializers
{
    public static class FolderInitializer
    {
        public static void InitFolders()
        {
            // Data folder
            if (!Directory.Exists(Constant.DataFolder))
            {
                Directory.CreateDirectory(Constant.DataFolder);
            }

            // Contract folder
            if (!Directory.Exists(Constant.ContractsFolder))
            {
                Directory.CreateDirectory(Constant.ContractsFolder);
            }
        }
    }
}
