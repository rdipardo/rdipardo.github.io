#r "../_lib/Fornax.Core.dll"

open Html

type SiteAuthor = { name: string; email: string }
type Attribution = { notice: string }

type SiteInfo =
    { title: string
      canonical: string
      headline: HtmlElement
      description: string
      author: SiteAuthor
      credits: Attribution
      postPageSize: int }

let loader (projectRoot: string) (siteContent: SiteContents) =
    let siteInfo =
        { title = "Heredocs"
          canonical = "https://rdipardo.github.io"
          headline = !! ""
          description = "Reflections, tips, and works in progress"
          author =
              { name = "Robert Di Pardo"
                email = "dipardo.r@gmail.com" }
          credits =
              { notice = """
            <div>
                <a rel="license" href="https://creativecommons.org/publicdomain/zero/1.0/">
                    <img style="border-style:none;display:block;margin:0 auto;" src="https://licensebuttons.net/p/zero/1.0/88x31.png" alt="CC0" /></a>
                <div class="license">
                    To the extent possible under law,
                    <a rel="dct:publisher" href="https://www.pexels.com/@startup-stock-photos">
                        <span property="dct:title">Startup Stock Photos</span></a>
                        has waived all copyright and related or neighboring rights to
                        <a href="https://www.pexels.com/photo/person-holding-black-pen-while-sitting-7095"><span property="dct:title">Person Holding Black Pen While Sitting</span></a>.
                </div>
            </div>
        """   }
          postPageSize = 5 }

    siteContent.Add(siteInfo)
    siteContent
