#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let private generate' (ctx: SiteContents) (page: string) =
    let normalPath = page.ToLower()

    let post =
        ctx.TryGetValues<Postloader.Post>()
        |> Option.defaultValue Seq.empty
        |> Seq.tryFind (fun n -> n.file.ToLower() = normalPath)

    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo>()

    let headline =
        siteInfo
        |> Option.map (fun si -> si.headline)
        |> Option.defaultValue !! ""

    post |> function
    | None -> html [] [ head [] [ string "" ]; body [] [] ]
    | Some post ->
        Layout.layout ctx post.title
            [ section [ Class "hero is-info is-medium is-bold" ] [
                div [ Class "hero-body" ] [
                    div [ Class "container has-text-centered" ] [
                        h1 [ Class "title" ] [ headline ]
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
