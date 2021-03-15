#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let private generate' (ctx: SiteContents) (_: string) =
    let posts =
        ctx.TryGetValues<Postloader.Post>()
        |> Option.defaultValue Seq.empty

    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo>()

    let postPageSize =
        siteInfo
        |> Option.map (fun si -> si.postPageSize)
        |> Option.defaultValue 10

    let headline =
        siteInfo
        |> Option.map (fun si -> si.headline)
        |> Option.defaultValue !! ""

    let blogs =
        posts
        |> Seq.sortByDescending (Layout.publications >> fst)
        |> Seq.toList
        |> List.chunkBySize postPageSize
        |> List.map (List.map (Layout.postLayout true))

    let pages = List.length blogs

    let getFilenameForIndex i =
        if i = 0 then sprintf "index.html" else sprintf "posts/page%i.html" i

    let layoutForPostSet i blogs =
        let pageLink i max op =
            if i = max then "#" else "/" + getFilenameForIndex (op i 1)

        let nextPage = pageLink i (pages - 1) (+)
        let previousPage = pageLink i 0 (-)

        Layout.layout
            ctx
            "Blog"
            [ section [ Class "hero is-info is-medium is-bold" ] [
                div [ Class "hero-body" ] [
                    div [ Class "container has-text-centered" ] [
                        h1 [ Class "title" ] [ headline ]
                    ]
                ]
              ]
              div [ Class "container" ] [
                  section [ Class "articles" ] [
                      div [ Class "column is-8 is-offset-2" ] blogs
                  ]
              ]
              div [ Class "container" ] [
                  div [ Class "container has-text-centered" ] [
                      a [ Href previousPage ] [
                          !! "Previous"
                      ]
                      span
                          []
                          (if (i + 1) < pages then
                               [ !!(sprintf "%i of %i" (i + 1) pages)
                                 a [ Href nextPage ] [ !! "Next" ] ]
                           else
                               [])
                  ]
              ] ]

    blogs
    |> List.mapi (fun i bs -> getFilenameForIndex i, layoutForPostSet i bs |> Layout.render ctx)

let generate (ctx: SiteContents) (projectRoot: string) (page: string) = generate' ctx page
