
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApi.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class UploadController:ControllerBase
    {
        public static string RenameChunk(IFormFile file, string fileID, int chunkNumber, string fileName)
        {
            string chunkName = $"{fileName}_{fileID}_{chunkNumber}.blob";
            string pathFolderTemp = Path.Combine("wwwroot", "Temp");
            string pathTemp = Path.Combine(Directory.GetCurrentDirectory(), pathFolderTemp, chunkName);
            return pathTemp;
        }
        public static bool AllChunksAreHere(string fileName,int totalChunks,string fileID)
        {
            string pathTemp = Path.Combine("wwwroot", "Temp");

            int findTotal = 0;
            for(var i = 1; i<=totalChunks; i++)
            {
                string findChunk = $"{fileName}_{fileID}_{i}.blob";
                string result = Directory.GetFiles(pathTemp, findChunk+"*").FirstOrDefault();
                if(result != null)
                {
                    findTotal++;
                }
            }
            if(findTotal == totalChunks)
            {
                return true;
            }
            return false;
        }
        public static void MergeChunks(string file, string chunk)
        {
            FileStream fs1 = null;
            FileStream fs2 = null;
            try
            {
                fs1 = System.IO.File.Open(file, FileMode.Append);
                fs2 = System.IO.File.Open(chunk, FileMode.Open);
                byte[] fs2Content = new byte[fs2.Length];
                fs2.Read(fs2Content, 0, (int)fs2.Length);
                fs1.Write(fs2Content, 0, (int)fs2.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " : " + ex.StackTrace);
            }
            finally
            {
                if (fs1 != null) fs1.Close();
                if (fs2 != null) fs2.Close();
                System.IO.File.Delete(chunk);
            }
        }
        public static string AssembleFile(string fileName,string fileID,int totalChunks)
        {
            string pathTemp = Path.Combine("wwwroot", "Temp");
            string pathFile = Path.Combine("wwwroot", "Files"); 
            string name = Path.GetRandomFileName();
            for (var i = 1; i <= totalChunks; i++)
            {
                string findChunk = $"{fileName}_{fileID}_{i}.blob";
                string resultChunk = Directory.GetFiles(pathTemp, findChunk + "*").FirstOrDefault();
                MergeChunks(Path.Combine(pathFile,name), resultChunk);
            }
            return name;
        }
        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 1073741824)]
        public async Task<IActionResult> PostFormData()
        {
            var reponse = new { complete = false, fileName = "" };
            string root = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("wwwroot", "Temp"));
            var provider = new MultipartFormDataStreamProvider(root);
            try
            {
                IFormCollection request = await Request.ReadFormAsync();
                IFormFile file = request.Files.FirstOrDefault();

                //Receive Data
                int chunkNumber = int.Parse(request.FirstOrDefault(x=>x.Key== "chunkNumber").Value);
                int totalChunks = int.Parse(request.FirstOrDefault(x => x.Key == "totalChunks").Value);
                string fileName = request.FirstOrDefault(x => x.Key == "fileName").Value;
                string fileId = request.FirstOrDefault(x => x.Key == "fileId").Value;

                if(totalChunks == 1)
                {
                    string pathFolder = Path.Combine("wwwroot","Files");
                    string pathSave = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
                    string name = Path.GetRandomFileName();
                    using (var stream = System.IO.File.Create(Path.Combine(pathSave, name)))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var reponse_success = new{complete = true,fileName = name};
                    return Ok(reponse_success);
                }
                string pathTemp = RenameChunk(file, fileId, chunkNumber, fileName);
                using (var stream = System.IO.File.Create(pathTemp))
                {
                    await file.CopyToAsync(stream);
                }
                if (AllChunksAreHere(fileName, totalChunks, fileId))
                {
                    string name = AssembleFile(fileName, fileId, totalChunks);
                    var reponse_success = new { complete = true, fileName = name };
                    return Ok(reponse_success);
                }
                return Ok(reponse);
            }
            catch (Exception exp)
            {
                return BadRequest(exp);
            }
        }
    }
}