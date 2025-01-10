namespace FileService.Domain.Entities;

public class File
{
    public Guid Id { get; private set; }
    public string BlobContainerName { get; private set; }
    public long Size { get; private set; }
    public string OwnerId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private File(Guid id, string blobContainerName, long size, string ownerId)
    {
        Id = id;
        BlobContainerName = blobContainerName;
        Size = size;
        OwnerId = ownerId;
    }

    public static File CreateFile(Guid id, string blobContainerName, long size, string ownerId)
    {
        return new File(id, blobContainerName, size, ownerId);
    }
}