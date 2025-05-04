using System.Security.Cryptography;

try
{
    // Define the path for the Strong Name Key file (in the root directory)
    string keyFilePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "WebSpark.snk"));

    Console.WriteLine($"Generating Strong Name Key file at: {keyFilePath}");

    // Create a new key pair
    using (var csp = new RSACryptoServiceProvider(2048))
    {
        // Export the key pair to SNK format
        byte[] keyPair = csp.ExportCspBlob(true);

        // Write the key pair to the SNK file
        File.WriteAllBytes(keyFilePath, keyPair);
    }

    Console.WriteLine("Strong Name Key file (WebSpark.snk) has been generated successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error generating Strong Name Key: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
