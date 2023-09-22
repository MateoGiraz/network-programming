namespace Common.Protocol;

public class ProtocolStandards
{

    public const int MaxPartSize = 32768; // 32KB 
    public const int FileDefinedLength = 8; // 8 bytes (64 bits)
    public const int SizeMessageDefinedLength = 4;
    public const int DirMessageDefinedLength = 3;
    public const int CmdMessageDefinedLength = 2;
    
    public const string LocalHostIp = "127.0.0.1";
    public const int ServerPort = 3000;
    public const int ClientPort = 0;

    public static long CalculatePartCount(long fileSize)
    {
        return (fileSize + MaxPartSize - 1) / MaxPartSize;
    }
}