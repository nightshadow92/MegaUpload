using CG.Web.MegaApiClient;
using System.Text.Json;
using System.CommandLine.DragonFruit;
using System.Text.Json.Serialization;
class Program {
    /// <summary>
    /// A simple program that uploads a provided file to mega.nz
    /// </summary>
    /// <param name="File">The full file path to the file to be uploaded to Mega.nz</param>
    static void Main (string? File = null) {
        if (File != null) {
            string? authjson = System.Environment.GetEnvironmentVariable("MEGA_AUTH");
            var login = JsonSerializer.Deserialize<MegaApiClient.AuthInfos>(authjson);
            MegaApiClient client = new MegaApiClient();
            client.Login(login);

            IEnumerable<INode> nodes = client.GetNodes();
            INode node = nodes.Single(x => x.Type == NodeType.Root);
            client.UploadFile(File, node);

            client.Logout();
        }
        else {
            Console.WriteLine("Please provide a valid path to a file");
        }
    }
    void DisplayNodesRecursive (IEnumerable<INode> nodes, INode parent, int level = 0) {
        IEnumerable<INode> children = nodes.Where(x => x.ParentId == parent.Id);
        foreach (INode child in children) {
            Console.WriteLine(child.Name);
            if (child.Type == NodeType.Directory) {
                DisplayNodesRecursive(nodes, child, level + 1);
            }
        }
    }
}
