#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let private generate' (ctx: SiteContents) (page: string) =
    let normalPath = page.ToLower()

    let doc =
        ctx.TryGetValues<Pageloader.Page>()
        |> Option.defaultValue Seq.empty
        |> Seq.tryFind (fun p -> p.file.ToLower() = normalPath)

    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo>()

    let headline =
        siteInfo
        |> Option.map (fun si -> si.headline)
        |> Option.defaultValue ""

    doc |> function
    | None -> html [] [ head [] [ string "" ]; body [] [] ]
    | Some doc ->
        Layout.layout
            ctx
            doc.title
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
                          div [ Class "card article" ] [
                              div [ Class "card-content" ] [
                                  div [ Class "content article-body" ] [
                                      !!doc.content
                                  ]
                              ]
                          ]
                      ]
                  ]
              ] ]

let generate (ctx: SiteContents) (projectRoot: string) (page: string) =
    generate' ctx page |> Layout.render ctx
