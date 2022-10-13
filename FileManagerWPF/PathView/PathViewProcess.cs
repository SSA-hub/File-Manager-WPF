using System.Collections.Generic;

namespace FileManagerWPF.PathProcessInfoAndOther
{
    public static class PathViewProcess
    {
        public static PathViewRepository GetPathViewRepository(List<string> path)
        {
            var repository = new PathViewRepository();
            for (var i = 0; i < path.Count; i++)
            {
                if (path[i] != null && path[i] != "")
                {
                    var pathItem = new PathViewContent();
                    pathItem.FolderName = path[i];
                    repository.Content.Add(pathItem);
                }
            }
            return repository;
        }
    }
}
