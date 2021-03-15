#r "../_lib/Fornax.Core.dll"
#r "../_lib/Markdig.dll"

open Markdig

let isSeparator (input: string) = input.StartsWith "---"
let isSummarySeparator (input: string) = input.Contains "<!--more-->"
let trimString (str: string) = str.Trim().TrimEnd('"').TrimStart('"')

let getConfig (fileContent: string) =
    let fileContent = fileContent.Split '\n'
    let fileContent = fileContent |> Array.skip 1 //First line must be ---

    let indexOfSeperator =
        fileContent |> Array.findIndex isSeparator

    let splitKey (line: string) =
        let seperatorIndex = line.IndexOf(':')

        if seperatorIndex > 0 then
            let key =
                line.[..seperatorIndex - 1]
                    .Trim()
                    .ToLower()

            let value = line.[seperatorIndex + 1..].Trim()

            Some(key, value)
        else
            None

    fileContent
    |> Array.splitAt indexOfSeperator
    |> fst
    |> Seq.choose splitKey
    |> Map.ofSeq

let getContent (fileContent: string) (markdownPipeline: MarkdownPipeline) =
    let fileContent = fileContent.Split '\n'
    let fileContent = fileContent |> Array.skip 1 //First line must be ---

    let indexOfSeperator =
        fileContent |> Array.findIndex isSeparator

    let _, content =
        fileContent |> Array.splitAt indexOfSeperator

    let summary, content =
        match content |> Array.tryFindIndex isSummarySeparator with
        | Some indexOfSummary ->
            let summary, _ = content |> Array.splitAt indexOfSummary
            summary, content
        | None -> content, content

    let summary =
        summary |> Array.skip 1 |> String.concat "\n"

    let content =
        content |> Array.skip 1 |> String.concat "\n"

    Markdown.ToHtml(summary, markdownPipeline), Markdown.ToHtml(content, markdownPipeline)
