namespace CodeShell.Files
{
    public class TmpFileData
    {
        public string Url { get; set; }
        public string TmpPath { get; set; }
        public int Size { get; set; }
        public string Name { get; set; }
        public string UploadId { get; set; }

        public FileData ToFileData(string fieldName)
        {
            return new FileData
            {
                FieldName = fieldName,
                FileName = Name,
                FullPath = TmpPath
            };
        }

    }
}
