#r "../_lib/Fornax.Core.dll"
#r "../_lib/Fornax.Seo.dll"

#load "layout.fsx"

open Html
open Fornax.Seo

let private toUrl (file: string) =
    let ext = System.IO.Path.GetExtension file
    file.Replace(ext, ".html")

let private generate' (ctx: SiteContents) (page: string) =
    let normalPath = page.ToLower()
    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo>()
    let siteAuthor = siteInfo |> Option.map (fun si -> si.author) |> Option.defaultValue ContentCreator.Default
    let baseUrl = siteInfo |> Option.map (fun si -> si.canonical) |> Option.defaultValue ""
    let headline = siteInfo |> Option.map (fun si -> si.headline) |> Option.defaultValue ""

    let post =
        ctx.TryGetValues<Postloader.Post>()
        |> Option.defaultValue Seq.empty
        |> Seq.tryFind (fun n -> n.file.ToLower() = normalPath)

    post |> function
    | None -> html [] [ head [] [ string "" ]; body [] [] ]
    | Some post ->
        let siteName =
            siteInfo |> Option.map (fun si -> si.title)
        let desc =
            siteInfo
            |> Option.map (fun si -> si.description)
            |> Option.defaultValue post.summary

        let postMeta =
            { Title = post.title
              BaseUrl = baseUrl
              Url = toUrl post.file
              Description = desc
              Author = { siteAuthor with Name = defaultArg post.author siteAuthor.Name }
              SiteName = siteInfo |> Option.map (fun si -> si.title)
              Headline = Some post.summary
              ObjectType = Some "Blog"
              ContentType = Some "BlogPosting"
              OpenGraphType = Some "article"
              Locale = Some "en-ca"
              Published = post.published
              Modified = post.updated
              Tags = Some post.tags
              Meta =
                  Some [ ("Image", defaultArg post.image $"{baseUrl}/images/people-notes-meeting-team-min.jpg")
                         ("Publisher", defaultArg siteName siteAuthor.Name) ] }

        ctx.Add(postMeta)

        Layout.layout ctx post.title
            [ section [ Class "hero is-info is-medium is-bold" ] [
                div [ Class "hero-body" ] [
                    div [ Class "container has-text-centered" ] [
                        h1 [ Class "title" ] [ !! headline ]
                    ]
                ]
              ]
              div [ Class "container" ] [
                  section [ Class "articles" ] [
                      div [ Class "column is-8 is-offset-2" ] [
                          Layout.postLayout false post
                      ]
                  ]
              ] ]

let generate (ctx: SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page |> Layout.render ctx
