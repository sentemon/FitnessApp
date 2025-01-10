namespace FileService.Domain.Entities;

public class File
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string BlobName { get; private set; }
    public long Size { get; private set; }
    public string OwnerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private File(string name, string blobName, long size, string ownerId)
    {
        Name = name;
        BlobName = blobName;
        Size = size;
        OwnerId = ownerId;
    }

    public static File CreateFile(string name, string blobName, long size, string ownerId)
    {
        return new File(name, blobName, size, ownerId);
    }
}