#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"
#load "../scripts/markdownUtils.fsx"

open System
open System.IO
open Markdig
open MarkdownUtils

type PostConfig = { disableLiveRefresh: bool }

type Post =
    { file: string
      link: string
      title: string
      author: string option
      published: DateTime option
      updated: DateTime option
      tags: string list
      categories: string list
      content: string
      summary: string }

let contentDir = "posts"

let markdownPipeline =
    MarkdownPipelineBuilder()
        .UsePipeTables()
        .UseGridTables()
        .Build()

let loadFile n =
    let text = File.ReadAllText n
    let config = getConfig text
    let excerpt, content = getContent text markdownPipeline

    let path =
        Path
            .Combine(contentDir, Path.GetFileNameWithoutExtension n)
            .Replace("\\", "/")

    let file = path + Path.GetExtension n
    let link = sprintf "/%s.html" path
    let title = config |> Map.find "title" |> trimString

    let author =
        config |> Map.tryFind "author" |> Option.map trimString

    let published =
        config
        |> Map.tryFind "published"
        |> Option.map (trimString >> DateTime.Parse)

    let updated =
        config
        |> Map.tryFind "updated"
        |> Option.map (trimString >> DateTime.Parse)

    let summary =
        match config |> Map.tryFind "summary" with
        | Some s -> s
        | None -> excerpt
        |> trimString

    let tags =
        let tagsOpt =
            config
            |> Map.tryFind "tags"
            |> Option.map (trimString >> fun n -> n.Split ',' |> Array.toList)

        defaultArg tagsOpt []

    let categories =
        defaultArg
        <| (config
            |> Map.tryFind "categories"
            |> Option.map (trimString >> fun n -> n.Split ' ' |> Array.toList))
        <| []

    { file = file
      link = link
      title = title
      author = author
      published = published
      updated = updated
      tags = tags
      categories = categories
      content = content
      summary = summary }

let loader (projectRoot: string) (siteContent: SiteContents) =
    let postsPath = Path.Combine(projectRoot, contentDir)

    Directory.GetFiles postsPath
    |> Array.filter (fun n -> n.EndsWith ".md")
    |> Array.map loadFile
    |> Array.iter (fun p -> siteContent.Add p)

    siteContent.Add({ disableLiveRefresh = true })
    siteContent
