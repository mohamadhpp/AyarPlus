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

            // Contact folder
            if (!Directory.Exists(Constant.ContactsFolder))
            {
                Directory.CreateDirectory(Constant.ContactsFolder);
            }
        }
    }
}
