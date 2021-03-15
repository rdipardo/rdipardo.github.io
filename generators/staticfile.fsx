#r "../_lib/Fornax.Core.dll"

open System
open System.Diagnostics
open System.IO
open System.Text

let private reportException (exc: Exception) =
    sprintf "%s%s: %s\n" <| exc.GetType().Name <| exc.Message
    |> Console.WriteLine

let private minify (scriptPath: string) (resource: string) =
    let mutable result = String.Empty

    try
        use p : Process = new Process()
        p.StartInfo.UseShellExecute <- false
        p.StartInfo.RedirectStandardInput <- true
        p.StartInfo.RedirectStandardOutput <- true
        p.StartInfo.FileName <- "node"
        p.StartInfo.Arguments <- sprintf "%s %s" scriptPath resource
        p.Start() |> ignore
        p.WaitForExit()
        result <- p.StandardOutput.ReadToEnd()
    with exc -> reportException (exc)

    result

let generate (ctx: SiteContents) (projectRoot: string) (page: string) =
    let inputPath = Path.Combine(projectRoot, page)

    let scriptPath =
        Path.Combine(projectRoot, "scripts/minify.js")

    let minified = minify scriptPath inputPath

    match minified.Trim() with
    | "" -> File.ReadAllBytes inputPath
    | _ -> Encoding.UTF8.GetBytes(minified.Trim())
