namespace TextToSpeechWPF
{
    public class FileOperationModel
    {
        public string FileName { get; private set; } 

        public FileOperationModel(string filePath)
        {
            FileName = filePath;
        }

        public void ChangeFileName(string fileName)
        {
            FileName = fileName;
        }

        public void MoveMp4()
        {
        }

        public void DeleteMp4()
        {
        }
    }
}
