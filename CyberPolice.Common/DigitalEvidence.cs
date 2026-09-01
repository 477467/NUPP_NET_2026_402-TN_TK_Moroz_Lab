using System;

namespace CyberPolice.Common
{
    public class DigitalEvidence
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string HashSha256 { get; set; }
        public long SizeBytes { get; set; }

        public DigitalEvidence(string fileName, string hash, long sizeBytes)
        {
            Id = Guid.NewGuid();
            FileName = fileName;
            HashSha256 = hash;
            SizeBytes = sizeBytes;
        }
    }
}