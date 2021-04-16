#r "../_lib/Fornax.Core.dll"

#if !FORNAX
#load "../loaders/postloader.fsx"
#load "../loaders/pageloader.fsx"
#load "../loaders/linkloader.fsx"
#load "../loaders/globalloader.fsx"
#endif

open Html
open System

let private injectWebsocketCode (webpage: string) =
    let websocketScript = script [ Src "/js/websocket.js"; Defer true ] []

    let head = "<head>"
    let index = webpage.IndexOf head
    let tag = HtmlElement.ToString websocketScript
    webpage.Insert((index + head.Length + 1), sprintf "\t%s\n" tag)

let private toDateString (date: DateTime option) =
    Option.defaultValue DateTime.Now date |> fun n -> n.ToString("yyyy-MM-dd")

let layout (ctx: SiteContents) active bodyCnt =
    let pages = ctx.TryGetValues<Pageloader.Page>() |> Option.defaultValue Seq.empty
    let siteInfo = ctx.TryGetValue<Globalloader.SiteInfo>()
    let baseUrl = siteInfo |> Option.map (fun si -> si.canonical) |> Option.defaultValue ""
    let ttl = siteInfo |> Option.map (fun si -> si.title) |> Option.defaultValue ""
    let headline = siteInfo |> Option.map (fun si -> si.headline) |> Option.defaultValue !! ""
    let desc = siteInfo |> Option.map (fun si -> si.description) |> Option.defaultValue ""

    let subtitle =
        if String.IsNullOrEmpty(active) then
            if String.IsNullOrEmpty(desc) then "" else $" | {desc}"
        else
            $" | {active}"

    let attribution =
        siteInfo
        |> Option.map (fun si -> si.credits)
        |> Option.defaultValue { notice = "" }

    let siteAuthor =
        siteInfo
        |> Option.map (fun si -> si.author)
        |> Option.defaultValue { name = ""; email = "" }

    let socialMedia =
        let sites = ctx.TryGetValues<Linkloader.Link>() |> Option.defaultValue Seq.empty

        sites
        |> Seq.map (
            (fun s ->
                match s.site.ToLower() with
                | "github" -> "fa-github"
                | "gitlab" -> "fa-gitlab"
                | "bitbucket" -> "fa-bitbucket"
                | "linkedin" -> "fa-linkedin-square"
                | "twitter" -> "fa-twitter"
                | "facebook" -> "fa-facebook-official"
                | "reddit" -> "fa-reddit"
                | "instagram" -> "fa-instagram"
                | "tumblr" -> "fa-tumblr-square"
                | "deviantart" -> "fa-deviantart"
                | _ -> ""
                |> (fun (s: Linkloader.Link) t -> (s.target, t)) s)
            >> (fun s ->
                let siteName = (snd s).Split('-')
                let site = Array.tryHead siteName.[1..]

                a [ Href(fst s)
                    Class "navicon"
                    HtmlProperties.Title(
                        site
                        |> function
                        | Some s -> (sprintf "Find %s on %s" siteAuthor.name s)
                        | None -> ""
                    ) ] [
                    span [ Class(sprintf "media-icon fa %s" (snd s)); Custom("aria-hidden", "true") ] []
                ])
        )
        |> Seq.toList

    let menuEntries =
        pages
        |> Seq.map
            (fun p ->
                let cls = if p.title = active then "navbar-item is-active" else "navbar-item"

                a [ Class cls; Href p.link ] [ !!p.title ])
        |> Seq.toList

    let footer =
        seq {
            footer [ Class "is-dark" ] [
                div [ Class "columns" ] [
                    div [ Class "stacked" ] [
                        p [] [
                            !!(sprintf "&copy;&nbsp;%d&nbsp;" DateTime.Now.Year)
                            a [ Href(sprintf "mailto:%s" siteAuthor.email) ] [ !!siteAuthor.name ]
                        ]
                        !!attribution.notice
                    ]
                    div [ Class "stacked" ] [ p [ Class "contact" ] socialMedia ]
                    div [ Class "stacked" ] [
                        p [] [
                            !! "Site generated with "
                            a [ Href "https://ionide.io/Tools/fornax.html" ] [ !! "Fornax" ]
                        ]
                    ]
                ]
            ]
        }

    let scripts =
        seq {
            script [ Src "/js/bulma.js"; Defer true ] []

            script [ Src "/js/replace_icon.js"; Defer true ] []
        }

    html [ Lang "en" ] [
        head [] [
            meta [ CharSet "utf-8" ]
            meta [ Name "viewport"; Content "width=device-width, initial-scale=1" ]
            title [] [ !!($"{ttl}{subtitle}") ]
            meta [ Name "generator"; Content "fornax" ]
            meta [ Property "og:title"; Content ttl ]
            meta [ Property "og:site_name"; Content ttl ]
            meta [ Property "og:url"; Content baseUrl ]
            meta [ Name "author"; Content siteAuthor.name ]
            meta [ Name "description"; Content desc ]
            meta [ Property "og:description"; Content desc ]
            link [ Rel "canonical"; Href baseUrl ]
            link [ Rel "icon"
                   HtmlProperties.Type "image/x-icon"
                   Sizes "32x32"
                   Href "/images/favicon.ico" ]
            link [ Rel "stylesheet"
                   Media "all"
                   Href "https://fonts.googleapis.com/css2?family=Quicksand:wght@600;700&display=swap" ]
            link [ Rel "stylesheet"
                   Media "all"
                   Href "https://fonts.googleapis.com/css2?family=Nunito&display=swap" ]
            link [ Rel "stylesheet"
                   Media "screen"
                   Href "https://unpkg.com/bulma@0.8.0/css/bulma.min.css" ]
            link [ Rel "stylesheet"; Media "screen"; Href "/style/style.css" ]
            (if String.IsNullOrEmpty((HtmlElement.ToString headline).Trim()) then
                 !! """<style>.hero-body { height: 200px; }</style>"""
             else
                 !! "")
        ]
        body [] [
            nav [ Class "navbar is-dark" ] [
                div [ Class "container" ] [
                    div [ Class "navbar-brand" ] [
                        a [ Class "navbar-item navicon"
                            HtmlProperties.Title "View the source code"
                            Href "https://github.com/rdipardo/rdipardo.github.io" ] [
                            span [ Class "media-icon fa fa-github"; Custom("aria-hidden", "true") ] []
                        ]
                        span [ Class "navbar-burger burger"; Custom("data-target", "navbarMenu") ] [
                            span [] []
                            span [] []
                            span [] []
                        ]
                    ]
                    div [ Id "navbarMenu"; Class "navbar-menu" ] (List.append menuEntries [])
                ]
            ]
            yield! bodyCnt
            yield! footer
            yield! scripts
        ]
    ]

