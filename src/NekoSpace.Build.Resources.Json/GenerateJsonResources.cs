using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NekoSpace.Build.Resources.Json
{
    public class GenerateJsonResources : Task
    {
        [Required]
        public ITaskItem[] Sources { get; set; }

        [Required]
        public string OutputPath { get; set; }

        [Required]
        public string ProjectRootNamespace { get; set; }

        [Output]
        public ITaskItem[] OutputResources => _outputResources.ToArray();

        private readonly List<ITaskItem> _outputResources = new List<ITaskItem>();

        public override bool Execute()
        {
            try
            {
                Directory.CreateDirectory(OutputPath);

                foreach (var inputItem in Sources)
                {
                    if (inputItem.ItemSpec == null)
                        continue;

                    // eg. "ExampleResource.zh-Hans" or "ExampleResource"
                    var filenameWithCulture = Path.GetFileNameWithoutExtension(inputItem.ItemSpec);

                    var resFilename = $"{filenameWithCulture}.resources";
                    var resLogicalName = GetResourceLogicalName(inputItem.ItemSpec, resFilename);

                    var writer = new ResourceWriter(Path.Combine(OutputPath, resLogicalName));

                    var ok = true;

                    bool ReadElement(string currentName, JsonElement element)
                    {
                        switch (element.ValueKind)
                        {
                            case JsonValueKind.Array:
                                Log.LogError($"Cannot accept array value in {inputItem.ItemSpec}.");
                                return false;

                            case JsonValueKind.Object:
                                var dict = element.Deserialize<Dictionary<string, JsonElement>>();
                                foreach (var kv in dict)
                                {
                                    var newName = currentName == "" ? kv.Key : $"{currentName}:{kv.Key}";
                                    ok = ok && ReadElement(newName, kv.Value);
                                }

                                return true;

                            default:
                                writer.AddResource(currentName, element.ToString());
                                return true;
                        }
                    }

                    var document = JsonDocument.Parse(File.ReadAllText(inputItem.ItemSpec));
                    ReadElement("", document.RootElement);

                    writer.Close();

                    if (!ok) return false;

                    var outputItem = new TaskItem();

                    // Example JsonResource:
                    //   <JsonResource Include="Resources\ExampleResource.json" />
                    // We should set this metadata, otherwise the metadata resource name will be something like:
                    //   "UsingJsonResource.obj.Debug.net8._0.Resources.ExampleResource.resources"
                    // After correction, the metadata resource name will be:
                    //   "UsingJsonResource.Resources.ExampleResource.resource"
                    // That's the default behavior of .resx files.
                    outputItem.SetMetadata("LogicalName", resLogicalName);

                    // Copy all the input metadata to output
                    inputItem.CopyMetadataTo(outputItem);

                    outputItem.ItemSpec = resLogicalName;

                    _outputResources.Add(outputItem);
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }

            return true;
        }

        private static readonly Regex SpaceRegex = new Regex(@"\s+");

        private string GetResourceLogicalName(string resItemSpec, string resFilename)
        {
            var dir = Path.GetDirectoryName(resItemSpec);
            if (dir == null) return resFilename;
            var dirParts = dir
                .Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar },
                    StringSplitOptions.RemoveEmptyEntries)
                .Select(part => SpaceRegex.Replace(part, ""));

            var builder = new StringBuilder();
            builder.Append($"{ProjectRootNamespace}.");
            foreach (var part in dirParts)
                builder.Append($"{part}.");
            builder.Append(resFilename);

            return builder.ToString();
        }
    }
}