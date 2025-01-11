namespace FileService.Domain.Entities;

public class File
{
    public Guid Id { get; private set; }
    public string BlobName { get; private set; }
    public string BlobContainerName { get; private set; }
    public long Size { get; private set; }
    public string OwnerId { get; private set; }
    public Guid ForeignEntityId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private File(string blobName, string blobContainerName, long size, string ownerId, Guid foreignEntityId)
    {
        BlobName = blobName;
        BlobContainerName = blobContainerName;
        Size = size;
        OwnerId = ownerId;
        ForeignEntityId = foreignEntityId;
    }

    public static File CreateFile(string blobName, string blobContainerName, long size, string ownerId, Guid foreignEntityId)
    {
        return new File(blobName, blobContainerName, size, ownerId, foreignEntityId);
    }
}