let publications (post: Postloader.Post) = (toDateString post.published, toDateString post.updated)

let postLayout (useSummary: bool) (post: Postloader.Post) =
    div [ Class "card article" ] [
        div [ Class "card-content" ] [
            div [ Class "media-content has-text-centered" ] [
                p [ Class "title article-title" ] [
                    a [ Name "main-content"; Href(sprintf "%s#main-content" post.link) ] [
                        !!post.title
                    ]
                ]
                p [ Class "subtitle is-6 article-subtitle" ] [
                    !!(sprintf
                        "%s%s"
                        (if post.author.IsSome then
                             sprintf "By&nbsp;%s&nbsp;&bullet;&nbsp;" (defaultArg post.author "Anonymous")
                         else
                             "")
                        (fst (publications post)))
                    if post.updated.IsSome then
                        !!(sprintf "&nbsp;&bullet;&nbsp;updated %s" (snd (publications post)))
                ]
            ]
            div [ Class "content article-body is-post" ] [
                (if useSummary then
                     div [ Class "has-text-centered" ] [
                         div [ HtmlProperties.Style [ FontStyle("italic") ] ] [
                             !!(post.summary + " . . .")
                         ]
                     ]
                 else
                     !!post.content)
            ]
        ]
    ]

let render (ctx: SiteContents) cnt =
    let disableLiveRefresh =
        ctx.TryGetValue<Postloader.PostConfig>()
        |> Option.map (fun n -> n.disableLiveRefresh)
        |> Option.defaultValue false

    cnt
    |> HtmlElement.ToString
    |> sprintf "<!DOCTYPE html>\n%s"
    |> fun dom -> if disableLiveRefresh then dom else injectWebsocketCode dom
