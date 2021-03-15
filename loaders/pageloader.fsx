#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"
#load "../scripts/markdownUtils.fsx"

open System.IO
open Markdig
open MarkdownUtils

type Page =
    { file: string
      title: string
      link: string
      content: string }

let contentDir = "pages"
let markdownPipeline = MarkdownPipelineBuilder().Build()

let loadFile n =
    let text = File.ReadAllText n

    let path =
        Path
            .Combine(contentDir, Path.GetFileNameWithoutExtension n)
            .Replace("\\", "/")

    let file = path + Path.GetExtension n
    let config = getConfig text
    let _, content = getContent text markdownPipeline
    let link = sprintf "/%s.html" path
    let title = config |> Map.find "title" |> trimString

    { file = file
      title = title
      link = link
      content = content }

let loader (projectRoot: string) (siteContent: SiteContents) =
    let contentPath = Path.Combine(projectRoot, contentDir)

    Directory.GetFiles contentPath
    |> Array.filter (fun n -> n.EndsWith ".md")
    |> Array.map loadFile
    |> Array.append [| { title = "Blog"; link = "/"; content = ""; file = "" } |]
    |> Array.iter (fun p -> siteContent.Add p)

    siteContent
