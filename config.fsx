#r "_lib/Fornax.Core.dll"

open Config
open System
open System.IO

let pagePredicate (projectRoot: string, page: string) =
    let ext = Path.GetExtension page
    page.StartsWith("pages") && List.contains ext [".md"; ".markdown"]

let postPredicate (projectRoot: string, page: string) =
    let ext = Path.GetExtension page
    page.StartsWith("posts") && List.contains ext [".md"; ".markdown"]

let staticPredicate (projectRoot: string, page: string) =
    let ext = Path.GetExtension page
    let path = page.ToLower()

    (path.Contains "license" ||
    path.Contains "_public" ||
    path.Contains "_bin" ||
    path.Contains "obj" ||
    path.Contains "_lib" ||
    path.Contains "_data" ||
    path.Contains "_settings" ||
    path.Contains "node_modules" ||
    path.Contains "scripts" ||
    path.Contains ".sass-cache" ||
    path.Contains ".config" ||
    path.Contains ".git" ||
    path.Contains ".ionide" ||
    List.contains ext [ ".fsx"; ".fsproj"; ".json"; ".md"; ".markdown"; ".rst"; ".yml" ] ||
    String.IsNullOrEmpty(Path.GetFileNameWithoutExtension page) &&
    not (List.contains ext [ ".htaccess"; ".nojekyll" ])) |> not

let config = {
    Generators = [
        {Script = "page.fsx"; Trigger = OnFilePredicate pagePredicate; OutputFile = ChangeExtension "html" }
        {Script = "post.fsx"; Trigger = OnFilePredicate postPredicate; OutputFile = ChangeExtension "html" }
        {Script = "staticfile.fsx"; Trigger = OnFilePredicate staticPredicate; OutputFile = SameFileName }
        {Script = "index.fsx"; Trigger = Once; OutputFile = MultipleFiles id }
    ]
}
